/*
 * Because I am dumb, this is a SECOND GUI script
 * GUIMaster is special because you make it special
 *  this makes it less special
 *  besides... you want these up at all times anyways. whats the big deal?
 */

using UnityEngine;
using System.Collections;

public class ResourcesDisplay : MonoBehaviour {

	public GUISkin skin; 

	private Resources playerResources;
	private GameMaster gm;

	public bool topLeft = false;
	public bool nextButton = false;
	private Rect rectToUse;

	// Use this for initialization
	void Start () 
	{
		playerResources = GetComponent<Resources>();
		gm = GameObject.Find("GameMaster").GetComponent<GameMaster>();
	}
	
	void OnGUI()
	{
		if(topLeft)
		{
			rectToUse = new Rect(Screen.width*.2f/10, Screen.height*0/10, Screen.width*1/10, Screen.height*5/10);
		}
		else
		{
			rectToUse = new Rect(Screen.width*8.8f/10, Screen.height*5/10, Screen.width*1/10, Screen.height*5/10);
		}

		Rect ResourcesBox = rectToUse;
		GUI.BeginGroup(ResourcesBox);
		{
			GUI.Box(new Rect(0, 0, ResourcesBox.width, ResourcesBox.height), "");
			GUI.Box(new Rect(0, ResourcesBox.height*0/10, ResourcesBox.width, ResourcesBox.height/10), "HP: " + playerResources.health.ToString(), skin.box);
			GUI.Box(new Rect(0, ResourcesBox.height*1/10, ResourcesBox.width, ResourcesBox.height/10), "Essence: " + playerResources.essence.ToString(), skin.box);
			GUI.Box(new Rect(0, ResourcesBox.height*2/10, ResourcesBox.width, ResourcesBox.height/10), "Turn #: " + (playerResources.turnCount - 1).ToString(), skin.box);
			GUI.Box(new Rect(0, ResourcesBox.height*3/10, ResourcesBox.width, ResourcesBox.height/10), "In Deck: " + playerResources.deck.Count.ToString(), skin.box);
			GUI.Box(new Rect(0, ResourcesBox.height*4/10, ResourcesBox.width, ResourcesBox.height/10), "In Grave: " + playerResources.grave.Count.ToString(), skin.box);

			if(nextButton && gm.getCurrentPlayer() == playerResources.playerID)
			{
				if(GUI.Button(new Rect(0, ResourcesBox.height*9/10, ResourcesBox.width, ResourcesBox.height/10), "End " + gm.getCurrentPhase().ToString()))
				{
					gm.incrementPhase();
				}
			}
		}
		GUI.EndGroup();
	}
}
