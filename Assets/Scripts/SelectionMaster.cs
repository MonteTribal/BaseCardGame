using UnityEngine;
using System.Collections;

public class SelectionMaster : MonoBehaviour {

	private GameObject selected;
	

	public void setSelected(GameObject target)
	{
		selected = target;
	}

	public GameObject getSelected()
	{
		return selected;
	}
}
