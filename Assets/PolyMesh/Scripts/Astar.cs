using UnityEngine;
using System.Collections;

public class Astar : ScriptableObject {

	Astar parent;
	Astar next;
	public int xcoord;
	public int ycoord;
	int targetx;
	int targety;
	bool[][] tested;
	bool[][] blocks;
	bool isrealpath = false;
	bool isdead = false;
	Astar[] children;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		//Update won't occur until the pathfind is done, so this is safe.
		if (!isrealpath)
		{
			tested[xcoord+MapGeneration3.sizeX/2][ycoord+MapGeneration3.sizeY/2] = false;
			Destroy (this);
		}
	}


	/// <summary>
	/// Initializes a new instance of the <see cref="Astar"/> class.
	/// </summary>
	/// <param name="p">The parent of the new Astar object</param>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	/// <param name="b">The grid of loations that have already been tested.</param>
	/// <param name="tx">The x coordinate of the target.</param>
	/// <param name="ty">The y coordinate of the target.</param>
	/// <param name="b">The grid of loations that are occupied by an obstacle.</param>
	public void Astarinit(Astar p, int x, int y, bool[][] b, int tx, int ty, bool[][] blocked){
		//if(p != null) MonoBehaviour.print("Not head\n");
		xcoord = x;
		ycoord = y;
		x = x+MapGeneration3.sizeX/2;
		y = y+MapGeneration3.sizeY/2;
		if(x < 0 || y < 0 || x > MapGeneration3.sizeX-1 || y > MapGeneration3.sizeY - 1)
		{
			MonoBehaviour.print("Invalid pathfind\n");
			return;
		}
		if (blocked [x] [y])
		{
			isdead = true;
		}
		parent = p;
		tested = b;
		targetx = tx;
		targety = ty;
		b [x] [y] = true;
		blocks = blocked;
		if(tx == x && ty == y)
		{
			finalizePath (this);
		}
	}

	/// <summary>
	/// Finalizes the path.
	/// </summary>
	/// <param name="n">A reference to the Astar object to be set as the next point in the path</param>
	void finalizePath(Astar n)
	{
		//MonoBehaviour.print("finalizing\n");
		next = n;
		isrealpath = true;
		tested [xcoord+MapGeneration3.sizeX/2] [ycoord+MapGeneration3.sizeY/2] = false;
		if(parent != null)
		{
			parent.finalizePath (this);
		}
	}

	public Astar getNext()
	{
		return next;
	}

	public bool isFinal()
	{
		return isrealpath;
	}

	/// <summary>
	/// Extends the search by another iteration.
	/// </summary>
	public void extend()
	{
		if(isdead) return;
		if(children == null)
		{
			children = new Astar[4];

			//Only checks a cross shape, but the way movement is handled means that the character should move on diagonals anyway if viable.
			//This is to avoid characters attempting to slip between buildings that touch at a corner, and to avoid zigzagging.
			for(int xmod = 0; xmod <= 1; xmod++)
			{
				if(xcoord + MapGeneration3.sizeX/2 + xmod*2 - 1 < MapGeneration3.sizeX &&
				   xcoord + MapGeneration3.sizeX/2 + xmod*2 - 1 >= 0 &&
				   ycoord + MapGeneration3.sizeY/2 < MapGeneration3.sizeY &&
				   ycoord + MapGeneration3.sizeY/2 >= 0 &&
				   !tested[xcoord + MapGeneration3.sizeX/2 + xmod*2 - 1] [ycoord + MapGeneration3.sizeY/2])
				{
					children[xmod] = (Astar)ScriptableObject.CreateInstance("Astar");
					children[xmod].Astarinit(this,xcoord + xmod*2 - 1, ycoord,tested,targetx,targety,blocks);
				}
			}
			for(int ymod = 0; ymod <= 1; ymod++)
			{
				if(xcoord + MapGeneration3.sizeX/2 < MapGeneration3.sizeX &&
				   xcoord + MapGeneration3.sizeX/2 >= 0 &&
				   ycoord + MapGeneration3.sizeY/2 + ymod*2 - 1 < MapGeneration3.sizeY &&
				   ycoord + MapGeneration3.sizeY/2 + ymod*2 - 1 >= 0 &&
				   !tested[xcoord + MapGeneration3.sizeX/2] [ycoord + MapGeneration3.sizeY/2 + ymod * 2 - 1])
				{
					children[2 + ymod] = (Astar)ScriptableObject.CreateInstance("Astar");
					children[2 + ymod].Astarinit(this,xcoord, ycoord + ymod*2 - 1,tested,targetx,targety,blocks);
				}
			}
		}
		else
		{
			for(int ind = 0; ind < 4; ind++)
			{
				if(children[ind] != null) children[ind].extend();
			}
		}
	}

	public void cleanup()
	{
		isrealpath = false;
		if(parent != null) parent.cleanup ();
	}
}
