using UnityEngine;
using System.Collections;

public class PathfindHelper : MonoBehaviour {

	public Astar head;
	public Mover2 m;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Pathfind(int x, int y)
	{
		bool[][] testedGrid = new bool[MapGeneration3.sizeX][];
		for(int i = 0; i < MapGeneration3.sizeX; i++)
		{
			testedGrid[i] = new bool[MapGeneration3.sizeY];
		}
		head = (Astar)ScriptableObject.CreateInstance("Astar");
		head.Astarinit(null, (int)transform.position.x, (int)transform.position.y, testedGrid, x, y, MapGeneration3.occupiedGrid);
		//head = new Astar (null, (int)transform.position.x, (int)transform.position.y, testedGrid, x, y, MapGeneration3.occupiedGrid);
		int patience;
		for(patience = 1000; !(head.isFinal ()) && patience > 0; patience--)
		{
			head.extend();
		}
		//print ("Found at patience = " + patience);
		if(patience == 0) m.found = true;
		testedGrid = null;
	}

	public Astar popNext()
	{
		head = head.getNext ();
		return head;
	}

	public bool hasHead()
	{
		return head != null;
	}
}
