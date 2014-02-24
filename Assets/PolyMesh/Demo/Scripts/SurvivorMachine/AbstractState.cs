using UnityEngine;
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
		foreach (GameObject a in GameObject.FindGameObjectsWithTag("survivor"))
			characters.Add ((survivorAI)a.GetComponent (typeof(survivorAI)));

		List<survivorAI> returnList = new List<survivorAI> ();

		foreach(survivorAI c in characters)
		{
			if(Vector3.Distance(this.survivorAI.transform.position,c.transform.position) <= 50)
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
		
		if (!Enemy.isDead ) {
			//Keep fighting
			survivorAI temp_survivorAI = (survivorAI) this.survivorAI;
			temp_survivorAI.doShoot();
			return this;
		}
		
		return this.Parent;
		
	}
}


public class RunState : AbstractState {
	public survivorAI Enemy;
	public survivorAI survivorAI;
	
	public override AbstractState Update ()
	{
		survivorAI.doRun ();
		
		return this.Parent;
		
	}
}

public class FightState : AbstractState {

	public survivorAI Enemy;
	public survivorAI survivorAI;

	public override AbstractState Update ()
	{

		if (!Enemy.isDead) {
			//Keep fighting
			survivorAI.doShoot();
			return this;
		}

		return this.Parent;

	}
}