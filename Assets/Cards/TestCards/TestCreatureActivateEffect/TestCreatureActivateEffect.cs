using UnityEngine;
using System.Collections;

public class TestCreatureActivateEffect : CreatureCard {

	private int activations = 0;

	public override void refresh ()
	{
		base.refresh ();
		activations = 0;
	}

	public override void OnActivateSkill ()
	{
		//base.OnActivateSkill ();
		transform.root.GetComponent<Resources>().health+=2;
		activations+=1;
		if(activations >= 2)
		{
			exhaust();
		}
	}
}
