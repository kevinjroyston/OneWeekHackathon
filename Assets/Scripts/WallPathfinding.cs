using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WallPathfinding : MonoBehaviour {
    // All players under one root object
    // Each player must have a navmesh agent attached
    public GameObject players;

    // All movable walls under one root object
    // Each wall should have a NavMesh Object attached
    public GameObject walls;
    private GameObject[] goals;

    // Use this for initialization
    void Start()
    {
        walls = GameObject.Find("MazeWalls");
        players = GameObject.Find("PlayerHolder");
        goals = GameObject.FindGameObjectsWithTag("Goal");
    }

    /*
     *  Movable walls should be an array of walls excluding the wall that was just disabled.
     */
    public void onWallEnabled(GameObject enableWall, Queue<GameObject> movableWalls)
    {
        if (players.transform.childCount == 0)
            return;
        enableWall.GetComponent<WallHelper>().ToggleWall(true);
        StartCoroutine(isPathToGoal(isPath =>
        {
            if (!isPath)
            {
                Debug.Log("The wall disabled did block the path");
                //disableWallToMakePath(enableWall, movableWalls);
                enableWall.GetComponent<WallHelper>().ToggleWall(false);
            } else
            {
                Debug.Log("The wall disabled did not block the path");
            }
        }));
    }

    private void disableWallToMakePath(GameObject enableWall, Queue<GameObject> movableWalls)
    {
        if (movableWalls.Count == 0)
        {
            Debug.LogError("There is no other wall to open the path, disabling wall that was just enabled");
            enableWall.GetComponent<WallHelper>().ToggleWall(false);
            return;
        }
        var wall = movableWalls.Dequeue();
        wall.GetComponent<WallHelper>().ToggleWall(false);
        StartCoroutine(isPathToGoal(isPath =>
        {
            if (!isPath)
            {
                wall.GetComponent<WallHelper>().ToggleWall(true);
                disableWallToMakePath(enableWall, movableWalls);
            } else
            {
                Debug.Log("Disabled a wall to make a path");
            }
        }));
    }

    public IEnumerator isPathToGoal(System.Action<bool> callback)
    {
        yield return new WaitForSeconds(0.1f);
        foreach (NavMeshAgent agent in players.GetComponentsInChildren<NavMeshAgent>())
        {
            foreach (var goal in goals)
            {
                NavMeshPath path = new NavMeshPath();
                bool result = agent.CalculatePath(goal.transform.position, path);
                if (!result || path.status != NavMeshPathStatus.PathComplete)
                {
                    callback(false);
                } else
                {
                    LineRenderer line = agent.GetComponent<LineRenderer>();
                    if (line != null)
                    {
                        line.positionCount = path.corners.Length; ;
                        for (int i = 0; i < path.corners.Length; i++)
                        {
                            line.SetPosition(i, path.corners[i]);
                        }
                    }
                }
            }
        }
        callback(true);
    }

    private void OnMouseUp()
    {
        //if (!Network.isServer)
          //  return;
        //Debug.Log("OnMouseUp");
        if (!gameObject.GetComponent<WallHelper>().IsToggled())
        {
            onWallEnabled(gameObject, new Queue<GameObject>(getOtherWalls()));
        } else
        {
            gameObject.GetComponent<WallHelper>().ToggleWall(false);
        }
    }

    private List<GameObject> getOtherWalls()
    {
        List<GameObject> otherWalls = new List<GameObject>();
        Debug.Log(walls);
        foreach (Transform child in walls.transform)
        {
            GameObject wall = child.gameObject;
            if (wall != gameObject && wall.GetComponent<WallHelper>().IsToggled())
            {
                otherWalls.Add(wall);
            }
        }
        Debug.Log("Other movable walls: " + otherWalls.Count);
        return otherWalls;
    }
}
