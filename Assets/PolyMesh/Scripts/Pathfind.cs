using UnityEngine;
using System.Collections;

public class Pathfind : ScriptableObject {

	public float xEnd, yEnd;
	public Pathfind next;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

	/// <summary>
	/// Generates a path from the starting position to (finalX,finalY)
	/// </summary>
	/// <param name="finalX">X position of the destinaton.</param>
	/// <param name="finalY">Y position of the destinaton.</param>
	/// <param name="forcedir">Prevents zigzagging</param>
	/// <param name="maxRec">maximum recursions. If this is passed, the character will say the path is "too confusing" and quit. Setting to a negative number disables this.</param>
	public string GenNext(float finalX, float finalY, int forcedir = 0, int maxRec = 5)
	{
		if(maxRec == 0)
		{
			return "That path is too confusing\n";
		}
		Pathfind p = (Pathfind)ScriptableObject.CreateInstance<Pathfind>();

		Vector3 vStart = new Vector3(xEnd, yEnd);
		Vector3 vEnd = new Vector3 (finalX, finalY);


		//represents the direction from the end of the current path to the start of the next. vDir will move clockwise and vDirb will move counterclockwise
		Vector3 vDir = vStart - vEnd;
		vDir.Normalize ();
		vDir = vDir * (-1);
		Vector3 vDirb = new Vector3 (vDir.x, vDir.y);

		//Sets the maximum raycast distance, or if no obstance is detected immediately creates the final path
		float maxDist = Vector3.Distance(vStart,vEnd);
		RaycastHit rHit;
		Physics.Raycast (vStart, vDir, out rHit, maxDist);
		if(rHit.collider == null)
		{
			p.xEnd = finalX;
			p.yEnd = finalY;
			next = p;
			return "Moving to position";
		}

		//Rotates vDir and vDirb in opposite directions until one or the other finds a clear path
		//forcedir will allow the pathfind to ignore one direction or another
		while((forcedir == 2 || Physics.Raycast(vStart,vDir,maxDist)) && (forcedir == 1 || Physics.Raycast(vStart,vDirb,maxDist)))
		{
			vDir = Vector3.Cross (vDir, (Vector3.forward * Mathf.PI * 0.1F));
			vDirb = Vector3.Cross (vDirb, (Vector3.forward * Mathf.PI * -0.1F));
			if(vDir.x == vDirb.x && vDir.y == vDirb.y)
			{
				return "I got lost\n";
			}
		}

		float xTest = xEnd;
		float yTest = yEnd;
		float distTest = Mathf.Pow(xEnd,2) + Mathf.Pow(yEnd,2); //the sqrt doesn't matter, since all I care about is if distance is decreasing or increasing)

		//moves a test point in the direction selected until it starts getting further away from the final destination
		if(Physics.Raycast(vStart,vDirb) && !(forcedir == 2))
		{
			forcedir = 1;
			while(distTest >= (Mathf.Pow(xTest,2) + Mathf.Pow(yTest,2)))
			{
				distTest = Mathf.Pow(xTest,2) + Mathf.Pow(yTest,2);
				xTest += vDir.x;
				yTest += vDir.y;
			}
		}
		else
		{
			forcedir = 2;
			while(distTest >= ((Mathf.Pow(xTest,2) + Mathf.Pow(yTest,2)) - 10))
			{
				distTest = Mathf.Pow(xTest,2) + Mathf.Pow(yTest,2);
				xTest += vDirb.x;
				yTest += vDirb.y;
			}
		}


		p.xEnd = xTest;
		p.yEnd = yTest;
		next = p;
		MonoBehaviour.print ("path to (+ " + xTest + "," + yTest + ")");

		//If the final destination isn't found, keep going until it is
		if(((Mathf.Pow(p.xEnd - finalX,2)) > 1) || ((Mathf.Pow(p.yEnd - finalY,2)) > 1))
		{
			return next.GenNext(finalX, finalY, forcedir, maxRec - 1);
		}
		return "Moving to position";
	}
}
