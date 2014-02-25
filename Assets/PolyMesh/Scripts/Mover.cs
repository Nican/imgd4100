using UnityEngine;
using System.Collections;


public class Mover : MonoBehaviour {
	
	public float defspeed = 50;
	public float destX = 10;
	public float destY = 10;

	public Rigidbody r;
	public Pathfind p;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(p == null)
		{
			p = (Pathfind)ScriptableObject.CreateInstance<Pathfind>();
			p.xEnd = transform.position.x;
			p.yEnd = transform.position.y;
			print(p.GenNext (destX, destY));
			p = p.next;
		}
		else
		{
			MoveTo (p.xEnd,p.yEnd);
			if(((Mathf.Pow(p.xEnd - transform.position.x,2)) <= 1) && ((Mathf.Pow(p.yEnd - transform.position.y,2)) <= 1))
			{
				p = p.next;
			}
		}
	}

	/// <summary>
	/// Moves to a point.
	/// </summary>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
	void MoveTo(float x, float y, float speed)
	{
		float xForce, yForce;
		xForce = x - transform.position.x;
		yForce = y - transform.position.y;

		//ForceT is a force that will point from the object being moved to (x,y) with magnitude speed
		Vector3 forceT;
		forceT = new Vector3 (xForce, yForce);
		forceT.Normalize ();
		forceT = forceT * speed;

		r.AddForce (forceT);
	}

	void MoveTo(float x, float y)
	{
		MoveTo (x, y, defspeed);
	}


}
