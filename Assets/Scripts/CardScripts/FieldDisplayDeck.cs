using UnityEngine;
using System.Collections;

public class FieldDisplayDeck : FieldDisplayCard {

	protected override void Start ()
	{
		base.Start ();
		cardType = CardTypes.FieldMarkerPlayer;
	}

	protected override void OnMouseEnter ()
	{
		base.OnMouseEnter();
		gm.GetComponent<SelectionMaster>().setPotential(this.gameObject, new CardTypes[1] {CardTypes.FieldMarkerPlayer} );
	}
	
	protected override void OnMouseDown ()
	{
		base.OnMouseDown();
	}

	public void addPlayerHP(int hp_to_add)
	{
		myStuff.health += hp_to_add;
	}

	public void dropPlayerHealth(int hp_to_lose)
	{
		myStuff.health -= hp_to_lose;
	}

	public int getPlayerHP()
	{
		return myStuff.health;
	}
}
