using UnityEngine;
using System.Collections;

public class TestCreatureOnAttackEffect : CreatureCard {

	protected override void Start ()
	{
		base.Start ();
		setPopupText("When this creature attacks, it gains a Stamina.");
	}
	
	public override void OnAttackSkill ()
	{
		//base.OnAttackSkill ();
		stamina++;
	}

}
