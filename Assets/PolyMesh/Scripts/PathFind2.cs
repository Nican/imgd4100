using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// We are going to find a path by A*. Generating a grid outwards growing from the character.
/// </summary>
public class PathFind2 : MonoBehaviour {

	int resolution = 10;

	int nonCharacterMask = ~(1 << 8);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {



	}

	void OnGUI () {

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		/*if (Physics.Raycast (ray, 100, out hit )) {



			IEnumerator<Vector2> path = FindPath (hit.point, new Vector2(0,0)).GetEnumerator();

			Vector2 current = path.Current;

			while(path.MoveNext()){

			}




		}*/




	}

	//Taken directly from wikipedia.
	public IEnumerable<Vector2> FindPath(Vector3 startVector, Vector3 end){

		List<Point> neighbors = new List<Point>();
		neighbors.Add (new Point (1, 0));
		neighbors.Add (new Point (-1, 0));
		neighbors.Add (new Point (0, 1));
		neighbors.Add (new Point (0, -1));

		Point start = new Point ((int)(startVector.x * resolution),(int)(startVector.y * resolution));
		Point goal = new Point ((int)(end.x * resolution), (int)(end.y * resolution));

		List<Point> closedset = new List<Point>();
		List<Point> openset = new List<Point>();
		Dictionary<Point, Point> came_from = new Dictionary<Point, Point> ();


		openset.Add (start);

		Dictionary<Point, float> g_score = new Dictionary<Point, float> ();
		Dictionary<Point, float> f_score = new Dictionary<Point, float> ();

		g_score [start] = 0.0f;
		f_score [start] = g_score [start] + distance (start, goal);

		while (openset.Count > 0) {
			//Find the minimun
			Point current = openset.Aggregate((c, d) => f_score[c] < f_score[d] ? c : d);

			if(current.Equals(goal)){
				return ReconstructPath(came_from, goal).Reverse();
			}

			openset.Remove(current);
			closedset.Add(current);

			IEnumerable<Point> currentNeighbors = neighbors.Where((pt)=> { 
				return Physics.Raycast ( PointToVector(current), PointToVector(pt), 1.0f/resolution, nonCharacterMask);
			});

			foreach(Point delta in currentNeighbors)
			{
				Point neighbor = new Point(current.x + delta.x, current.y + delta.y);

				if(closedset.Contains(neighbor))
					continue;

				float tentative_g_score = g_score[current] + 1.0f;

				if(!openset.Contains(neighbor) || tentative_g_score < g_score[neighbor])
				{
					came_from[neighbor] = current;
					g_score[neighbor] = tentative_g_score;
					f_score[neighbor] = g_score[neighbor] + distance(neighbor, goal);
					if(!openset.Contains(neighbor))
						openset.Add(neighbor);
				}

			}

		}

		return null;

	}

	public IEnumerable<Vector2> ReconstructPath(Dictionary<Point, Point> came_from, Point start_node){
		/*
		Point node = start_node;

		yield return start_node;

		while (came_from.ContainsKey[node]) 
		{
			node = came_from[node];
			yield return node;
		}

		/*
		 * 
			if current_node in came_from
       	 		p := reconstruct_path(came_from, came_from[current_node])
        		return (p + current_node)
   			else
        		return current_node
		*/
		return null;
	}

	public Vector2 PointToVector( Point pt ){
		return new Vector3(pt.x * resolution, pt.y * resolution);
	}

	public float distance(Point a, Point b){
		return Mathf.Sqrt ( Mathf.Pow(a.x-b.x, 2) + Mathf.Pow(a.y-b.y,2)  );
	}


}
