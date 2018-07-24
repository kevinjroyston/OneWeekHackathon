using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerPathfinding : MonoBehaviour {
    private NavMeshAgent agent;
    private GameObject[] goals;

	// Use this for initialization
	void Start () {
        agent = gameObject.GetComponent<NavMeshAgent>();
        goals = GameObject.FindGameObjectsWithTag("Goal");
	}

    /*
     *  Movable walls should be an array of walls excluding the wall that was just disabled.
     */
    public void onWallEnabled(GameObject enableWall, Queue<GameObject> movableWalls)
    {
        enableWall.GetComponent<MeshRenderer>().enabled = true;
        if (isPathToGoal())
        {
            Debug.Log("The wall enabled did not block the path");
            return;
        }
        Debug.Log("The wall disabled did block the path");
        disableWallToMakePath(enableWall, movableWalls);
    }

    private void disableWallToMakePath(GameObject enableWall, Queue<GameObject> movableWalls)
    {
        if (movableWalls.Count == 0)
        {
            Debug.LogError("There is no other wall to disable, disabling wall");
            enableWall.GetComponent<MeshRenderer>().enabled = false;
        }
        GameObject wall = movableWalls.Dequeue();
        wall.GetComponent<MeshRenderer>().enabled = false;
        if (isPathToGoal())
        {
            Debug.Log("Found a wall to disable");
        } else
        {
            GetComponent<MeshRenderer>().enabled = true;
            disableWallToMakePath(enableWall, movableWalls);
        }
    }
	
	private bool isPathToGoal()
    {
        NavMeshPath path = new NavMeshPath();
        foreach (var goal in goals)
        {
            agent.CalculatePath(goal.transform.position, path);
            if (path.status == NavMeshPathStatus.PathComplete)
            {
                return true;
            }
        }
        return false;
    }
}
