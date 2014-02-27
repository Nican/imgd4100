using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UI : MonoBehaviour {
	bool windowOn;
	Rect windowRect;
	public Vector2 scrollPosition = Vector2.zero;
	List<survivorAI> sList;

	// Use this for initialization
	void Start () {
		windowRect = new Rect(Screen.width/4, Screen.height/4, Screen.width/2, 2*Screen.height/3);
		for (int i = 0; i < 20; i++)
			Instantiate((Object)GameObject.FindGameObjectWithTag("survivor"));
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("s"))
			windowOn = windowOn? false: true;
			
	
	}
	void OnGUI(){
		if (windowOn){
			sList = new List<survivorAI>();
			int i = 0;
			foreach (GameObject a in GameObject.FindGameObjectsWithTag("survivor")){
				survivorAI b = a.GetComponent (typeof(survivorAI)) as survivorAI;
				b.name = "john" + i;
				i++;
				sList.Add (b);
			}
			windowRect = GUI.Window (0, windowRect, windowFunc, "Survivor");
		}
	}
	
	void windowFunc(int windowID){
		print (sList.Count);
		scrollPosition = GUI.BeginScrollView(new Rect(20, 40, Screen.width/2 -30, Screen.height/4 + 100), scrollPosition, new Rect(0, 0, 400, sList.Count*30));
		for (int i = 0; i < sList.Count; i++){
			GUI.Button(new Rect(0, i*30, 100, 20), sList[i].name);
			GUI.Label(new Rect(120, i*30, 100, 20), "Ammo: " + sList[i].ammo);
			GUI.Label(new Rect(240, i*30, 100, 20), "Hunger: " + sList[i].hunger);
		}
		GUI.EndScrollView();

		}
}
