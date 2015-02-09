using UnityEngine;
using System.Collections;

public class FieldDisplayGrave : FieldDisplayCard {

	public override void OnMouseEnter ()
	{
		return;
	}
	
	public override void OnMouseDown ()
	{
		if(gm.GetComponent<GUIMaster>().canOpenNewBox())
		{
			display = true;
			gm.GetComponent<GUIMaster>().openNewBox(GUIMaster.displayTypes.List);
		}
	}

	public override void OnMouseUp ()
	{
		if(gm.GetComponent<GUIMaster>().currentlyDisplaying == GUIMaster.displayTypes.List)
		{
			display = false;
			gm.GetComponent<GUIMaster>().closeBox();
		}
	}

	public override void OnMouseExit ()
	{
		return;
	}

	public override void Update ()
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
					GUI.Box(new Rect(0, 0, listBox.width, listBox.height), "");

					foreach(GameObject card in myStuff.grave)
					{
						Debug.Log("HOW DO I SHOW CARDS?!");
					}

				}
				GUI.EndGroup();
			}
			GUI.EndGroup();
		}
	}
}
