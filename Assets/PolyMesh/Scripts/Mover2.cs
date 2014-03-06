using UnityEngine;
using System.Collections;

public class Mover2 : MonoBehaviour {

	public float defspeed = 50;
	public float destX = 15;
	public float destY = 15;
	
	public Rigidbody r;
	public PathfindHelper p;
	Astar[] next;

	public bool found = true;
	public bool start = true;

	int MOVEFORCE = 50;
	
	// Use this for initialization
	void Start () {
		next = new Astar[3];
	}
	
	// Update is called once per frame
	void Update () {

		if(!found) MoveTo(destX,destY);
	}
	
	/// <summary>
	/// Moves to a point.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	void MoveTo(float x, float y)
	{
		if(start)
		{
			p.Pathfind ((int)x, (int)y);
			next[0] = p.head;
			next[1] = p.popNext();
			next[2] = p.popNext();
			start = false;
		}
		/*if(next[1] == null)
		{
			return;
		}*/
		Vector3 next0V = new Vector3 ((float)(next [0].xcoord), (float)(next [0].ycoord));
		Vector3 next1V = new Vector3 ((float)(next [1].xcoord), (float)(next [1].ycoord));
		Vector3 next2V = new Vector3 ((float)(next [2].xcoord), (float)(next [2].ycoord));
		if(Vector3.Distance(transform.position,next1V) <
		        (Vector3.Distance(transform.position,next0V)))
		{
			//print ("Next!\n");
			next[0] = next[1];
			next[1] = next[2];
			next[2] = p.popNext();
			if(next[0] == next[1])
			{
				found = true;
				start = true;
				next[0].cleanup();
				next[0] = null;
				next[1] = null;
				p.head = null;
				return;
			}
			next0V = next1V;
			next1V = new Vector3 ((float)(next [1].xcoord), (float)(next [1].ycoord));
		}
		Vector3 normnext1v = next1V - transform.position;
		Vector3 normnext2v = next2V - transform.position;
		normnext1v.Normalize();
		normnext2v.Normalize();
		//r.AddForce (normnext0v * 50); //DO NOT UNCOMMENT THIS, IT THROWS OFF THE PATHING. The path is smooth looking enough anyway.
		r.AddForce (normnext1v * MOVEFORCE);
		r.AddForce (normnext2v * MOVEFORCE/2);
	}
}
