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
			tested[xcoord][ycoord] = false;
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
	public Astar(Astar p, int x, int y, bool[][] b, int tx, int ty, bool[][] blocked){
		if (blocked [x] [y])
		{
			isdead = true;
		}
		parent = p;
		xcoord = x;
		ycoord = y;
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
		next = n;
		isrealpath = true;
		tested [xcoord] [ycoord] = false;
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
			children = new Astar[9];
			for(int xmod = -1; xmod <= 1; xmod++)
			{
				for(int ymod = -1; ymod <= 1; ymod++)
				{
					if(!tested[xcoord + xmod] [ycoord + ymod])
					{
						children[(xmod + 1)*3 + ymod + 1] = new Astar(this,xcoord + xmod, ycoord + ymod,tested,targetx,targety,blocks);
					}
				}
			}
		}
		else
		{
			for(int ind = 0; ind < 9; ind++)
			{
				if(children[ind] != null) children[ind].extend();
			}
		}
	}
}
