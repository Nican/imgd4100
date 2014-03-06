using UnityEngine;
using System.Collections;

public class survivorAI : survivor {
	public bool inCamp;
	public bool isCarrying = false;
	public bool isSearching, isPatrol;
	public float angle;
	public AbstractState State = new Patrol();
	GameObject[] effect = new GameObject[6];
	public float time = 0f;
	bool bEffect = false;
	public GameObject zombie1;

	public GameObject shot;

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
		if(Time.time-time>=0.2f&&time!=0&&time!=-1){
			for(int i =0; i<5;i++)
				Destroy(effect[i]);
			time = -1;
			bEffect = false;

		}
		if(bEffect){
			Vector3[] eP = new Vector3[5];
			for(int i =0; i<5;i++)
				eP[i] = effect[i].transform.position;
			effect[0].transform.position = Vector3.MoveTowards(effect[0].transform.position, new Vector3(eP[0].x+1f,eP[0].y,0), 2.5f * Time.deltaTime);
			effect[1].transform.position = Vector3.MoveTowards(effect[1].transform.position, new Vector3(eP[1].x-1f,eP[1].y,0), 3f * Time.deltaTime);
			effect[2].transform.position = Vector3.MoveTowards(effect[2].transform.position, new Vector3(eP[2].x,eP[2].y+1f,0), 4f * Time.deltaTime);
			effect[3].transform.position = Vector3.MoveTowards(effect[3].transform.position, new Vector3(eP[3].x,eP[3].y-1f,0), 5f * Time.deltaTime);
			effect[4].transform.position = Vector3.MoveTowards(effect[4].transform.position, new Vector3(eP[4].x+1f,eP[4].y+1f,0), 0.5f * Time.deltaTime);
		}

	}

	public void OnCollisionEnter(Collision collision)
	{
		//print ("WTF?");
		if(collision.gameObject.CompareTag ("Collectable"))
		{
			Destroy(collision.gameObject);
		}
		else if(collision.gameObject.CompareTag ("subcollectable"))
		{
			Destroy(collision.gameObject.transform.parent.gameObject);
		}
	}

	public void doSearch() 
	{
		isSearching = true;
	}

	public void doShoot(survivorAI enemy, zombie zombie) 
	{

		float random = Random.Range (0.0f, 100.0f);
		if(random < skill.defence)
		{
			if(SwitchToNight())
			{
				angle = GetAngle (transform.position.x, transform.position.y, zombie.transform.position.x, zombie.transform.position.y);
				transform.localEulerAngles = new Vector3 (0, 0, transform.rotation.z + angle);
				zombie.isDead = true;
				zombie.decayTime = Time.time;
				zombie.gameObject.tag = "Untagged";
				zombie.gameObject.transform.renderer.material.color = Color.gray;
			}
			else
			{
				angle = GetAngle (transform.position.x, transform.position.y, enemy.transform.position.x, enemy.transform.position.y);
				transform.localEulerAngles = new Vector3 (0, 0, transform.rotation.z + angle);
				enemy.isDead = true;
				enemy.decayTime = Time.time;
				enemy.gameObject.tag = "Untagged";
				enemy.gameObject.transform.renderer.material.color = Color.gray;
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

	void doBulletEffect(Transform enemy)
	{
		print ("start");
		time = Time.time + 1f;

		effect[5] = GameObject.FindGameObjectWithTag("effect");
		for(int i = 0; i< 5; i++)
		{
			Transform gun = transform.FindChild("Gun");
			effect[i] = Instantiate(shot, gun.position, Quaternion.identity) as GameObject;
			//effect[i].transform.Translate((enemy.position-transform.position)*0.1f);
		}
		bEffect = true;
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
	
}
