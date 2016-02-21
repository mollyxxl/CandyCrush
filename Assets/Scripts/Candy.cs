using UnityEngine;
using System.Collections;

public class Candy : MonoBehaviour {

	public int rowIndex;
	public int columnIndex;
	public float xOffset=-3f;
	public float yOffset=-2.5f;

	public GameObject[] bgs;
	private GameObject bg;

	public int type;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	private void AddRandonBg()
	{
		if (bg != null) return;

	    type = Random.Range (0, bgs.Length);
		bg=	Instantiate (bgs [type]) as GameObject;
		bg.transform.parent = this.transform;
	}
	public void UpdatePosition()
	{
		AddRandonBg ();
		transform.position = new Vector3 (columnIndex+xOffset, rowIndex+yOffset, 0f);
	}
}
