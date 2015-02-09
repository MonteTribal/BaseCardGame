using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Resources : MonoBehaviour {

	public int playerID = 0;
	public int health=20;
	public int essence=0;
	public int maxEssence = 20;
	public List<GameObject> hand  = new List<GameObject>();
	public List<GameObject> deck  = new List<GameObject>();
	public List<GameObject> field = new List<GameObject>();
	public List<GameObject> grave = new List<GameObject>();

	public int turnCount = 1;

	void Start()
	{
		GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
		for(int i=0; i<players.Length; i++)
		{
			for(int j=0; j<players.Length; j++)
			{
				if(i==j)
				{
					continue;
				}
				if(players[i].GetComponent<Resources>().playerID == players[j].GetComponent<Resources>().playerID)
				{
					//Debug.LogError("To many players with same ID");
				}
			}
		}
	}

	public void addEssence(int toAdd)
	{
		essence+=toAdd;
		if(essence > maxEssence)
		{
			essence = maxEssence;
		}
	}

	public void loseEssence(int toLose)
	{
		essence-=toLose;
		if(essence<0)
		{
			essence=0;
		}
	}
}
