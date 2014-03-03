using UnityEngine;
using System.Collections;

public class survivorAI : survivor {
	public bool inCamp;
	public bool isCarrying = false;
	public bool isSearching, isPatrol;

	public AbstractState State = new Patrol();

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
		GameObject z = GameObject.FindGameObjectWithTag("zombie");
		var angle = Vector3.Angle(z.transform.position-transform.position,transform.forward);
		if(angle>=5f||angle<=-5f)
			this.gameObject.transform.Rotate (new Vector3(0,0,angle));
		else
			transform.Rotate(new Vector3(0,0,0));

	}

	public void doSearch() 
	{
		isSearching = true;
	}

	public void doShoot(survivorAI enemy, zombie zombie) 
	{

		float random = Random.value;
		if(random < skill.defence && random >= 0f)
		{
			if(SwitchToNight())
			{
				var angle = Vector3.Angle(zombie.transform.position-transform.position,transform.forward);
				this.gameObject.transform.Rotate (new Vector3(0,0,angle));
				zombie.isDead = true;
				zombie.decayTime = Time.time;
				zombie.gameObject.tag = "zombie";
			}
			else
			{
				enemy.isDead = true;
				enemy.decayTime = Time.time;
				enemy.gameObject.tag = "survivor";
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
