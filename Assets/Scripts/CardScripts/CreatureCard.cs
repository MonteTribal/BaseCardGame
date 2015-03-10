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

	private CardTypes[] attackTargets = new CardTypes[2] {CardTypes.Creature, CardTypes.FieldMarkerPlayer};

	//--- mono functions ---
	protected override void Start ()
	{
		base.Start ();
	}

	protected override void OnMouseDown ()
	{
		base.OnMouseDown();
		if(transform.root.name != "Player") //you cant play your opponenets spells! ..silly..
		{
			return;
		}

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
		gm.GetComponent<SelectionMaster>().setPotential(this.gameObject, attackTargets );
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

	protected override void Update ()
	{
		base.Update ();
		if(stamina <= 0)
		{
			kill();
		}
	}

	// --- creature card specific functions ---
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

	protected IEnumerator attack()
	{
		//Debug.Log("In attack");

		if(!gm.GetComponent<SelectionMaster>().getNewTarget(attackTargets, this.gameObject))
		{
			Debug.Log(myname + " forced closed a box to attack");
			gm.GetComponent<GUIMaster>().forceCloseBox(this.gameObject);
			if(!gm.GetComponent<SelectionMaster>().getNewTarget(attackTargets, this.gameObject))
			{
				Debug.LogException(new Exception("Could not open box so " + myname + "  could attack a new target"));
				yield break; //if false, then this fails. should do more here
			}										
		}

		GameObject target = null;
		waitingOnTarget = true;

		while(waitingOnTarget)
		{
			target = gm.GetComponent<SelectionMaster>().getTarget();
			if(target != null)
			{
				if(target.GetComponent<CreatureCard>())
				{
					if(!target.GetComponent<CreatureCard>().hasBeenCast)
					{
						target = null;
						yield return 0;
						continue;
					}
					waitingOnTarget = false;
				}

			}
			Debug.Log(myname + " is in attack...");
			/*if(Input.GetKeyDown(KeyCode.Space))
			{
				break;
			}*/
			yield return 0;
		}


		if(target.GetComponent<CreatureCard>())
		{
			int targetOrigStamina = target.GetComponent<CreatureCard>().stamina;
			target.GetComponent<CreatureCard>().stamina -= stamina;
			if(!target.GetComponent<CreatureCard>().getIsExhausted())
			{
				stamina -= targetOrigStamina;
			}
			gm.GetComponent<SelectionMaster>().targetAquired(this.gameObject);		
		}
		else if(target.GetComponent<FieldDisplayDeck>())
		{
			target.transform.root.GetComponent<Resources>().health -= stamina;
			gm.GetComponent<SelectionMaster>().targetAquired(this.gameObject);
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

		yield break;
	}

	protected void exhaust()
	{
		transform.RotateAround(transform.position, Vector3.back, 30f);
		canAttack = false;
		canActivate = false;
		isExhausted = true;
	}

	public bool getIsExhausted()
	{
		return isExhausted;
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

	// --- GUI functions ---
	void OnGUI()
	{
		if(showGuiOptions) //this is why changing phases doesnt close the gui...
		{
			Rect EffectBox = new Rect(Screen.width/2 - (Screen.width/3)/2, Screen.height/2 - (Screen.height/3)/2, Screen.width/3, Screen.height/3);
			GUI.BeginGroup(EffectBox);
			{
				GUI.Box (new Rect(0, 0, EffectBox.width, EffectBox.height) , myname);
				GUI.Box (new Rect(0, EffectBox.height/10, EffectBox.width, EffectBox.height/2), popupText, skin.box);
				GUI.Box (new Rect(0, EffectBox.height*6/10, EffectBox.width, EffectBox.height/10), "Stamina: " + stamina.ToString());

				if(gm.GetComponent<GameMaster>().getCurrentPlayer() == myStuff.playerID)
				{
					//if in your hand and can cast creature
					if(!hasBeenCast && canCast())
					{
						if(GUI.Button(new Rect(0, EffectBox.height*3/4, EffectBox.width, EffectBox.height/4), "Cast " + myname))
						{
							cast();
							gm.GetComponent<GameMaster>().fieldFocus = true;		
							closePopups();
						}
					}
					//if on the field, during setup phase, and has an effect
					else if(hasBeenCast && gm.GetComponent<GameMaster>().currentPhase == GameMaster.turnPhases.Setup)
					{
						if(effectType == EffectTypes.OnActivate && canActivate)
						{
							if(GUI.Button(new Rect(0, EffectBox.height*3/4, EffectBox.width, EffectBox.height/4), "Activate Effect"))
							{
								OnActivateSkill();
								gm.GetComponent<GameMaster>().fieldFocus = true;		
								closePopups();
							}
						}
						else if(effectType == EffectTypes.OnActivate)
						{
							GUI.Box(new Rect(0, EffectBox.height*3/4, EffectBox.width, EffectBox.height/4), "Effect cannot be used");
						}
					}
					else if(hasBeenCast && gm.GetComponent<GameMaster>().currentPhase == GameMaster.turnPhases.Attack && canAttack)
					{
						if(GUI.Button(new Rect(0, EffectBox.height*3/4, EffectBox.width, EffectBox.height/4), "ATTACK!"))
						{
							gm.GetComponent<GameMaster>().fieldFocus = true;		
							closePopups();
							//need to close popup *before* attacking
							StartCoroutine("attack");
							//pop up says "Choose attack target"
						}
					}
					//escape button
					if(GUI.Button(new Rect(EffectBox.width*9/10, EffectBox.height*0, EffectBox.width/10, EffectBox.height/10), "X"))
					{
						gm.GetComponent<GameMaster>().fieldFocus = true;
						closePopups();
					}
				}
			}
			GUI.EndGroup();
		}
	}
}
