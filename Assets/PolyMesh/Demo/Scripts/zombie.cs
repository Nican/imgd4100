using UnityEngine;
using System.Collections;

public class zombie : Character {

	// Use this for initialization
	void Start () {
		Mover2 m = GetComponent<Mover2> ();
		m.destX = MapGeneration3.sizeX / 2;
		m.destY = MapGeneration3.sizeY / 2;
		m.found = false;
		m.start = true;
	}

	public override float MoveSpeed ()
	{
		return 10.0f;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
	
	}

	void bite()
	{

	}
}
