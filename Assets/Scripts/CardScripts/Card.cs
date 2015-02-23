/*
 * BASE CLASS FOR ALL CARDS
 * 
 */

using UnityEngine;
using System.Collections;
 
public class Card : MonoBehaviour {

	public string myname = "AAA";

	//public string cost;

	public float slideSpeed = 50f;

	public enum CardTypes{Creature, Spell, FieldMarker};
	protected CardTypes cardType = CardTypes.Creature;

	private Vector3 target;
	private bool moveTo = false;

	private GameObject originalPrefab;

	private bool scaledUp = false;

	public int cost = 1;
	public GUISkin skin;

	protected GameObject gm;
	protected Resources myStuff;
	//[HideInInspector]
	public enum gameZones{Deck, Hand, Field, Grave};
	//[HideInInspector]
	public gameZones zone = gameZones.Deck;

	protected bool showGuiOptions = false; // so, i forgot that protected was a thing. so there might be some redundant code somewhere.
	public string popupText;

	// Use this for initialization
	protected virtual void Start () {
		//int a = Random.Range(-100, 100);
		//myname = a.ToString();

		gm = GameObject.Find("GameMaster");
		myStuff = transform.root.GetComponent<Resources>();
	}

	// Update is called once per frame
	protected virtual void Update () 
	{
		if(moveTo)
		{
			float step = slideSpeed * Time.deltaTime;
			Vector3 next = Vector3.MoveTowards(transform.position, target, step);
			transform.position = next;
			if(Vector3.Distance(target, transform.position) < .1f)
			{
				moveTo = false;
			}
		}

		if(transform.position.z > -.1f) //ground = 0, camera = -1
		{
			transform.position = new Vector3( transform.position.x, transform.position.y, -transform.position.z);
		}

	}

	protected virtual void OnMouseEnter()
	{
		if(!scaledUp)
		{
			transform.localScale = new Vector3(transform.localScale.x*2f, transform.localScale.y*2f, transform.localScale.z*2f);
			transform.Translate( new Vector3(0f, 0f, -.2f));
			scaledUp = true;
		}
	}

	protected virtual void OnMouseExit()
	{
		if(scaledUp)
		{
			transform.localScale = new Vector3(transform.localScale.x/2f, transform.localScale.y/2f, transform.localScale.z/2f);
			transform.Translate( new Vector3(0f, 0f, .2f));
			scaledUp = false;
		}
	}

	protected virtual void OnMouseDown()
	{
		//Debug.Log("Base Card click");
		gm.GetComponent<SelectionMaster>().setSelected(this.gameObject);
	}

	protected virtual void OnMouseUp()
	{
		//Debug.Log("Base Card Click-Up");this.game
	}

	protected virtual void cast()
	{
		//Debug.Log("Base Card cast");
	}


	public void moveTowardsPoint(Vector3 pos)
	{
		target = pos;
		moveTo = true;
	}

	public void setOriginalPrefab(GameObject orig)
	{
		originalPrefab = orig;
	}

	public GameObject getOriginalPrefab()
	{
		return originalPrefab;
	}

	public bool canCast()
	{
		if(transform.parent.root.GetComponent<Resources>().essence >= cost)
		{
			return true;
		}
		return false;
	}

	public void closePopups()
	{
		if(showGuiOptions)
		{
			showGuiOptions = false;
			gm.GetComponent<GUIMaster>().closeBox(this.gameObject);
		}
	}

}
