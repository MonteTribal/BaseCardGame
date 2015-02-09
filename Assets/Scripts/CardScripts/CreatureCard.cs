/*
 * BASE CLASS FOR CREATURES
 * 
 */

using UnityEngine;
using System.Collections;

public class CreatureCard : Card {

	public int stamina;

	public enum EffectTypes{OnAttack, OnDeath, OnActivate, OnEnter, OnTurnStart, OnTurnEnd, None};
	public EffectTypes effectType = EffectTypes.None;

	private bool hasBeenCast = false;
	private bool canAttack = false;
	private bool canActivate = false;
	private bool isExhausted = false;

	private bool showGuiOptions = false;

	public string popupText;

	public override void Start ()
	{
		base.Start ();
	}

	public override void OnMouseDown ()
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
			showGuiOptions = true;
			gm.GetComponent<GameMaster>().fieldFocus = false;
			base.OnMouseExit();
		}
	}

	public override void OnMouseEnter()
	{
		if(gm.GetComponent<GameMaster>().fieldFocus == false)
		{
			return;
		}
		base.OnMouseEnter();
		//GUI- show effects?
	}

	public override void OnMouseExit()
	{
		if(gm.GetComponent<GameMaster>().fieldFocus == false)
		{
			return;
		}
		base.OnMouseExit();
		//hide effects
	}


	public override void cast()
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

	public void attack()
	{
		exhaust();
		if(effectType == EffectTypes.OnAttack)
		{
			OnAttackSkill();
		}
	}

	public void exhaust()
	{
		transform.RotateAround(transform.position, Vector3.back, 30f);
		canAttack = false;
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
		}
	}

	public void kill()
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
				GUI.Box (new Rect(0, 0, EffectBox.width, EffectBox.height) ,"BOX");
				GUI.Box (new Rect(0, EffectBox.height/10, EffectBox.width, EffectBox.height/2), popupText, skin.box);
				GUI.Box (new Rect(0, EffectBox.height*6/10, EffectBox.width, EffectBox.height/10), "Stamina: " + stamina.ToString());


				if(!hasBeenCast && canCast())
				{
					if(GUI.Button(new Rect(0, EffectBox.height*3/4, EffectBox.width, EffectBox.height/4), "Cast " + myname))
					{
						cast();
						GameObject.Find("GameMaster").GetComponent<GameMaster>().fieldFocus = true;		
						showGuiOptions = false;
					}
				}
				else if(hasBeenCast && GameObject.Find("GameMaster").GetComponent<GameMaster>().currentPhase == GameMaster.turnPhases.Setup)
				{
					if(effectType == EffectTypes.OnActivate && canActivate)
					{
						if(GUI.Button(new Rect(0, EffectBox.height*3/4, EffectBox.width, EffectBox.height/4), "Activate Effect"))
						{
							OnActivateSkill();
							GameObject.Find("GameMaster").GetComponent<GameMaster>().fieldFocus = true;		
							showGuiOptions = false;
						}
					}
					else if(effectType == EffectTypes.OnActivate)
					{
						GUI.Box(new Rect(0, EffectBox.height*3/4, EffectBox.width, EffectBox.height/4), "Effect has been used");
					}
				}
				else if(GameObject.Find("GameMaster").GetComponent<GameMaster>().currentPhase == GameMaster.turnPhases.Attack && canAttack)
				{
					if(GUI.Button(new Rect(0, EffectBox.height*3/4, EffectBox.width, EffectBox.height/4), "ATTACK!"))
					{
						attack();
						GameObject.Find("GameMaster").GetComponent<GameMaster>().fieldFocus = true;		
						showGuiOptions = false;
					}
				}

				if(GUI.Button(new Rect(EffectBox.width*9/10, EffectBox.height*0, EffectBox.width/10, EffectBox.height/10), "X"))
				{
					GameObject.Find("GameMaster").GetComponent<GameMaster>().fieldFocus = true;
					showGuiOptions = false;
				}
			}
			GUI.EndGroup();
		}
	}
}
