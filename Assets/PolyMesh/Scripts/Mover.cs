using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour {

	public Rigidbody r;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		r.AddForce(0,-10,0);
	}
}
