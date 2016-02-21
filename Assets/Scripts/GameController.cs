using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public Candy candy;
	public int columnNum=10;
	public int rowNum=7;
	private ArrayList candyArr;

	public AudioClip swapClip;
	public AudioClip explodeClip;
	public AudioClip math3Clip;
	// Use this for initialization
	void Start () {
	
		candyArr = new ArrayList ();

		for (int rowIndex=0; rowIndex<rowNum; rowIndex++) {
			ArrayList tmp=new ArrayList();
			for (int columnIndex =0; columnIndex<columnNum; columnIndex++) {

				Candy c=AddCandy(rowIndex,columnIndex);
				tmp.Add(c);
			}
			candyArr.Add(tmp);
		}
		//first check
	  if (CheckMatches ())
		 RemoveMatches ();

	}
	private Candy AddCandy(int rowIndex,int columnIndex)
	{
		Candy c=Instantiate(candy) as Candy;	
		c.transform.parent=this.transform;
		c.columnIndex=columnIndex;
		c.rowIndex=rowIndex;
		c.UpdatePosition();
		c.game=this;
		return c;
	}

	private Candy GetCandy(int rowIndex,int columnIndex)
	{
		ArrayList tmp = candyArr [rowIndex] as ArrayList;
		Candy c = tmp [columnIndex] as Candy;
		return c;
	}
	private void SetCandy(int rowIndex,int columnIndex,Candy c)
	{
		ArrayList tmp = candyArr [rowIndex] as ArrayList;
		tmp [columnIndex] = c;
	}
	private Candy crt;
    public void Select(Candy c)
	{
		//Remove (c);return;
		if (crt == null) 
		{
			crt = c;
			crt.Selected=true;
			return;
		} 
		else 
		{
			if(Mathf.Abs(crt.rowIndex-c.rowIndex)+Mathf.Abs(crt.columnIndex-c.columnIndex)==1)
			{
				StartCoroutine(Exchange2(crt,c));
			}
			crt.Selected=false;
			crt=null;
		}
	}
	IEnumerator Exchange2(Candy c1, Candy c2)
	{
		Exchange(c1,c2);
		//wait for 0.4s
		yield return new WaitForSeconds (0.4f);

		if(CheckMatches())
		{
			RemoveMatches();
		}
		else
		{
			Exchange(c1,c2);
		}

	}
	private void Exchange(Candy c1,Candy c2)
	{
		//play audio
		audio.PlayOneShot (swapClip);

		SetCandy (c1.rowIndex, c1.columnIndex, c2);
		SetCandy (c2.rowIndex, c2.columnIndex, c1);

		int rowIndex = c1.rowIndex;
		c1.rowIndex = c2.rowIndex;
		c2.rowIndex = rowIndex;

		int columnIndex = c1.columnIndex;
		c1.columnIndex = c2.columnIndex;
		c2.columnIndex = columnIndex;

		//c1.UpdatePosition ();
		//c2.UpdatePosition ();
		c1.TweenToPosition ();
		c2.TweenToPosition ();
	}
	private void Remove(Candy c)
	{
		AddEffect (c.transform.position);

		audio.PlayOneShot (explodeClip);
		c.Dispose ();

		//move up candy down
		int columnIndex = c.columnIndex;
		for (int rowIndex=c.rowIndex+1; rowIndex<rowNum; rowIndex++) 
		{
			Candy c2=GetCandy(rowIndex,columnIndex)	;
			c2.rowIndex--;
			//c2.UpdatePosition();
			c2.TweenToPosition();
			SetCandy(rowIndex-1,columnIndex,c2);
		}

		//add new candy up
		Candy newC = AddCandy (rowNum - 1, columnIndex);
		newC.rowIndex = rowNum;
		newC.UpdatePosition ();
		newC.rowIndex--;
		newC.TweenToPosition ();
		SetCandy (rowNum - 1, columnIndex, newC);
	}
	private bool CheckMatches()
	{
		return CheckHorizontalMatches ()||CheckVerticalMatches ();
	}
	private bool CheckHorizontalMatches()
	{
		bool result = false;
		for (int rowIndex=0; rowIndex<rowNum; rowIndex++) 
		{
			for (int columnIndex=0; columnIndex<columnNum-2; columnIndex++) 
			{
				if((GetCandy(rowIndex,columnIndex).type==GetCandy(rowIndex,columnIndex+1).type)&&
				   (GetCandy(rowIndex,columnIndex+2).type==GetCandy(rowIndex,columnIndex+1).type))
				{
					audio.PlayOneShot(math3Clip);
					result=true;
					AddMatch(GetCandy(rowIndex,columnIndex));
					AddMatch(GetCandy(rowIndex,columnIndex+1));
					AddMatch(GetCandy(rowIndex,columnIndex+2));
				}
			}	
		}
		return result;
	}
	private bool CheckVerticalMatches()
	{
		bool result = false;
		for (int columnIndex=0; columnIndex<columnNum; columnIndex++) 
		{
			for (int rowIndex=0; rowIndex<rowNum-2; rowIndex++) 
			{
				if((GetCandy(rowIndex,columnIndex).type==GetCandy(rowIndex+1,columnIndex).type)&&
				   (GetCandy(rowIndex+2,columnIndex).type==GetCandy(rowIndex+1,columnIndex).type))
				{
					audio.PlayOneShot(math3Clip); 
					result=true;
					AddMatch(GetCandy(rowIndex,columnIndex));
					AddMatch(GetCandy(rowIndex+1,columnIndex));
					AddMatch(GetCandy(rowIndex+2,columnIndex));
				}
			}	
		}
		return result;
	}
	private ArrayList matches=new ArrayList();
	private void AddMatch(Candy c)
	{
		if (matches == null)
			matches = new ArrayList ();
		int index = matches.IndexOf (c);
		if (index == -1)
			matches.Add (c);
	}
	private void RemoveMatches()
	{
		Candy tmp;
		for (int i=0; i<matches.Count; i++) 
		{
			tmp=matches[i] as Candy;
			Remove(tmp);
		}
		matches = new ArrayList ();

		StartCoroutine (WaitAndCheck());
	}
  IEnumerator WaitAndCheck()
	{
		yield return new WaitForSeconds (0.5f);
		if (CheckMatches ())
			RemoveMatches ();

	}
	private void AddEffect(Vector3 pos)
	{
		Instantiate (Resources.Load ("Prefabs/Explosion2"), pos, Quaternion.identity);
		CameraShake.shakeFor (0.5f, 0.1f);
	}

}
