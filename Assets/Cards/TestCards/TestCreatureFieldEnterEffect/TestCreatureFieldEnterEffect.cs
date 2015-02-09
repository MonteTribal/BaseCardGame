using UnityEngine;
using System.Collections;

public class TestCreatureFieldEnterEffect : CreatureCard {

	public override void OnEnterFieldSkill ()
	{
		//base.OnEnterFieldSkill();
		transform.root.GetComponent<Dealer>().drawCard();
	}
}
