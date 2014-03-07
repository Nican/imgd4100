using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	public bool isDead;
	public float decayTime;
	// Use this for initialization
	void Start () {
		decayTime = 0f;
	}
	
	// Update is called once per frame
	public void Update () {
		if (isDead)
		{
			if(decayTime != 0f && (Time.time - decayTime) >= 3f){
				Destroy(gameObject);
			}
		}
	}

	public virtual float MoveSpeed(){
		return 50.0f;
	}

	void move(){
	}
	public void setDT(float DT){
		decayTime = DT;
	}

}
