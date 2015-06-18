using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class MapManager : MonoBehaviour
{
    public GameObject tree;
    public int treeCount;

    public void MapSetup()
    {
        // For each tree:
        for (int i = 0; i < treeCount; i++)
        {
            // Generate random positions until you find one that is not blocked.
            float treeX = Random.Range(-25, 25);
            float treeZ = Random.Range(-25, 25);
            Instantiate(tree, new Vector3(treeX, 0F, treeZ), Quaternion.identity);
        }
    }
}