using UnityEngine;
using System.Collections;

public class InitGame : MonoBehaviour {
	public GameObject rock;

	// Use this for initialization
	void Start () {

		var rock2 = Instantiate (rock, new Vector3 (0, 10), Quaternion.identity) as GameObject;
		var a = rock2.GetComponent<PolyMesh> ();

		a.keyPoints.Clear ();
		a.isCurve.Clear ();

		for (float r = 0.0f; r <= Mathf.PI * 2; r += 0.1f) {
			a.keyPoints.Add(new Vector3(Mathf.Cos(r), Mathf.Sin(r) * 2));
			a.isCurve.Add(false);
		}

		a.BuildMesh ();



	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
