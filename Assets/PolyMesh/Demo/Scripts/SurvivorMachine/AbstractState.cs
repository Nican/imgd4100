﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public abstract class AbstractState {
	
	public AbstractState Parent;
	
	public survivorAI survivorAI;
	
	public abstract AbstractState Update ();
}


public class Patrol : AbstractState {
	
	
	
	public override AbstractState Update ()
	{
		List<survivorAI> characters = FindsurvivorAIsInSight ();
		
		if (characters.Count == 0)
			return this; //We have nothing to do, our next state is out current states
		
		//We have to deal with the character that is getting close to our base
		
		float random = Random.value;
		if (random >= 0f && random <= 0.6f) {
			TalkState talk = new TalkState();
			talk.Parent = this;
			talk.survivorAI = this.survivorAI;
			talk.Enemy = characters.First ();
			return talk;
			
		} else if(random >= 0.6f && random <= 0.8f)
		{
			FightState fight = new FightState();
			fight.Parent = this;
			fight.survivorAI = this.survivorAI;
			fight.Enemy = characters.First();
			return fight;
		} else 
		{
			RunState run = new RunState();
			run.Parent = this;
			run.survivorAI = this.survivorAI;
			run.Enemy = characters.First();
			return run;
		}
	}
	
	public List<survivorAI> FindsurvivorAIsInSight() {
		List<survivorAI> characters = new List<survivorAI> ();//FindObjectsOfType (survivorAI.GetType ()) as GameObject[];
		foreach (GameObject a in GameObject.FindGameObjectsWithTag("survivor")){
			survivorAI tempS = a.GetComponent<survivorAI>();
			if(!tempS.inCamp)
				characters.Add (tempS);
		}
		
		List<survivorAI> returnList = new List<survivorAI> ();
		
		foreach(survivorAI c in characters)
		{
			if(Vector3.Distance(this.survivorAI.transform.position,c.transform.position) <= 50)
				returnList.Add(c);
			
		}
		return returnList;
	}
	
}


public class Night : AbstractState {
	
	
	
	public override AbstractState Update ()
	{
		List<zombie> zombies = FindzombiesInSight ();
		
		if (zombies.Count == 0)
			return this; //We have nothing to do, our next state is out current states
		
		//We have to deal with the character that is getting close to our base
		
		FightState fight = new FightState();
		fight.Parent = this;
		fight.survivorAI = this.survivorAI;
		fight.azombie = zombies.First();
		return fight;
	}
	
	public List<zombie> FindzombiesInSight() {
		List<zombie> characters = new List<zombie> ();//FindObjectsOfType (survivorAI.GetType ()) as GameObject[];
		foreach (GameObject a in GameObject.FindGameObjectsWithTag("zombie"))
			characters.Add ((zombie)a.GetComponent (typeof(zombie)));
		
		List<zombie> returnList = new List<zombie> ();
		
		foreach(zombie c in characters)
		{
			if(Vector3.Distance(this.survivorAI.transform.position,c.transform.position) <= 20)
				returnList.Add(c);
			
		}
		Debug.Log("zombie number = " + characters.Count);
		return returnList;
	}
	
}

public class SearchAI : AbstractState {
	
	
	
	public override AbstractState Update ()
	{
		if(!this.survivorAI.isCarrying && !this.survivorAI.isSearching)
			return doSearch();
		else return checkSight();
	}
	public AbstractState checkSight(){
		List<survivorAI> characters = FindsurvivorAIsInSight ();
		if (characters.Count == 0){
			Debug.Log("do Search running!");
			RunState run = new RunState();
			run.Parent = this;
			run.survivorAI = this.survivorAI;
			return run; //We have nothing to do, our next state is out current states
		}else{
			
			//We have to deal with the character that is getting close to the searcher
			
			float random = Random.value;
			if (random >= 0f && random <= 0.6f) {
				TalkState talk = new TalkState();
				talk.Parent = this;
				talk.survivorAI = this.survivorAI;
				talk.Enemy = characters.First ();
				return talk;
				
			} else if(random >= 0.6f && random <= 0.8f)
			{
				FightState fight = new FightState();
				fight.Parent = this;
				fight.survivorAI = this.survivorAI;
				fight.Enemy = characters.First();
				return fight;
			} else 
			{
				RunState run = new RunState();
				run.Parent = this;
				run.survivorAI = this.survivorAI;
				run.Enemy = characters.First();
				return run;
			}
		}
		
	}
	
	public AbstractState doSearch()
	{
		//MapGeneration3 map = Camera.main.GetComponent<MapGeneration3> ();
		GameObject[] caches = GameObject.FindGameObjectsWithTag("Collectable");
		
		GameObject cache = caches [Random.Range (0, caches.Count ()-1)];
		
		var dest = MapGeneration3.convertRealToGrid(cache.transform.position.x,cache.transform.position.y);
		
		this.survivorAI.doSearch(dest.x, dest.y);
		return this;
		
	}
	
	public List<survivorAI> FindsurvivorAIsInSight() {
		List<survivorAI> characters = new List<survivorAI> ();//FindObjectsOfType (survivorAI.GetType ()) as GameObject[];
		foreach (GameObject a in GameObject.FindGameObjectsWithTag("survivor")){
			survivorAI tempS = a.GetComponent<survivorAI>();
			if(!tempS.inCamp)
				characters.Add (tempS);
		}
		
		List<survivorAI> returnList = new List<survivorAI> ();
		
		foreach(survivorAI c in characters)
		{
			if(Vector3.Distance(this.survivorAI.transform.position,c.transform.position) <= 20)
				returnList.Add(c);
			
		}
		return returnList;
	}
	
}
public class TalkState : AbstractState {
	public survivorAI Enemy;
	public survivorAI survivorAI;
	
	public override AbstractState Update ()
	{
		
		float random = Random.value;
		if (random >= 0f && random <= 0.5f) 
		{
			float random1 = Random.value;
			if (random1 >= 0f && random1 <= 0.8f) 
			{
				Enemy.inCamp = true;
				Enemy.doRun ();
				survivorAI.doRun();
			} else if(random1 > 0.8f && random1 <= 1.0f)
			{
				Enemy.doRun();
				survivorAI.doRun ();
			}
		}
		return this.Parent;
		
	}
}


public class RunState : AbstractState {
	public survivorAI Enemy;
	public survivorAI survivorAI;
	
	public override AbstractState Update ()
	{
		Debug.Log("do state running!");
		/*if(survivorAI.isCarrying)
		{
			survivorAI.doDrop();
		}*/
		if(Enemy != null) Enemy.doRun();
		survivorAI.doRun ();
		return new StandByState();
		
	}
}

public class FightState : AbstractState {
	
	public survivorAI Enemy;
	public zombie azombie;
	public survivorAI survivorAI;
	
	public override AbstractState Update ()
	{
		bool keepFight = false;
		if (Enemy == null && azombie != null)
			keepFight = true;
		else if(Enemy != null && azombie == null)
			keepFight = true;
		else
			Debug.Log("Both zombie and survivor are null");
		
		if (keepFight) {
			//Keep fighting
			float random = Random.value;
			if(this.survivorAI.ammo <= 0)
			{
				Debug.Log("doing run");
				this.survivorAI.doHide();
				float random1 = Random.value;
				if(random < survivorAI.skill.defence && random >= 0f)
					this.survivorAI.doRun();
			} else {
				Debug.Log("doing shooting");
				this.survivorAI.doShoot(Enemy, azombie);
			}
			return this;
		}
		
		return this.Parent;
		
	}
}

public class StandByState: AbstractState{
	public override AbstractState Update ()
	{
		return this;
	}
}

