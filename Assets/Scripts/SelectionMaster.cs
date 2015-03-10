using UnityEngine;
using System.Collections;
using System;

public class SelectionMaster : MonoBehaviour {

	private GameObject selected = null;
	private GameObject selector = null;
	private bool display = false;
	private Card.CardTypes[] validTargetTypes = null;

	private GameObject potentialSelected = null;

	public void setPotential(GameObject potential, Card.CardTypes[] validTargets)
	{
		foreach(Card.CardTypes type in validTargets)
		{
			if(potential.GetComponent<Card>().getCardType() == type)
			{
				potentialSelected = potential;
				return;
			}
		}
	}

	public void clearPotential()
	{
		potentialSelected = null;
	}

	public void setSelected(GameObject newTarget)
	{
		if(validTargetTypes != null)
		{
			foreach(Card.CardTypes type in validTargetTypes)
			{
				if(newTarget.GetComponent<Card>().getCardType() == type)
				{
					selected = newTarget;
					return;
				}
			}
		}
	}

	public GameObject getTarget()
	{
		return selected;
	}

	public bool getNewTarget(Card.CardTypes[] targetTypes, GameObject targetter)
	{
		if(!GetComponent<GUIMaster>().canOpenNewBox())
		{
			Debug.LogWarning("If a GUI box is already open, you have to close it before trying to get a new target");
			return false; //
		}

		validTargetTypes = targetTypes;
		selected = null;
		selector = targetter;
		guiOn();
		return true;
	}

	private void guiOn()
	{
		display = true;
		GetComponent<GUIMaster>().openNewBox(GUIMaster.displayTypes.Text, selector);
	}

	public void targetAquired(GameObject targetter)
	{
		//if(targetter != selector)
		//{
			if(GetComponent<GUIMaster>().closeBox(targetter))
			{
				display = false;
			}
			else
			{
				Debug.LogWarning(targetter.name + " was not the one who opened the targetting box..?");
			}
		//}
		//else
		//{
		//	Debug.LogException(new Exception(targetter.name + " tried to claim a target meant for " + selector.name));
		//}
	}

	void OnGUI()
	{
		if(display)
		{
			Rect EffectBox = new Rect(Screen.width/2 - (Screen.width/3)/2, Screen.height/2 - (Screen.height/3)/2, Screen.width/3, Screen.height/3);
			GUI.BeginGroup(EffectBox);
			{
				GUI.Box (new Rect(0, 0, EffectBox.width, EffectBox.height) ,"Selecting BOX");

				string targettingStr = getTargettingText();

				GUI.Box (new Rect(0, EffectBox.height/10, EffectBox.width, EffectBox.height/2), targettingStr);

				if(GUI.Button(new Rect(EffectBox.width*9/10, EffectBox.height*0, EffectBox.width/10, EffectBox.height/10), "X"))
				{
					GameObject.Find("GameMaster").GetComponent<GameMaster>().fieldFocus = true;
					display = false;
					selector.GetComponent<CreatureCard>().StopAllCoroutines(); //this might break stuff later? 
					selector = null;
					validTargetTypes = null;
					GetComponent<GUIMaster>().closeBox(selector);
				}
			}
			GUI.EndGroup();


		}
	}

	private string getTargettingText()
	{
		string targettingStr = "";
		targettingStr += selector.GetComponent<Card>().myname;
		if(selector.GetComponent<CreatureCard>())
		{
			targettingStr += " (Stamina: " + selector.GetComponent<CreatureCard>().stamina + ")";
		}
		if(potentialSelected)
		{
			targettingStr += " to target ";

			if(potentialSelected.GetComponent<CreatureCard>())
			{
				targettingStr += potentialSelected.GetComponent<Card>().myname;
				targettingStr += " (Stamina: " + potentialSelected.GetComponent<CreatureCard>().stamina + ")";
			}
			else if(potentialSelected.GetComponent<FieldDisplayCard>())
			{
				targettingStr += " (HP: " + potentialSelected.GetComponent<FieldDisplayDeck>().getPlayerHP().ToString() + ")";
			}
			else
			{
				targettingStr += potentialSelected.name;
			}
		}
		else
		{
			targettingStr += "_";
		}
		return targettingStr;
	}
}
