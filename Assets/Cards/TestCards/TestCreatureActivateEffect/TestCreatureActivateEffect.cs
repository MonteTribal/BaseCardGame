using UnityEngine;
using System.Collections;

public class TestCreatureActivateEffect : CreatureCard {

	private int activations = 0;
	
	protected override void Start ()
	{
		base.Start ();
		popupText = "Activate: Gain 2 life. Using this ability twice exhausts this creature.";
	}

	public override void refresh ()
	{
		base.refresh ();
		activations = 0;
	}

	public override void OnActivateSkill ()
	{
		//base.OnActivateSkill ();
		if(canActivate)
		{
			transform.root.GetComponent<Resources>().health+=2;
			activations+=1;
		}
		if(activations == 2)
		{
			exhaust();
		}
	}
}
