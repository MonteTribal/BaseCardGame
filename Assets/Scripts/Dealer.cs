using UnityEngine;
using System.Collections;

public class Dealer : MonoBehaviour {
	
	public GameObject cardSpawnSpot;

	public GameObject handLeft;
	public GameObject handRight;

	public GameObject fieldLeft;
	public GameObject fieldRight;

	private Resources myStuff;

	private bool handHasChanged = false;
	private bool fieldHasChanged = false;

	// Use this for initialization
	void Awake()
	{
		myStuff = GetComponent<Resources>();
		if(cardSpawnSpot == null)
		{
			cardSpawnSpot = transform.FindChild("CardSpawner").gameObject;
		}
		if(handLeft == null)
		{
			handLeft = transform.FindChild("HandLeft").gameObject;
		}
		if(handRight == null)
		{
			handRight = transform.FindChild("HandRight").gameObject;
		}
		if(fieldLeft == null)
		{
			fieldLeft = transform.FindChild("FieldLeft").gameObject;
		}
		if(fieldRight == null)
		{
			fieldRight = transform.FindChild("FieldRight").gameObject;
		}
	}

	void Start () 
	{


	}
	
	// Update is called once per frame
	void Update () 
	{
		if(handHasChanged)
		{
			organizeHandCards();
		}	
		if(fieldHasChanged)
		{
			organizeFieldCards();
		}
	}

	void organizeFieldCards()
	{
		Vector3 left = fieldLeft.transform.position;
		Vector3 right = fieldRight.transform.position;
		float fieldLength = Vector3.Distance(left, right);
		int numCards = myStuff.field.Count;
		int numGaps = numCards;
		float distFromCardToCard = fieldLength/numGaps;
		
		for(int i=0; i<myStuff.field.Count; i++)
		{
			Vector3 start = left;
			float x = (i+.5f)*distFromCardToCard;
			myStuff.field[i].GetComponent<Card>().moveTowardsPoint(start + new Vector3( x, 0, 0));
		}
		fieldHasChanged=false;
	}
	
	void organizeHandCards()
	{
		Vector3 left = handLeft.transform.position;
		Vector3 right = handRight.transform.position;
		float handLength = Vector3.Distance(left, right);
		int numCards = myStuff.hand.Count;
		int numGaps = numCards;
		float distFromCardToCard = handLength/numGaps;

		for(int i=0; i<myStuff.hand.Count; ++i)
		{
			Vector3 start = left;
			float x = (i+.5f)*distFromCardToCard;
			myStuff.hand[i].GetComponent<Card>().moveTowardsPoint(start + new Vector3( x, 0, -.1f));
		}
		handHasChanged=false;
	}

	public void drawCard(int numToDraw=1)
	{
		for(int i=0; i<numToDraw; i++)
		{
			GameObject card = Instantiate( myStuff.deck[0], cardSpawnSpot.transform.position, Quaternion.identity ) as GameObject;
			card.GetComponent<Card>().setOriginalPrefab(myStuff.deck[0]);
			myStuff.hand.Add( card );
			card.transform.parent = transform.FindChild("Hand");
			card.GetComponent<Card>().zone = Card.gameZones.Hand;
			myStuff.deck.RemoveAt(0);
			handHasChanged = true;
		}
	}

	public void moveToGraveFromField(GameObject card)
	{
		myStuff.field.Remove(card);
		GameObject pre = card.GetComponent<Card>().getOriginalPrefab();
		pre.GetComponent<Card>().zone = Card.gameZones.Grave;
		myStuff.grave.Add(pre);
		Destroy(card.gameObject);
		fieldHasChanged = true;
	}

	public void moveToGraveFromHand(GameObject card)
	{
		myStuff.hand.Remove(card);
		GameObject pre = card.GetComponent<Card>().getOriginalPrefab();
		pre.GetComponent<Card>().zone = Card.gameZones.Grave;
		myStuff.grave.Add(pre);
		Destroy(card.gameObject);
		handHasChanged = true;
	}

	public void moveToField(GameObject card)
	{
		myStuff.hand.Remove(card);
		myStuff.field.Add(card);
		card.transform.parent = transform.FindChild("Field");
		card.GetComponent<Card>().zone = Card.gameZones.Field;
		fieldHasChanged = true;
		handHasChanged = true;
	}

}
