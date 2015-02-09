/*
 * BASE CLASS FOR SPELLS
 * 
 */

using UnityEngine;
using System.Collections;

public class SpellCard : Card {
	
	public override void OnMouseDown ()
	{
		if(canCast())
		{
			cast();
		}
	}


	public override void OnMouseEnter()
	{
		if(GameObject.Find("GameMaster").GetComponent<GameMaster>().fieldFocus == false)
		{
			return;
		}
		base.OnMouseEnter();
		//GUI- show effects?
	}
	
	public override void OnMouseExit()
	{
		if(GameObject.Find("GameMaster").GetComponent<GameMaster>().fieldFocus == false)
		{
			return;
		}
		base.OnMouseExit();
		//hide effects
	}

	public void postCast()
	{
		transform.parent.root.GetComponent<Resources>().loseEssence(cost);
		//Debug.Log("CAST " + myname);
		transform.parent.root.GetComponent<Dealer>().moveToGraveFromHand(this.gameObject);
	}

	public virtual void spellAffect()
	{
		//Debug.Log("Spell base spellAffect");
	}

	public override void cast ()
	{
		spellAffect();
		postCast();
	}
	
}
