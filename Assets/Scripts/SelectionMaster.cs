using UnityEngine;
using System.Collections;

public class SelectionMaster : MonoBehaviour {

	private GameObject selected = null;
	private bool display = false;

	public void setSelected(GameObject newTarget)
	{
		selected = newTarget;
	}

	public GameObject getTarget()
	{
		return selected;
	}

	public void getNewTarget(Card.CardTypes[] targetTypes)
	{
		if(!GetComponent<GUIMaster>().canOpenNewBox())
		{
			Debug.LogWarning("If a GUI box is already open, you have to close it before trying to get a new target");
			return; //
		}

		selected = null;
		guiOn();
	}

	private void guiOn()
	{
		display = true;
		GetComponent<GUIMaster>().openNewBox(GUIMaster.displayTypes.Text, this.gameObject);
	}

	void OnGUI()
	{
		if(display)
		{
			Rect EffectBox = new Rect(Screen.width/2 - (Screen.width/3)/2, Screen.height/2 - (Screen.height/3)/2, Screen.width/3, Screen.height/3);
			GUI.BeginGroup(EffectBox);
			{
				GUI.Box (new Rect(0, 0, EffectBox.width, EffectBox.height) ,"BOX");
			}
			GUI.EndGroup();

			if(GUI.Button(new Rect(EffectBox.width*9/10, EffectBox.height*0, EffectBox.width/10, EffectBox.height/10), "X"))
			{
				GameObject.Find("GameMaster").GetComponent<GameMaster>().fieldFocus = true;
				display = false;
				GetComponent<GUIMaster>().closeBox(this.gameObject);
			}
		}
	}



}
