using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
	public int columns = 60;
	public int rows = 100;
	public GameObject redCastle;
	public int redCastleX = 3;
	public int redCastleY = 3;
	public GameObject blueCastle;
	public int blueCastleX = 57;
	public int blueCastleY = 97;
	public GameObject tree;
	public int treeCount = 25;
	public GameObject[] grass;
	private Transform mapHolder;

	public void MapSetup ()
	{
		mapHolder = new GameObject ("Map").transform;

		for (int x = 0; x < columns; x++) {
			for (int y = 0; y < rows; y++) {
				// Initialize grass
				GameObject toInstantiate = grass [Random.Range (0, grass.Length)];
				GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0F), Quaternion.identity) as GameObject; 
				instance.transform.SetParent (mapHolder);
			}
		}
		// Initialize castles
		GameObject instance_CastleRed = Instantiate (redCastle, new Vector3 (redCastleX, redCastleY, 0F), Quaternion.identity) as GameObject; 
		instance_CastleRed.transform.SetParent (mapHolder);
		GameObject instance_CastleBlue = Instantiate (blueCastle, new Vector3 (blueCastleX, blueCastleY, 0F), Quaternion.identity) as GameObject; 
		instance_CastleBlue.transform.SetParent (mapHolder);

		// Initialize trees
		List<Vector2> taken = new List<Vector2>{
					new Vector2 (redCastleX, redCastleY),
					new Vector2 (blueCastleX, blueCastleY)
				};
		for (int i = 0; i < treeCount; i++) {
			bool blocked;
			do {
				blocked = false;
				int treeX = Random.Range (0, columns);
				int treeY = Random.Range (0, rows);
				Vector2 treePos = new Vector2 (treeX, treeY);
				foreach (Vector2 something in taken) {
					if (something.Equals (treePos)) {
						blocked = true;
						break;	
					}
						
				}
				if (!blocked) {
					GameObject treeInstance = Instantiate (tree, new Vector3 (treeX, treeY, 0F), Quaternion.identity) as GameObject;
					taken.Add (treePos);
					treeInstance.transform.SetParent (mapHolder);
				}
			} while (blocked);	
		} 
	}
}
