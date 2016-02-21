using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public Candy candy;
	public int columnNum=10;
	public int rowNum=7;
	// Use this for initialization
	void Start () {
	
		for (int rowIndex=0; rowIndex<rowNum; rowIndex++) {
			for (int columnIndex =0; columnIndex<columnNum; columnIndex++) {
				Candy c=Instantiate(candy) as Candy;	
				c.transform.parent=this.transform;
				c.columnIndex=columnIndex;
				c.rowIndex=rowIndex;
				c.UpdatePosition();
			}
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
