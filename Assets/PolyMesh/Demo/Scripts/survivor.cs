using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class survivor : Character {
	public string name = "asd";
	public List<float> trust; // trust level
	public List<string> names; //other survivors' names
	public int ammo; // bullets left
	public float hunger; // level of hunger for food
	public skills skill = new skills();



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public void setSkills(float defense, float search)
	{
		skill.defence = defense;
		skill.searchFood = search;
	}

	public void addNewSurvivor(string aName, float new_trust)
	{
		names.Add (aName);
		trust.Add (new_trust);
	}

	public void removeSurvivor(string aName)
	{
		int index = -1;
		for(int i=0;i<names.Count;i++){
			if(aName == names[i]){
				index = i+1;
				break;
			}
			if(i == (names.Count-1))
				return;
		}
		names.RemoveAt (index);
		trust.RemoveAt (index);
	}

}
