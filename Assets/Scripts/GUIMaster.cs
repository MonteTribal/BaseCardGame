using UnityEngine;
using System.Collections;
using System;

public class GUIMaster : MonoBehaviour {

	private bool boxOpen = false;

	public enum displayTypes{None, List, Card};
	public displayTypes currentlyDisplaying;

	public bool canOpenNewBox()
	{
		return !boxOpen;
	}

	public void openNewBox(displayTypes boxType)
	{
		if(canOpenNewBox())
		{
			boxOpen = true;
			currentlyDisplaying = boxType;
		}
	}

	public void closeBox()
	{
		boxOpen = false;
		currentlyDisplaying = displayTypes.None;
	}

	void Update()
	{
		if(boxOpen == false && currentlyDisplaying != displayTypes.None)
		{
			Debug.LogException(new Exception("GUIMaster is not synched correctly"));
		}
	}

}
