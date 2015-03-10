using UnityEngine;
using System.Collections;

public class FieldDisplayGrave : FieldDisplayCard {
	

	protected override void OnMouseEnter ()
	{
		return;
	}
	
	protected override void OnMouseDown ()
	{
		if(gm.GetComponent<GUIMaster>().canOpenNewBox())
		{
			display = true;
			gm.GetComponent<GUIMaster>().openNewBox(GUIMaster.displayTypes.List, this.gameObject);
		}
	}

	protected override void OnMouseUp ()
	{
		if(gm.GetComponent<GUIMaster>().currentlyDisplaying == GUIMaster.displayTypes.List)
		{
			display = false;
			gm.GetComponent<GUIMaster>().closeBox(this.gameObject);
		}
	}

	protected override void OnMouseExit ()
	{
		return;
	}

	protected override void Update ()
	{
		base.Update ();
	}

	void OnGUI()
	{
		if(display)
		{
			Rect GUIBox = new Rect(Screen.width/4, Screen.height/4, Screen.width/2, Screen.height/2);
			GUI.BeginGroup(GUIBox);
			{
				GUI.Box(new Rect(0, 0, GUIBox.width, GUIBox.height), myStuff.name + "'s Grave");
				Rect listBox = new Rect (4, GUIBox.height/10, GUIBox.width-8, GUIBox.height*9/10-4);
				GUI.BeginGroup(listBox);
				{
					string cardsInGrave = "";

					foreach(GameObject card in myStuff.grave)
					{
						cardsInGrave += card.name + '\n';
					}

					GUI.Box(new Rect(0, 0, listBox.width, listBox.height), "");

				}
				GUI.EndGroup();
			}
			GUI.EndGroup();
		}
	}
}
