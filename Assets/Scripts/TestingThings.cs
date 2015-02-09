using UnityEngine;
using System.Collections;

public class TestingThings : MonoBehaviour {

	public GameObject player;
	public GameObject opponent;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.A))
		{
			player.GetComponent<Dealer>().drawCard();
    	}
		if(Input.GetKeyDown(KeyCode.S))
		{
			opponent.GetComponent<Dealer>().drawCard();
    	}
  	}
}
