using UnityEngine;
using System.Collections;

public class TestCreatureFieldEnterEffect : CreatureCard {

	protected override void Start ()
	{
		base.Start ();
		setPopupText("When this creature enters the field, draw a card.");
	}

	public override void OnEnterFieldSkill ()
	{
		//base.OnEnterFieldSkill();
		transform.root.GetComponent<Dealer>().drawCard();
	}
}
