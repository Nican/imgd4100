using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class UI : MonoBehaviour {
	public bool listWindowOn, infoWindowOn;
	Rect windowRect;
	public Vector2 scrollPosition = Vector2.zero;
	List<survivorAI> sList;
	survivorAI theSurvivor;
	GUIStyle[] buttons = new GUIStyle[2];

	// Use this for initialization
	void Start () {
		windowRect = new Rect(Screen.width/4, Screen.height/4, Screen.width/2, 2*Screen.height/3);


	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown("s"))
			listWindowOn = listWindowOn? false: true;
		if(listWindowOn){
			if(Input.GetKeyDown("b")){
				print ("pressed");
				listWindowOn = listWindowOn? false: true;
				infoWindowOn = !listWindowOn;
			}
			if(Input.GetKeyDown("x")){
				infoWindowOn = false;
				listWindowOn = false;
			}
		}
		if(infoWindowOn){
			if(Input.GetKeyDown("b")){
				infoWindowOn = infoWindowOn? false: true;
				listWindowOn = !infoWindowOn;
			}
			if(Input.GetKeyDown("x")){
				infoWindowOn = false;
				listWindowOn = false;
			}
		}
	}
	void OnGUI(){
		if (listWindowOn){
			sList = new List<survivorAI>();
			int i = 0;
			foreach (GameObject a in GameObject.FindGameObjectsWithTag("survivor")){
				survivorAI b = a.GetComponent (typeof(survivorAI)) as survivorAI;
				b.name = "john" + i;
				i++;
				sList.Add (b);
			}
			windowRect = GUI.Window (0, windowRect, ListWindow, "Survivor");
		}

		if (infoWindowOn){
			windowRect = GUI.Window (0, windowRect, infoWindow, theSurvivor.name);
		}
	}
	
	void ListWindow(int windowID){
		scrollPosition = GUI.BeginScrollView(new Rect(20, 40, Screen.width/2 -30, Screen.height/4 + 150), scrollPosition, new Rect(0, 0, 400, sList.Count*30));
		GUI.Label(new Rect(120, 0, 50, 20), "Ammo");
		GUI.Label(new Rect(180, 0, 50, 20), "Hunger");
		GUI.Label(new Rect(240, 0, 50, 20), "Defense");
		GUI.Label(new Rect(300, 0, 50, 20), "Search");
		for (int i = 1; i <= sList.Count; i++){
			if(GUI.Button(new Rect(0, i*30, 100, 20), sList[i-1].name)){
				theSurvivor = sList[i-1];
				listWindowOn = false;
				infoWindowOn = true;
			}
			GUI.Label(new Rect(120, i*30, 50, 20), sList[i-1].ammo.ToString());
			GUI.Label(new Rect(180, i*30, 50, 20), sList[i-1].hunger.ToString());
			GUI.Label(new Rect(240, i*30, 50, 20), sList[i-1].skill.defence.ToString());
			GUI.Label(new Rect(300, i*30, 50, 20), sList[i-1].skill.searchFood.ToString());
			if(GUI.Button(new Rect(360, i*30, 100, 20), "doSearch")){
				sList[i-1].State = new SearchAI();
			}
			if(GUI.Button(new Rect(480, i*30, 100, 20), "doDefense")){
				sList[i-1].State = new Patrol();
			}
		}
		GUI.EndScrollView();
		if(GUI.Button(new Rect(Screen.width/2 -130, Screen.height/4 + 200, 100,50 ), "Exit"))
		{
			listWindowOn = false;
			infoWindowOn = false;
		}
		if(GUI.Button(new Rect(Screen.width/4 - 300, Screen.height/4 + 200, 100,50 ), "Back"))
		{
			listWindowOn = listWindowOn? false: true;
			infoWindowOn = !listWindowOn;
		}

	}
	void infoWindow(int windowID){
		scrollPosition = GUI.BeginScrollView(new Rect(20, 40, Screen.width/2 -30, Screen.height/4 + 150), scrollPosition, new Rect(0, 0, 400, sList.Count*30));
		GUI.Label(new Rect(120, 0, 100, 20), "Name");
		GUI.Label(new Rect(360, 0, 100, 20), "Trust Level");
		for (int i = 1; i <= theSurvivor.names.Count; i++){
			GUI.Label(new Rect(120, i*30, 100, 20), theSurvivor.names[i-1]);
			GUI.Label(new Rect(360, i*30, 100, 20), theSurvivor.trust[i-1].ToString());
		}
		GUI.EndScrollView();
		if(GUI.Button(new Rect(Screen.width/2 -130, Screen.height/4 + 200, 100,50 ), "Exit"))
		{
			listWindowOn = false;
			infoWindowOn = false;
		}
		if(GUI.Button(new Rect(Screen.width/4 - 300, Screen.height/4 + 200, 100,50 ), "Back"))
		{
			infoWindowOn = infoWindowOn? false: true;
			listWindowOn = !infoWindowOn;
		}
		
	}

	void test()
	{
		for (int i = 0; i < 20; i++)
			Instantiate((Object)GameObject.FindGameObjectWithTag("survivor"));
		List<survivorAI> new_sList = new List<survivorAI>();
		int j = 0;
		foreach (GameObject a in GameObject.FindGameObjectsWithTag("survivor")){
			survivorAI b = a.GetComponent (typeof(survivorAI)) as survivorAI;
			b.name = "john" + j;
			j++;
			new_sList.Add (b);
		}
		for (int i = 0; i < new_sList.Count; i++) 
		{
			//List<survivorAI> temp_sList = new_sList;
			foreach(survivorAI a in new_sList){
				if(a.name != new_sList[i].name){
					new_sList[i].addNewSurvivor(a.name, Random.value*(float)Random.Range(0,100));
					new_sList[i].setSkills(Random.value*(float)Random.Range(0,100),Random.value*(float)Random.Range(0,100));
				}
			}
		}
	}
}
