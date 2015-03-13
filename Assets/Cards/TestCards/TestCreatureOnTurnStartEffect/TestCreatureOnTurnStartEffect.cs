using UnityEngine;
using System.Collections;

public class TestCreatureOnTurnStartEffect : CreatureCard {

	protected override void Start ()
	{
		base.Start ();
		setPopupText("At the start of each turn, add 1 Essence to you Reliquary.");
	}
	
	public override void OnTurnStartSkill ()
	{
		//base.OnTurnStartSkill ();
		myStuff.addEssence(1);
	}
}
