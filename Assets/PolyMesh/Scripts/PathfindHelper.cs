using UnityEngine;
using System.Collections;

public class PathfindHelper : MonoBehaviour {

	public Astar head;
	public int mapx, mapy;

	// Use this for initialization
	void Start () {
		mapx = (int)(Camera.main.orthographicSize * 2.0f) ;
		mapy = (int)(mapx * Screen.width / Screen.height);

		mapx -= 2;
		mapy -= 2;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Pathfind(int x, int y)
	{
		bool[][] testedGrid = new bool[mapx][];
		for(int i = 0; i < mapx; i++)
		{
			testedGrid[i] = new bool[mapy];
		}
		head = new Astar (null, (int)transform.position.x, (int)transform.position.y, testedGrid, x, y, MapGeneration3.occupiedGrid);
		for(int patience = 10000; !(head.isFinal ()) && patience > 0; patience--)
		{
			head.extend();
		}
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
