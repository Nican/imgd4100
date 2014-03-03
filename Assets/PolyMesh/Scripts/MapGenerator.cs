using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Map generator.
/// We are going to separate the game world into squares of random size and location, and place polygons into each of the squares
/// </summary>

public class MapGenerator : MonoBehaviour {

	// Use this for initialization
	void Start () {
		float verticalSize   = Camera.main.orthographicSize * 2.0f;
		float horizontalSize = verticalSize * Screen.width / Screen.height;

		GameObject rock = GameObject.Find ("Rock");
		MapGeneratorInternal generator = new MapGeneratorInternal (new Vector2(horizontalSize, verticalSize));

		for (int i = 0; i < 10000; i++) {
			generator.Generate ();
		}

		Rect center1 = new Rect ();
		Rect center2 = new Rect ();
		Rect center3 = new Rect ();

		center1.Set (horizontalSize / 2, 0, horizontalSize / 100, verticalSize);
		center2.Set (0, verticalSize / 2, horizontalSize, verticalSize / 100);
		center2.Set (horizontalSize * 0.45f, verticalSize * 0.45f, horizontalSize * 0.10f, verticalSize * 0.10f);


		foreach(var rect in generator.Rects){
			if(Random.Range(0,2) != 0)
				continue;

			if(rect.Overlaps(center1) || rect.Overlaps(center2))
				continue;

			Vector3 center = new Vector3(rect.x + rect.width/2 - horizontalSize/2, rect.y + rect.height / 2-verticalSize/2);
			GameObject rock2 = Instantiate (rock, center, Quaternion.identity) as GameObject;
			PolyMesh a = rock2.GetComponent<PolyMesh> ();
			
			a.makeUnique ();
			
			a.keyPoints.Clear ();
			a.isCurve.Clear ();

			a.keyPoints.Add(new Vector3(-rect.width/2, -rect.height/2));
			a.keyPoints.Add(new Vector3(rect.width/2, -rect.height/2));			
			a.keyPoints.Add(new Vector3(rect.width/2, rect.height/2));
			a.keyPoints.Add(new Vector3(-rect.width/2, rect.height/2));

			for(int i =0; i< a.keyPoints.Count; i++)
				a.isCurve.Add(false);
			
			a.BuildMesh ();

		}
	
	}
	
	// Update is called once per frame
	void Update () {
	}
}


class MapGeneratorInternal {

	enum Direction {
		North,
		South,
		East,
		West
	}

	public List<Rect> Rects = new List<Rect>();

	Vector2 Size;
	Vector2 DeltaSearch;
	float MinSize;

	public MapGeneratorInternal(Vector2 size)
	{
		this.Size = size;
		DeltaSearch = Size / 300;
		MinSize = this.Size.x * this.Size.y * 0.01f;


		//Add an initial rectangle to the map
		Rect initialRect = new Rect();
		initialRect.width = Random.Range (0.01f, Size.x / 5);
		initialRect.height = Random.Range (0.01f, Size.x / 5);

		initialRect.x = 0; //Random.Range (0.0f, Size.x / 2);
		initialRect.y = size.y - initialRect.height; //Random.Range (0.0f, Size.x / 2);

		Rects.Add (initialRect);


	}

	public bool IsOpenPoint(Vector2 point)
	{
		if (point.x < 0 || point.x > Size.x || point.y < 0 || point.y > Size.y)
			return false;

		foreach (var rect in Rects) {
			if(rect.Contains(point)){
				return false;
			}
		}

		return true;
	}

	public bool IsOpenRect(Rect target)
	{
		Rect world = new Rect ();
		world.x = 0.0f;
		world.y = 0.0f;
		world.width = Size.y;
		world.height = Size.y;

		if (target.x < 0 || target.y < 0)
			return false;

		if ((target.x + target.width) > Size.x || (target.y + target.height) > Size.y)
			return false;

		foreach (var rect in Rects) {
			if(rect.Overlaps(target))
				return false;
		}

		return true;
	}

