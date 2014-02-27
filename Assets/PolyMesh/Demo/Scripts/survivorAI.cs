using UnityEngine;
using System.Collections;

public class survivorAI : survivor {
	public bool inCamp;
	public bool isCarrying = false;

	AbstractState State = new Patrol();

	// Use this for initialization
	void Start () {
		State.survivorAI = this;
	
	}

	public bool SwitchToNight()
	{
		return true;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
		//this.State = State.Update ();
	}

	public void doSearch() 
	{

	}

	public void doShoot(survivorAI enemy, zombie zombie) 
	{

		float random = Random.value;
		if(random < skill.defence && random >= 0f)
		{
			if(SwitchToNight())
			{
				zombie.isDead = true;
				zombie.decayTime = Time.time;
				zombie.gameObject.tag = "Untagged";
			}
			else
			{
				enemy.isDead = true;
				enemy.decayTime = Time.time;
				enemy.gameObject.tag = "Untagged";
			}
		}
	}

	public void doRun()
	{

	}
	

	public void doHide()
	{

	}

	public void doDrop()
	{
		isCarrying = false;
	}
	
}
