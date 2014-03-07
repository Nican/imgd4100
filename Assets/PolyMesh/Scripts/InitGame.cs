using UnityEngine;
using System.Linq;
using System.Collections;

public class InitGame : MonoBehaviour {

	int level = 0;
	float nextGame = 0;
	float nextZombie = 0;
	bool isDay = true;

	public GameObject zombie;

	// Use this for initialization
	void Start () {
		SwitchToDay();
		//SwitchToNight();


	
	}
	
	// Update is called once per frame
	void Update () {

		if (Time.fixedTime < nextGame) {
			if (!isDay) {
				ThinkNight();
			}

			return;
		}

		if (isDay) {
			SwitchToNight();
		} else {
			SwitchToDay();
		}
		

	}

	public void SwitchToDay(){
		var objs = FindObjectsOfType<survivorAI>();
		var mapGenerator = FindObjectOfType<MapGeneration3> ();

		foreach(var obj in GameObject.FindObjectsOfType<zombie>())
			Destroy(obj.gameObject);

		foreach(var obj in GameObject.FindObjectsOfType<survivorAI>())
			Destroy(obj.gameObject);
			
		//This generates the new players
		mapGenerator.GenerateGame ();

		foreach(var obj in GameObject.FindObjectsOfType<survivorAI>())
			obj.State = new SearchAI(obj);

		nextGame = Time.fixedTime + 30;
		isDay = true;
	}

	void ThinkNight()
	{
		var mapGenerator = FindObjectOfType<MapGeneration3> ();

		if (Time.fixedTime > nextZombie) {

			int blockX = 0;
			int blockY = 0;

			switch(Random.Range(0, 4)){
			case 0: //NORTH
				blockX = Random.Range(0, MapGeneration3.sizeX);
				blockY = 1;
				break;
			case 1: //EAST
				blockX = MapGeneration3.sizeX - 1;
				blockY = Random.Range(0, MapGeneration3.sizeY);
				break;
			case 2: //SOUTH
				blockX = Random.Range(0, MapGeneration3.sizeX);
				blockY = MapGeneration3.sizeY - 1;
				break;
			case 3: //WEST
				blockX = 1;
				blockY = Random.Range(0, MapGeneration3.sizeY);
				break;


			}

			if(!MapGeneration3.occupiedGrid[blockX][blockY]){
				Instantiate(zombie, MapGeneration3.convertGridToReal(blockX, blockY), Quaternion.identity);
				
			}

			nextZombie = Time.fixedTime + Random.Range(0.4f, 1.0f);
		}
	}
	
	public void SwitchToNight(){
		var objs = FindObjectsOfType<survivorAI>();

		foreach(var obj in GameObject.FindGameObjectsWithTag("Collectable"))
			Destroy(obj.gameObject);

		foreach(var obj in GameObject.FindObjectsOfType<survivorAI>())
			obj.State = new Night(obj);

		nextGame = Time.fixedTime + 30;
		isDay = false;
	}
}
