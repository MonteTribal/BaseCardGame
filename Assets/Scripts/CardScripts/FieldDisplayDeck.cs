using UnityEngine;
using System.Collections;

public class FieldDisplayDeck : FieldDisplayCard {

	protected override void OnMouseEnter ()
	{
		base.OnMouseEnter();
		gm.GetComponent<SelectionMaster>().setPotential(this.gameObject, new CardTypes[1] {CardTypes.FieldMarkerPlayer} );
	}
	
	protected override void OnMouseDown ()
	{
		base.OnMouseDown();
	}

	public int getPlayerHP()
	{
		return myStuff.health;
	}
}
