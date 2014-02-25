using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	public bool isDead;
	public float decayTime = 0f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void Update () {
		if (isDead)
		{
			if(decayTime != 0f && (Time.time - decayTime) >= 2)
				Destroy(gameObject);
		}
	}

	void move(){
	}
}
