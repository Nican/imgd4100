using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MapGeneration2 : MonoBehaviour {

	Vector2 startPoint = new Vector2();
	Vector2 blockSize = new Vector2();
	Vector2 buildingSize = new Vector2();
	CityGenerator generator;

	int sizeX = 13;
	int sizeY = 13;

	
	// Use this for initialization
	void Start () {


		float verticalSize   = Camera.main.orthographicSize * 2.0f;
		float horizontalSize = verticalSize * Screen.width / Screen.height;

		
		startPoint.x = -horizontalSize / 2;
		startPoint.y = -verticalSize / 2;

		blockSize.x = horizontalSize / (sizeX - 1);
		blockSize.y = verticalSize / (sizeY - 1);

		buildingSize = blockSize * 0.6f;
		

		generator = new CityGenerator (sizeX - 2, sizeY - 2);

		for (int i = 0; i < 500; i++)
			generator.GlueBuildings ();



		//var building = new CityBuilding (5,5);
		//building.points.Add (new Point(0,-1));
		//building.points.Add (new Point(0,1));
		//building.points.Add (new Point(-1,0));
		//building.points.Add (new Point(1,0));
		//AddBuilding(building);

		for (int x = 0; x < sizeX -2 ; x++) {
			for(int y = 0; y < sizeX - 2; y++){
				CityBuilding building = generator.Grid[x,y];

				if( building.X == x && building.Y == y){
					AddBuilding(building);
				}
			}
		}
		
	}

	void AddBuilding(CityBuilding building)
	{
		GameObject rock = GameObject.Find ("Rock");
		Vector3 center = new Vector3(startPoint.x + blockSize.x * building.X + blockSize.x , startPoint.y + blockSize.y * building.Y + blockSize.y * 1);
		GameObject rock2 = Instantiate (rock, center, Quaternion.identity) as GameObject;
		PolyMesh a = rock2.GetComponent<PolyMesh> ();
		
		a.makeUnique ();
		
		a.keyPoints.Clear ();
		a.isCurve.Clear ();

		List<Vector3> pts = new List<Vector3>();

		pts.Add(new Vector3(-buildingSize.x/2, -buildingSize.y/2));

		if(building.HasPoint(new Point(0,1))){
			pts.Add(new Vector3(-buildingSize.x/2, -buildingSize.y/2 - blockSize.y));
			pts.Add(new Vector3(buildingSize.x/2, -buildingSize.y/2 - blockSize.y));
		}

		pts.Add(new Vector3(buildingSize.x/2, -buildingSize.y/2));

		if(building.HasPoint(new Point(1,0))){
			pts.Add(new Vector3(buildingSize.x/2 + blockSize.x, -buildingSize.y/2));
			pts.Add(new Vector3(buildingSize.x/2 + blockSize.x, buildingSize.y/2));
		}

		pts.Add(new Vector3(buildingSize.x/2, buildingSize.y/2));

		if(building.HasPoint(new Point(0,-1))){
			pts.Add(new Vector3(buildingSize.x/2, buildingSize.y/2 + blockSize.y));
			pts.Add(new Vector3(-buildingSize.x/2, buildingSize.y/2 + blockSize.y));
		}

		pts.Add(new Vector3(-buildingSize.x/2, buildingSize.y/2));

		if(building.HasPoint(new Point(-1, 0))){
			pts.Add(new Vector3(-buildingSize.x/2 - blockSize.x, buildingSize.y/2));
			pts.Add(new Vector3(-buildingSize.x/2 - blockSize.x, -buildingSize.y/2));
		}

		pts.Reverse ();

		a.keyPoints.AddRange (pts);

		/*
		a.keyPoints.Add(new Vector3(-rect.width/2, -rect.height/2));
		a.keyPoints.Add(new Vector3(rect.width/2, -rect.height/2));			
		a.keyPoints.Add(new Vector3(rect.width/2, rect.height/2));
		a.keyPoints.Add(new Vector3(-rect.width/2, rect.height/2));
		*/

		for(int i =0; i< a.keyPoints.Count; i++)
			a.isCurve.Add(false);
		
		a.BuildMesh ();

	

	}


	void OnGUI () {
		for (int x = 0; x < sizeX -2 ; x++) {
			for(int y = 0; y < sizeX - 2; y++){
				CityBuilding building = generator.Grid[y,x];
				
				GUI.Label (new Rect (Screen.width / (sizeX-2) * x,Screen.height / (sizeY-2) * y,100,100), "("+ building.X + ","+ building.Y + ")");



			}
		}

	}

	// Update is called once per frame
	void Update () {
	
	}


}

public struct Point {
	public int x;
	public int y;

	public Point(int x, int y){
		this.x = x;
		this.y = y;
	}
}

class CityBuilding {
	public int X;
	public int Y;

	public List<Point> points = new List<Point>();

	public CityBuilding(int x, int y){
		this.X = x;
		this.Y = y;

		points.Add (new Point (0, 0));
	}

	public bool HasPoint(Point point)
	{
		foreach (Point pt in points) {
			if(pt.x == point.x && pt.y == point.y)
				return true;
		}
		return false;
	}
	
}

class CityGenerator {

	public int sizeX;
	public int sizeY;

	public CityBuilding[,] Grid;

	public CityGenerator(int sizeX, int sizeY){
		this.sizeX = sizeX;
		this.sizeY = sizeY;

		Grid = new CityBuilding[sizeX,sizeY];

		for (int x = 0; x < sizeX; x++) {
			for(int y = 0; y < sizeX; y++){
				Grid[x,y] = new CityBuilding(x,y);
			}
		}


	}


	public void GlueBuildings(){
		//Find a random building
		int x = Random.Range (0, sizeX);
		int y = Random.Range (0, sizeY);

		CityBuilding building = Grid [x, y];

		if (building.points.Count > 4) {
			return;
		}

		if (x != building.X || y != building.Y) {
			return;
		}

		//Direction to move in (A value between -1 and 1)
		int dirX = 0;
		int dirY = 0;

		//Do not stay in the same place
		while(dirX == 0 && dirY == 0)
		{
			dirX = Random.Range (-1, 2);
			dirY = Random.Range (-1, 2);
		}

		if (x + dirX >= sizeX || y + dirY >= sizeY || x + dirX < 0 ||  y + dirY < 0)
			return;

		CityBuilding other = Grid [x + dirX, y + dirY];

		//Lets not merge with other large buildings to avoid loops and unreachable space
		if (other.points.Count > 1 || x + dirX != other.X || y + dirY != other.Y ) {
			return;
		}

		//Find the relation to the actual building.
		int relX = x + dirX - building.X;
		int relY = y + dirY - building.Y;

		if (Mathf.Abs (relX) > 1 || Mathf.Abs (relY) > 1 || (Mathf.Abs (relX) == 1 && Mathf.Abs (relY) == 1))
			return;

		building.points.Add(new Point(relX, relY));

		Grid [x + dirX, y + dirY] = building;
	}


}
