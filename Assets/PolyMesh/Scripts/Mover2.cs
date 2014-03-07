using UnityEngine;
using System.Collections;

public class Mover2 : MonoBehaviour {

	//public float defspeed = 50;
	public float destX = 15;
	public float destY = 15;
	
	public Rigidbody r;
	public PathfindHelper p;
	Astar[] next;

	public bool found = true;
	public bool start = true;
	
	// Use this for initialization
	void Start () {
		next = new Astar[3];
	}
	
	// Update is called once per frame
	void Update () {

		var character = GetComponent<Character> ();

		if (character == null)
			return;

		if (character.isDead)
			return;

		if(!found) 
			MoveTo(destX,destY);
	}

	public float GetMoveForce(){
		var character = GetComponent<Character> ();

		if (character != null)
			return character.MoveSpeed ();

		return 50.0f;


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
			r.drag = 10;
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
		if(Vector3.Distance(transform.position,MapGeneration3.convertGridToReal((int)x,(int)y)) < 0.02)
		{
			found = true;
			start = true;
			next[0].cleanup();
			next[0] = null;
			next[1] = null;
			p.head = null;
			r.drag = 100;
			return;
		}
		if(Vector3.Distance(transform.position,next1V) <
		        (Vector3.Distance(transform.position,next0V)))
		{
			//print ("Next!\n");
			next[0] = next[1];
			next[1] = next[2];
			next[2] = p.popNext();
			/*if(next[0] == next[1])
			{
				found = true;
				start = true;
				next[0].cleanup();
				next[0] = null;
				next[1] = null;
				p.head = null;
				return;
			}*/
			next0V = next1V;
			next1V = new Vector3 ((float)(next [1].xcoord), (float)(next [1].ycoord));
		}

		Vector3 normnext1v = next1V - transform.position;
		Vector3 normnext2v = next2V - transform.position;
		float moveSpeed = GetMoveForce ();
		normnext1v.Normalize();
		normnext2v.Normalize();
		//r.AddForce (normnext0v * 50); //DO NOT UNCOMMENT THIS, IT THROWS OFF THE PATHING. The path is smooth looking enough anyway.
		r.AddForce (normnext1v * moveSpeed);
		r.AddForce (normnext2v * moveSpeed/2);
	}
}
