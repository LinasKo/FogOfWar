using UnityEngine;
using System.Collections;

public class FogManager : MonoBehaviour
{

    public GameObject redFogTile;
    public GameObject blueFogTile;

    private int columns;
    private int rows;

    // Use this for initialization
    public void InitializeFog(int mapCols, int mapRows)
    {
        columns = mapCols;
        rows = mapRows;

        // For each cell
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                // Initialize fog for Red and Blue
                Instantiate(redFogTile, new Vector3(x, y, 0F), Quaternion.identity);
                Instantiate(blueFogTile, new Vector3(x, y, 0F), Quaternion.identity);
            }
        }
    }

    public void removeFog(int x, int y, Player color)
    {
        
    }
}
