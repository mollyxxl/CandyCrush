﻿using UnityEngine;
using System.Collections;

public class Candy : MonoBehaviour {

	public int rowIndex;
	public int columnIndex;
	public float xOffset=-3f;
	public float yOffset=-2.5f;

	public GameObject[] bgs;
	private GameObject bg;

	public int type;

	public GameController game;

	public int candyTypeNum=6;

	private SpriteRenderer sr;
	public bool Selected
	{
		set{
			if(sr!=null)
				sr.color=value?Color.yellow:Color.white;
		}
	}
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnMouseDown()
	{
		//Debug.Log("mouseDown");
		game.Select (this);
	}

	private void AddRandonBg()
	{
		if (bg != null) return;

	    type = Random.Range (0, Mathf.Min(candyTypeNum,bgs.Length));
		bg=	Instantiate (bgs [type]) as GameObject;
		bg.transform.parent = this.transform;

		sr = bg.GetComponent<SpriteRenderer> ();
	}
	public void UpdatePosition()
	{
		AddRandonBg ();
		transform.position = new Vector3 (columnIndex+xOffset, rowIndex+yOffset, 0f);
	}
	public void TweenToPosition()
	{
		AddRandonBg ();
		iTween.MoveTo (this.gameObject, iTween.Hash (
			"x",columnIndex+xOffset,
			"y",rowIndex+yOffset,
			"time",0.3f
			));
	}
	public void Dispose()
	{
		game = null;
		Destroy (bg.gameObject);
		Destroy (this.gameObject);
	}
}
