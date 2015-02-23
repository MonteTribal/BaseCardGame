/*
 * BASE CLASS FOR CREATURES
 * 
 */

using UnityEngine;
using System.Collections;
using System;

public class CreatureCard : Card {

	public int stamina;

	public enum EffectTypes{OnAttack, OnDeath, OnActivate, OnEnter, OnTurnStart, OnTurnEnd, None};
	public EffectTypes effectType = EffectTypes.None;

	protected bool hasBeenCast = false;
	protected bool canAttack = false;
	protected bool canActivate = false;
	protected bool isExhausted = false;

	private bool waitingOnTarget = false;

	private CardTypes[] attackTargets = new CardTypes[2] {CardTypes.Creature, CardTypes.FieldMarker};

	protected override void Start ()
	{
		base.Start ();
	}

	protected override void OnMouseDown ()
	{
		if(transform.root.name != "Player") //you cant play your opponenets spells! ..silly..
		{
			return;
		}

		/*
		if(!hasBeenCast)
		{
			cast();
		}
		else if(canAttack)
		{
			if(effectType == EffectTypes.OnActivate)
			{
				//OnActivateSkill();
				showGuiOptions = true;
				GameObject.Find("GameMaster").GetComponent<GameMaster>().fieldFocus = false;
				base.OnMouseExit();
			}
		}*/

		if(gm.GetComponent<GUIMaster>().canOpenNewBox())
		{
			gm.GetComponent<GUIMaster>().openNewBox(GUIMaster.displayTypes.Card, this.gameObject);
			showGuiOptions = true;
			gm.GetComponent<GameMaster>().fieldFocus = false;
			base.OnMouseExit();
		}
	}

	protected override void OnMouseEnter()
	{
		if(gm.GetComponent<GameMaster>().fieldFocus == false)
		{
			return;
		}
		base.OnMouseEnter();
		//GUI- show effects?
	}

	protected override void OnMouseExit()
	{
		if(gm.GetComponent<GameMaster>().fieldFocus == false)
		{
			return;
		}
		base.OnMouseExit();
		//hide effects
	}


	protected override void cast()
	{
		if(canCast())
		{
			transform.parent.root.GetComponent<Resources>().loseEssence(cost);
			//Debug.Log("CAST " + myname);
			transform.parent.root.GetComponent<Dealer>().moveToField(this.gameObject);
			hasBeenCast = true;
			canAttack = false;
			if(effectType == EffectTypes.OnEnter)
			{
				OnEnterFieldSkill();
			}
		}
		else
		{
			//Debug.Log("Not enough essence to cast " + myname);
		}
	}

	protected void attack()
	{
		waitingOnTarget = true;

		gm.GetComponent<SelectionMaster>().getNewTarget( attackTargets );

		GameObject target = null;

		while(waitingOnTarget)
		{
			target = gm.GetComponent<SelectionMaster>().getTarget();
			if(target != null)
			{
				waitingOnTarget = false;
			}
		}

		if(target.GetComponent<CreatureCard>())
		{
			int targetOrigStamina = target.GetComponent<CreatureCard>().stamina;
			target.GetComponent<CreatureCard>().stamina -= stamina;
			stamina -= targetOrigStamina;
		}
		else if(target.GetComponent<Resources>())
		{
			target.GetComponent<Resources>().health -= stamina;
		}
		else
		{
			Debug.LogException(new Exception("This shouldn't be possible... Invalid target for attack"));
		}

		exhaust();
		if(effectType == EffectTypes.OnAttack)
		{
			OnAttackSkill();
		}
	}

	protected void exhaust()
	{
		transform.RotateAround(transform.position, Vector3.back, 30f);
		canAttack = false;
		canActivate = false;
		isExhausted = true;
	}

	public virtual void refresh()
	{
		canAttack = true;
		if(effectType == EffectTypes.OnActivate)
		{
			canActivate = true;
		}
		if(isExhausted)
		{
			transform.RotateAround(transform.position, Vector3.back, -30f);
			isExhausted = false;
		}
	}

