/*
 * BASE CLASS FOR SPELLS
 * 
 */

using UnityEngine;
using System.Collections;

public class SpellCard : Card {

	protected override void Start ()
	{
		base.Start ();
		cardType = CardTypes.Spell;
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
			showGuiOptions = true;
			gm.GetComponent<GameMaster>().fieldFocus = false;
			gm.GetComponent<GUIMaster>().openNewBox(GUIMaster.displayTypes.Card, this.gameObject);
			base.OnMouseExit();
		}
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
				if(canCast())
				{
					if(GUI.Button(new Rect(0, EffectBox.height*3/4, EffectBox.width, EffectBox.height/4), "Cast " + myname))
					{
						cast();
						gm.GetComponent<GameMaster>().fieldFocus = true;		
						showGuiOptions = false;
						gm.GetComponent<GUIMaster>().closeBox(this.gameObject);
					}

					if(GUI.Button(new Rect(EffectBox.width*9/10, EffectBox.height*0, EffectBox.width/10, EffectBox.height/10), "X"))
					{
						gm.GetComponent<GameMaster>().fieldFocus = true;
						showGuiOptions = false;
						gm.GetComponent<GUIMaster>().closeBox(this.gameObject);
					}
				}
			}
			GUI.EndGroup();
			
		}
	}

	protected override void OnMouseEnter()
	{
		if(GameObject.Find("GameMaster").GetComponent<GameMaster>().fieldFocus == false)
		{
			return;
		}
		base.OnMouseEnter();
		//GUI- show effects?
	}
	
	protected override void OnMouseExit()
	{
		if(GameObject.Find("GameMaster").GetComponent<GameMaster>().fieldFocus == false)
		{
			return;
		}
		base.OnMouseExit();
		//hide effects
	}

	protected void postCast()
	{
		transform.parent.root.GetComponent<Resources>().loseEssence(cost);
		//Debug.Log("CAST " + myname);
		transform.parent.root.GetComponent<Dealer>().moveToGraveFromHand(this.gameObject);
	}

	protected virtual void spellAffect()
	{
		//Debug.Log("Spell base spellAffect");
	}

	protected override void cast ()
	{
		spellAffect();
		postCast();
	}
	
}
