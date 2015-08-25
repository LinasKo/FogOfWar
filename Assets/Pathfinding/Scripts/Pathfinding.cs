using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum PathfinderType
{
    GridBased,
    WaypointBased
}

public class Pathfinding : MonoBehaviour 
{
    public List<Vector3> Path = new List<Vector3>();
    public PathfinderType PathType = PathfinderType.GridBased;
	public bool JS = false;
    public Player allegiance;

    private Pathfinder pathfinder;
    private Vector3 target = Vector3.zero; 

    public void FindPath(Vector3 startPosition, Vector3 endPosition)
    {
        if (PathType == PathfinderType.GridBased)
        {
            pathfinder.InsertInQueue(startPosition, endPosition, SetList);
            if (Path.Count == 0)
            {
                target = Vector3.zero;
            }
            else
            {
                target = endPosition;
            }
        }
        else if (PathType == PathfinderType.WaypointBased)
        {
            WaypointPathfinder.Instance.InsertInQueue(startPosition, endPosition, SetList);          
        }
    }
	
	public void FindJSPath(Vector3[] arr)
    {
        if(arr.Length > 1)
		{	
			if (PathType == PathfinderType.GridBased)
	        {
	            pathfinder.InsertInQueue(arr[0], arr[1], SetList);
	        }
	        else if (PathType == PathfinderType.WaypointBased)
	        {
	            WaypointPathfinder.Instance.InsertInQueue(arr[0], arr[1], SetList);
	        }
		}
    }

    public void Start()
    {
        if (allegiance == Player.RED)
        {
            pathfinder = GameObject.Find("Pathfinder_red").GetComponent<Pathfinder>();
        }
        else
        {
            pathfinder = GameObject.Find("Pathfinder_blue").GetComponent<Pathfinder>();
        }
    }

    public void Update()
    {
        if (target != Vector3.zero)
        {
            Move();
            if (Path.Count == 0)
            {
                target = Vector3.zero;
            }
        }
    }

    public bool IsMoving()
    {
        return target != Vector3.zero; 
    }

    //A test move function, can easily be replaced
    public void Move()
    {
        if (Path.Count > 0)
        {  
            transform.position = Vector3.MoveTowards(transform.position, Path[0], Time.deltaTime * 30F);
            if (Vector3.Distance(transform.position, Path[0]) < 0.4F)
            {
                Path.RemoveAt(0);
            }
        }
    }

    protected virtual void SetList(List<Vector3> path)
    {
        if (path == null)
        {
            return;
        }
		
		if(!JS)
		{
	        Path.Clear();
	        Path = path;
            if (Path.Count > 0)
            {
                //Path[0] = new Vector3(Path[0].x, Path[0].y - 1, Path[0].z);
                //Path[Path.Count - 1] = new Vector3(Path[Path.Count - 1].x, Path[Path.Count - 1].y - 1, Path[Path.Count - 1].z);
            }
		}
		else
		{
			Vector3[] arr = new Vector3[path.Count];
			for(int i = 0; i < path.Count; i++)
			{
				arr[i] = path[i];
			}
            if (arr.Length > 0)
            {
               // arr[0] = new Vector3(arr[0].x, arr[0].y - 1, arr[0].z);
               // arr[arr.Length - 1] = new Vector3(arr[arr.Length - 1].x, Path[arr.Length - 1].y - 1, Path[arr.Length - 1].z);
                gameObject.SendMessage("GetJSPath", arr);
            }
		}
    }
}
	
