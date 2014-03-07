using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class survivorAI : survivor {
	public bool inCamp;
	public bool isCarrying = false;
	public bool isSearching, isPatrol;
	public float angle;
	public AbstractState State;
	GameObject[] effect = new GameObject[6];
	public float time = 0f;
	bool bEffect = false;
	public GameObject zombie1;
	
	public GameObject shot;

	public survivorAI(){
		State = new StandByState (this);
	}
	
	// Use this for initialization
	void Start () {
		State.survivorAI = this;
		inCamp = true;
		skill.defence = 80f;
		ammo = 10;
		
	}
	
	public bool SwitchToNight()
	{
		return true;
	}
	
	// Update is called once per frame
	void Update () {
		base.Update ();
		this.State = State.Update ();
		/*if(time==0){
			//GameObject zombie1 = GameObject.FindGameObjectWithTag("zombie");
			zombie zombie = zombie1.GetComponent(typeof(zombie)) as zombie;
			print (zombie.decayTime);
			zombie1 = zombie.gameObject;
			angle = GetAngle (transform.position.x, transform.position.y, zombie1.transform.position.x, zombie1.transform.position.y);
			transform.localEulerAngles = new Vector3 (0, 0, transform.rotation.z + angle);
			zombie.isDead = true;
			zombie.setDT(Time.time+1);
			zombie.gameObject.tag = "Untagged";
			zombie.gameObject.transform.renderer.material.color = Color.gray;
			doBulletEffect(zombie1.transform);
		}*/
		if(Time.time-time>=1.2f&&time!=0&&time!=-1){
			for(int i =0; i<5;i++)
				Destroy(effect[i]);
			time = -1;
			bEffect = false;
			
		}
		if(bEffect){
			Vector3[] eP = new Vector3[5];
			for(int i =0; i<5;i++)
				eP[i] = effect[i].transform.position;
			effect[0].transform.position = Vector3.MoveTowards(effect[0].transform.position, new Vector3(eP[0].x+1f,eP[0].y,-5), 2.5f * Time.deltaTime);
			effect[1].transform.position = Vector3.MoveTowards(effect[1].transform.position, new Vector3(eP[1].x-1f,eP[1].y,-5), 3f * Time.deltaTime);
			effect[2].transform.position = Vector3.MoveTowards(effect[2].transform.position, new Vector3(eP[2].x,eP[2].y+1f,-5), 4f * Time.deltaTime);
			effect[3].transform.position = Vector3.MoveTowards(effect[3].transform.position, new Vector3(eP[3].x,eP[3].y-1f,-5), 5f * Time.deltaTime);
			effect[4].transform.position = Vector3.MoveTowards(effect[4].transform.position, new Vector3(eP[4].x+1f,eP[4].y+1f,-5), 0.5f * Time.deltaTime);
		}
		
	}
	
	public void OnCollisionEnter(Collision collision)
	{
		//print ("WTF?");
		if(collision.gameObject.CompareTag ("Collectable"))
		{
			Destroy(collision.gameObject);
			isCarrying = true;
		}
		else if(collision.gameObject.CompareTag ("subcollectable"))
		{
			Destroy(collision.gameObject.transform.parent.gameObject);
			isCarrying = true;
		}
	}
	
	public void doSearch(float x, float y) 
	{
		isSearching = true;
		Mover2 m = GetComponent<Mover2> ();
		m.destX = x;
		m.destY = y;
		m.found = false;
		m.start = true;
	}
	
	public void doShoot(survivorAI enemy, zombie zombie) 
	{
		Debug.Log("doing AI shooting");
		if(enemy != null)
			rotateToShoot(enemy.transform);
		else 
			rotateToShoot(zombie.transform);
		float random = Random.Range (0.0f, 100.0f);
		if(random < skill.defence)
		{
			if(SwitchToNight())
			{
				zombie.isDead = true;
				if(zombie.decayTime==0f) zombie.setDT(Time.time+1f);
				zombie.gameObject.tag = "Untagged";
				zombie.gameObject.transform.renderer.material.color = Color.gray;
				doBulletEffect(zombie.gameObject.transform);
			}
			else
			{
				enemy.isDead = true;
				enemy.setDT(Time.time+1f);
				enemy.gameObject.tag = "Untagged";
				enemy.gameObject.transform.renderer.material.color = Color.gray;
				doBulletEffect(enemy.gameObject.transform);
			}
		}
	}
	
	public void doRun()
	{
		Debug.Log("do running!");
		Mover2 m = GetComponent<Mover2> ();
		//var dest = MapGeneration3.convertRealToGrid(1f,-1f);
		m.destX = 20f;
		m.destY = 13f;
		m.found = false;
		m.start = true;
		isCarrying = false;
		isSearching = false;
		Debug.Log("finish set up running!");
	}
	
	
	public void doHide()
	{
		
	}
	
	public void doDrop()
	{
		isCarrying = false;
	}
	
	void doBulletEffect(Transform enemy)
	{
		if(!bEffect){
			print ("start");
			time = Time.time + 1f;

			for(int i = 0; i< 5; i++)
			{
				Transform gun = transform.FindChild("Gun");
				effect[i] = Instantiate(shot, new Vector3(gun.position.x,gun.position.y,-5f), Quaternion.identity) as GameObject;
				//effect[i].transform.Translate((enemy.position-transform.position)*0.1f);
			}
			bEffect = true;
		}
	}
	
	public float GetAngle(float X1, float Y1, float X2, float Y2) {
		
		
		// take care of special cases - if the angle
		
		// is along any axis, it will return NaN,
		
		// or Not A Number.  This is a Very Bad Thing(tm).
		
		if (Y2 == Y1) {
			
			return (X1 > X2) ? 180 : 0; 
			
		}
		
		if (X2 == X1) {
			
			return (Y2 > Y1) ? 90 : 270;
			
		}
		
		
		// convert from radians to degrees
		float ang =  Mathf.Atan2(Y2-Y1,X2-X1) * Mathf.Rad2Deg;
		//double ang = (float) Mathf.Atan(tangent) * Mathf.Rad2Deg;
		
		// the arctangent function is non-deterministic,
		
		// which means that there are two possible answers
		
		// for any given input.  We decide which one here.
		
		//if ((Y2-Y1 > 0 && X2-X1<0)||(Y2-Y1 < 0 && X2-X1<0)) ang += 180;
		
		
		
		// NOTE that this does NOT need to be normalised.  Arctangent
		
		// always returns an angle that is within the 0-360 range.
		
		
		// barf it back to the calling function
		
		return (float) ang;
		
		
		
	}
	
	void rotateToShoot(Transform enemy)
	{
		angle = GetAngle (transform.position.x, transform.position.y, enemy.position.x, enemy.position.y);
		transform.localEulerAngles = new Vector3 (0, 0, transform.rotation.z + angle);
	}
	
}
