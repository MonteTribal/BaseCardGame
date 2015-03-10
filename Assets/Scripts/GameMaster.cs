using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameMaster : MonoBehaviour {
	
	private List<GameObject> players = new List<GameObject>();
	private int currentPlayer = 0;

	public enum turnPhases{PreDraw = 0, Draw, Setup, Attack, PostAttack}; //post attack 
	public turnPhases currentPhase = turnPhases.PreDraw;

	public bool fieldFocus = true; // you are looking at the field, not at an effect or targeting box

	// Use this for initialization
	void Start () 
	{
		GameObject[] ps = GameObject.FindGameObjectsWithTag("Player");
		foreach(GameObject p in ps)
		{
			players.Add(p);
		}

		if(players[0].name != "Player") //hey, Im first!!! :D
		{
			players.Reverse();
		}

		foreach(GameObject player in players)
		{
			Dealer p = player.GetComponent<Dealer>();
			p.drawCard(5);
			player.GetComponent<Resources>().playerID = players.IndexOf(player);
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
		GameObject thisPlayer = players[currentPlayer];
		GameObject otherPlayer;
		try{
			otherPlayer = players[currentPlayer+1];
		}
		catch{
			otherPlayer = players[0];
		}

		if(currentPhase == turnPhases.PreDraw) //or == 0
		{
			//Debug.Log("Time for player " + currentPlayer.ToString() + " to start.");
			//for when creatures have OnTurnStart effects. currently none
			currentPhase += 1;
			thisPlayer.GetComponent<Resources>().addEssence(thisPlayer.GetComponent<Resources>().turnCount);
			foreach(GameObject creature in thisPlayer.GetComponent<Resources>().field)
			{
				creature.GetComponent<CreatureCard>().refresh();
				if(creature.GetComponent<CreatureCard>().effectType == CreatureCard.EffectTypes.OnTurnStart)
				{
					creature.GetComponent<CreatureCard>().OnTurnStartSkill();
				}
			}
		}
		else if(currentPhase == turnPhases.Draw)
		{
			players[currentPlayer].GetComponent<Dealer>().drawCard();
			currentPhase += 1;
		}
		else if(currentPhase == turnPhases.Setup)
		{
			//Debug.Log("Time for player " + currentPlayer.ToString() + " to cast things.");
			if(Input.GetKeyDown(KeyCode.Space))
			{
				currentPhase += 1;
			}
    	}
		else if(currentPhase == turnPhases.Attack)
		{
			//Debug.Log("Time for player " + currentPlayer.ToString() + " to attack things.");
			if(Input.GetKeyDown(KeyCode.Space))
			{
				currentPhase += 1;
      		}
   	 	}
		else if(currentPhase == turnPhases.PostAttack)
		{
			if(!GetComponent<GUIMaster>().canOpenNewBox())
			{
				GetComponent<GUIMaster>().forceCloseBox(this.gameObject);
			}
			currentPhase = 0;

			currentPlayer++;
			if(currentPlayer == players.Count)
			{
				currentPlayer = 0;
			}

			otherPlayer.GetComponent<Resources>().turnCount+=1;
    	}
	}

	public int getCurrentPlayer()
	{
		return currentPlayer;
	}

	public turnPhases getCurrentPhase()
	{
		return currentPhase;
	}

	public void incrementPhase()
	{
		currentPhase+=1;
	}

}
