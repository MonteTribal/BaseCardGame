using UnityEngine;
using System.Collections;
using System;

public class GUIMaster : MonoBehaviour {

	private bool boxOpen = false;

	public enum displayTypes{None, List, Card, Text}; //probably a redundant measure. 
	public displayTypes currentlyDisplaying;

	private GameObject objectWhoIsDisplaying;

	public bool canOpenNewBox()
	{
		return !boxOpen;
	}

	public void openNewBox(displayTypes boxType, GameObject openerObject) // i guess I could have this return true/false, for-going the if-blocks around calling this in other files
	{
		if(canOpenNewBox())
		{
			boxOpen = true;
			currentlyDisplaying = boxType;
			objectWhoIsDisplaying = openerObject;
		}
	}

	public bool closeBox(GameObject closer)
	{
		//Can only be called by Object who is currently displaying a GUI. It closes it's GUI
		if(objectWhoIsDisplaying == closer)
		{
			boxOpen = false;
			currentlyDisplaying = displayTypes.None;
			objectWhoIsDisplaying = null;
			return true;
		}
		else
		{
			Debug.Log(closer.name + " tried to close a box hold open by " + objectWhoIsDisplaying.name);
			return false;
		}
	}

	public void swapBoxOfSameType(GameObject swapper)
	{
		if(objectWhoIsDisplaying.GetComponent<Card>() && swapper.GetComponent<Card>())
		{
			closeBox(objectWhoIsDisplaying);
			openNewBox(currentlyDisplaying, swapper);
		}
	}

	public void forceCloseBox(GameObject closer)
	{
		Debug.Log(closer.name + " forced a box hold open by " + objectWhoIsDisplaying.name + " to close.");
		boxOpen = false;
		currentlyDisplaying = displayTypes.None;
		if(objectWhoIsDisplaying.GetComponent<CreatureCard>())
		{
			objectWhoIsDisplaying.GetComponent<CreatureCard>().closePopups();
		}
		objectWhoIsDisplaying = null;
	}

	void Update()
	{
		if(boxOpen == false && currentlyDisplaying != displayTypes.None)
		{
			Debug.LogException(new Exception("GUIMaster is not synched correctly"));
		}
	}

}
