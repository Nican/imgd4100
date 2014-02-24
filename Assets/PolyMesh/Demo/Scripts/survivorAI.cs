using UnityEngine;
using System.Collections;

public class survivorAI : survivor {
	public bool inCamp;
	bool isSearching = false;

	AbstractState State = new Patrol();

	// Use this for initialization
	void Start () {
		State.survivorAI = this;
	
	}

	public void SwitchToNight(){

	}
	
	// Update is called once per frame
	void Update () {
		this.State = State.Update ();
	}

	public void doSearch() {
		isSearching = true;
	}

	public void doShoot() {
	}

	public void doRun() {}
}
