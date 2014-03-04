using UnityEngine;
using System.Collections;

public class Mover2 : MonoBehaviour {

	public float defspeed = 50;
	public float destX = 10;
	public float destY = 100;
	
	public Rigidbody r;
	public PathfindHelper p;
	Astar[] next;
	
	// Use this for initialization
	void Start () {
		next = new Astar[2];
	}
	
	// Update is called once per frame
	void Update () {

		r.AddForce (new Vector3 (0, 100));
	}
	
	/// <summary>
	/// Moves to a point.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	void MoveTo(float x, float y)
	{
		if(!p.hasHead ())
		{
			p.Pathfind ((int)x, (int)y);
			next[0] = p.head;
			next[1] = p.popNext();
		}
		Vector3 next0V = new Vector3 ((float)(next [0].xcoord), (float)(next [0].ycoord));
		Vector3 next1V = new Vector3 ((float)(next [1].xcoord), (float)(next [1].ycoord));
		if(Vector3.Distance(transform.position,next1V) <
		        Vector3.Distance(transform.position,next0V))
		{
			next[0] = next[1];
			next[1] = p.popNext();
			next0V = next1V;
			next1V = new Vector3 ((float)(next [1].xcoord), (float)(next [1].ycoord));
		}
		r.AddForce ((next0V - transform.position).Normalize () * 10);
		r.AddForce ((next1V - transform.position).Normalize () * 5);
	}
}
