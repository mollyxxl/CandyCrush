using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public Candy candy;
	public int columnNum=10;
	public int rowNum=7;
	private ArrayList candyArr;
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
		Remove (c);return;
		if (crt == null) 
		{
			crt = c;
			return;
		} 
		else 
		{
			Exchange(crt,c);
			crt=null;
		}
	}
	private void Exchange(Candy c1,Candy c2)
	{
		int rowIndex = c1.rowIndex;
		c1.rowIndex = c2.rowIndex;
		c2.rowIndex = rowIndex;

		int columnIndex = c1.columnIndex;
		c1.columnIndex = c2.columnIndex;
		c2.columnIndex = columnIndex;

		c1.UpdatePosition ();
		c2.UpdatePosition ();
	}
	private void Remove(Candy c)
	{
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

}