	protected void kill()
	{
		if(effectType == EffectTypes.OnDeath)
		{
			OnDeathSkill();
		}
		transform.parent.root.GetComponent<Dealer>().moveToGraveFromField(this.gameObject);
	}

	public virtual void OnActivateSkill()
	{
		//Debug.Log("Creature base OnActivateSkill");
		canActivate = false; //this limits it to once per turn
	}

	public virtual void OnTurnStartSkill()
	{
		//Debug.Log("Creature base OnTurnStartSkill");
  	}

	public virtual void OnTurnEndSkill()
	{
		//Debug.Log("Creature base OnTurnEndSkill");
	}

	public virtual void OnAttackSkill()
	{
		//Debug.Log("Creature base OnAttackSkill");
	}

	public virtual void OnDeathSkill()
	{
		//Debug.Log("Creature base OnDeathSkill");
    }

	public virtual void OnEnterFieldSkill()
	{
		//Debug.Log("Creature base OnEnterFieldSkill");
    }

	public void setPopupText(string text)
	{
		popupText = text;
	}

	void OnGUI()
	{
		if(showGuiOptions)
		{
			Rect EffectBox = new Rect(Screen.width/2 - (Screen.width/3)/2, Screen.height/2 - (Screen.height/3)/2, Screen.width/3, Screen.height/3);
			GUI.BeginGroup(EffectBox);
			{
				GUI.Box (new Rect(0, 0, EffectBox.width, EffectBox.height) , myname);
				GUI.Box (new Rect(0, EffectBox.height/10, EffectBox.width, EffectBox.height/2), popupText, skin.box);
				GUI.Box (new Rect(0, EffectBox.height*6/10, EffectBox.width, EffectBox.height/10), "Stamina: " + stamina.ToString());


				if(!hasBeenCast && canCast())
				{
					if(GUI.Button(new Rect(0, EffectBox.height*3/4, EffectBox.width, EffectBox.height/4), "Cast " + myname))
					{
						cast();
						gm.GetComponent<GameMaster>().fieldFocus = true;		
						showGuiOptions = false;
						gm.GetComponent<GUIMaster>().closeBox(this.gameObject);
					}
				}
				else if(hasBeenCast && gm.GetComponent<GameMaster>().currentPhase == GameMaster.turnPhases.Setup)
				{
					if(effectType == EffectTypes.OnActivate && canActivate)
					{
						if(GUI.Button(new Rect(0, EffectBox.height*3/4, EffectBox.width, EffectBox.height/4), "Activate Effect"))
						{
							OnActivateSkill();
							gm.GetComponent<GameMaster>().fieldFocus = true;		
							showGuiOptions = false;
							gm.GetComponent<GUIMaster>().closeBox(this.gameObject);
						}
					}
					else if(effectType == EffectTypes.OnActivate)
					{
						GUI.Box(new Rect(0, EffectBox.height*3/4, EffectBox.width, EffectBox.height/4), "Effect cannot be used");
					}
				}
				else if(gm.GetComponent<GameMaster>().currentPhase == GameMaster.turnPhases.Attack && canAttack)
				{
					if(GUI.Button(new Rect(0, EffectBox.height*3/4, EffectBox.width, EffectBox.height/4), "ATTACK!"))
					{
						attack();
						gm.GetComponent<GameMaster>().fieldFocus = true;		
						showGuiOptions = false;
						//pop up says "Choose attack target"
					}
				}

				if(GUI.Button(new Rect(EffectBox.width*9/10, EffectBox.height*0, EffectBox.width/10, EffectBox.height/10), "X"))
				{
					gm.GetComponent<GameMaster>().fieldFocus = true;
					showGuiOptions = false;
					gm.GetComponent<GUIMaster>().closeBox(this.gameObject);
				}
			}
			GUI.EndGroup();
		}
	}
}
