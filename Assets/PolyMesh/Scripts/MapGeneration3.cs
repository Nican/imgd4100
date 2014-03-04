using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Map generator.
/// We are going to separate the game world into squares of random size and location, and place polygons into each of the squares
/// </summary>

public class MapGeneration3 : MonoBehaviour {

	int sizeX = 13;
	int sizeZ = 13;

	public static bool[][] occupiedGrid;

	// Use this for initialization
	void Start () {

		float verticalSize   = Camera.main.orthographicSize * 2.0f;
		float horizontalSize = verticalSize * Screen.width / Screen.height;

		sizeX = (int)horizontalSize - 2;
		sizeZ = (int)verticalSize - 2;

		//init occupiedGrid
		occupiedGrid = new bool[sizeX][];
		for(int i = 0; i < sizeX; i++)
		{
			occupiedGrid[i] = new bool[sizeZ];
		}

		makeBuildings (occupiedGrid, sizeX, sizeZ, 30);
		expandBuildings (occupiedGrid, sizeX, sizeZ, 70);
		makeRoadsH (occupiedGrid, sizeX, sizeZ, 4);
		makeRoadsV (occupiedGrid, sizeX, sizeZ, 4);

		GameObject rock = GameObject.Find ("Rock");

		Vector3 center; 
		GameObject rock2;
		PolyMesh a;

		for (int indx = 0; indx < sizeX; indx++)
		{
			for(int indz = 0; indz < sizeZ; indz++)
			{
				if(occupiedGrid[indx][indz])
				{
					center = new Vector3 ((float)(indx-sizeX/2),(float)(indz-sizeZ/2));
					rock2 = Instantiate (rock, center, Quaternion.identity) as GameObject;
					a = rock2.GetComponent<PolyMesh> ();

					a.makeUnique ();

					a.keyPoints.Clear ();
					a.isCurve.Clear ();

					a.keyPoints.Add (new Vector3 (-0.5f, -0.5f));
					a.keyPoints.Add (new Vector3 (0.5f, -0.5f));			
					a.keyPoints.Add (new Vector3 (0.5f, 0.5f));
					a.keyPoints.Add (new Vector3 (-0.5f, 0.5f));

					for (int i =0; i< a.keyPoints.Count; i++)
							a.isCurve.Add (false);

					a.BuildMesh ();
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {

	}

	/// <summary>
	/// Makes buildings initially
	/// </summary>
	/// <param name="city">The grid to create the buildings on</param>
	/// <param name="x">The x size of the grid.</param>
	/// <param name="z">The z size of the grid.</param>
	/// <param name="density">The density of the buildings out of 100.</param>
	void makeBuildings(bool[][] city, int x, int z, int density)
	{
		if (density > 90)
						density = 90;
		if (density < 10)
						density = 10;
		int indx, indz;
		for(indx = 0; indx < x; indx++)
		{
			for(indz = 0; indz < z; indz++)
			{
				if(Random.value*100 < density)
				{
					city[indx][indz] = true;
				}
			}
		}
	}

	/// <summary>
	/// Expands existing buildings, call after makeBuildings.
	/// </summary>
	/// <param name="city">The grid to create the buildings on</param>
	/// <param name="x">The x size of the grid.</param>
	/// <param name="z">The z size of the grid.</param>
	/// <param name="density">The density of the buildings out of 100.</param>
	void expandBuildings(bool[][] city, int x, int z, int density)
	{
		if (density > 90)
			density = 90;
		if (density < 10)
			density = 10;
		int indx, indz;
		for(indx = 0; indx < x; indx++)
		{
			for(indz = 0; indz < z; indz++)
			{
				if(Random.value*400 < (density*checkAdj (city,indx,indz)))
				{
					city[indx][indz] = true;
					density--;
				}
			}
		}
	}

	/// <summary>
	/// Makes horizontal roads.
	/// </summary>
	/// <param name="city">The grid to create the buildings on</param>
	/// <param name="x">The x size of the grid.</param>
	/// <param name="z">The z size of the grid.</param>
	/// <param name="num">The number of roads.</param>
	void makeRoadsH(bool[][] city, int x, int z, int num)
	{
		int indx, indz;
		for(indx = 0; indx < x; indx += 2)
		{
			if(Random.value*num > num/2)
			{
				for(indz = 0; indz < z; indz++)
				{
					city[indx][indz] = false;
					num--;
				}
			}
		}
	}

	/// <summary>
	/// Makes vertical roads.
	/// </summary>
	/// <param name="city">The grid to create the buildings on</param>
	/// <param name="x">The x size of the grid.</param>
	/// <param name="z">The z size of the grid.</param>
	/// <param name="num">The number of roads.</param>
	void makeRoadsV(bool[][] city, int x, int z, int num)
	{
		int indx, indz;
		for(indz = 0; indz < z; indz += 2)
		{
			if(Random.value*num > num/2)
			{
				for(indx = 0; indx < x; indx++)
				{
					city[indx][indz] = false;
					num--;
				}
			}
		}
	}

	/// <summary>
	/// Checks the number of adjacent filled squares
	/// </summary>
	/// <returns>The number of adjacent filled squares</returns>
	/// <param name="city">The grid to create the buildings on</param>
	/// <param name="x">The x coordinate.</param>
	/// <param name="z">The z coordinate.</param>
	int checkAdj(bool[][] city, int x, int z)
	{
		int num = 0;
		if (x < (sizeX - 1) && city [x + 1] [z])
						num++;
		if (x > 0 && city [x - 1] [z])
			num++;
		if (z < (sizeZ - 1) && city [x] [z + 1])
			num++;
		if (z > 0 && city [x] [z - 1])
			num++;
		return num;
	}
}