	public void Generate()
	{
		Rect referenceRect = RandomRect();

		//Find a edge to attach ourselfs to
		System.Array values = System.Enum.GetValues(typeof(Direction));
		Direction direction = (Direction) values.GetValue(Random.Range(0,values.Length));

		Vector2 StartPoint = new Vector2();
		Vector2 SearchVelocity = new Vector2();
		Vector2 GrowVelocity = new Vector2();

		switch (direction) 
		{
		case Direction.North:
			StartPoint.x = referenceRect.x + referenceRect.width * Random.value;
			StartPoint.y = referenceRect.y - 0.01f;

			SearchVelocity.x = Random.Range(0,2) == 0 ? 1.0f : -1.0f;
			SearchVelocity.y = 0.0f;

			GrowVelocity.x = SearchVelocity.x;
			GrowVelocity.y = 1.0f;
			break;

		case Direction.South:
			StartPoint.x = referenceRect.x + referenceRect.width * Random.value;
			StartPoint.y = referenceRect.y + referenceRect.height + 0.01f;
			
			SearchVelocity.x = Random.Range(0,2) == 0 ? 1.0f : -1.0f;
			SearchVelocity.y = 0.0f;
			
			GrowVelocity.x = SearchVelocity.x;
			GrowVelocity.y = -1.0f;
			break;

		case Direction.East:
			StartPoint.x = referenceRect.x + referenceRect.width + 0.01f;
			StartPoint.y = referenceRect.y + referenceRect.height * Random.value;
			
			SearchVelocity.x = 0.0f;
			SearchVelocity.y = Random.Range(0,2) == 0 ? 1.0f : -1.0f;
			
			GrowVelocity.x = 1.0f;
			GrowVelocity.y = SearchVelocity.y;
			break;

		case Direction.West:
			StartPoint.x = referenceRect.x - 0.01f;
			StartPoint.y = referenceRect.y + referenceRect.height * Random.value;
			
			SearchVelocity.x = 0.0f;
			SearchVelocity.y = Random.Range(0,2) == 0 ? 1.0f : -1.0f;
			
			GrowVelocity.x = -1.0f;
			GrowVelocity.y = SearchVelocity.y;
			break;

		}

		GrowVelocity.x *= Random.value;
		GrowVelocity.y *= Random.value;
		GrowVelocity /= Mathf.Max (GrowVelocity.x, GrowVelocity.y); //Normalize new velocity such that either x or y is 1
		GrowVelocity.x *= DeltaSearch.x; //Scale it down to a good iteration value
		GrowVelocity.y *= DeltaSearch.y;
		SearchVelocity.x *= DeltaSearch.x;
		SearchVelocity.y *= DeltaSearch.y;

		int counter = 128;
		while (!IsOpenPoint(StartPoint) && counter >= 0) {
			StartPoint += SearchVelocity;
			counter--;
		}

		if (counter == 0)
			return;

		float maxSize = Random.value * Size.x * Size.y * 0.02f;
		Rect newRect = new Rect ();
		newRect.x = StartPoint.x;
		newRect.y = StartPoint.y;

		newRect = Grow (newRect, GrowVelocity);

		while(newRect.width * newRect.height < maxSize){
			if(!IsOpenRect(newRect))
				break;

			newRect = Grow (newRect, GrowVelocity);
		}

		if(!IsOpenRect(newRect))
			return;

		if (newRect.width * newRect.height < MinSize)
			return;

		if ((Mathf.Min(newRect.width, newRect.height) / Mathf.Max (newRect.width, newRect.height)) < 0.05)
			return;

		newRect.x -= Mathf.Abs(DeltaSearch.x);
		newRect.y -= Mathf.Abs(DeltaSearch.y);
		newRect.width += Mathf.Abs(DeltaSearch.x) * 2.0f;
		newRect.height += Mathf.Abs(DeltaSearch.y) * 2.0f;

		Rects.Add (newRect);
	}

	public Rect Grow(Rect rect, Vector2 velocity){
		Rect retnRect = rect;

		retnRect.width += Mathf.Abs( velocity.x );
		retnRect.height += Mathf.Abs( velocity.y );

		if (velocity.x < 0) {
			retnRect.x += velocity.x;
		}

		if (velocity.y < 0) {
			retnRect.y += velocity.y;
		}

		return retnRect;
	}


	public Rect RandomRect()
	{
		return Rects [Random.Range (0, Rects.Count)];
	}



}