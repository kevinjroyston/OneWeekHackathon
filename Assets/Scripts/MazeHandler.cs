using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeHandler : MonoBehaviour {
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public int length;
    public int width;
    public TileNode[,] maze;

	// Use this for initialization
	void Start () {
        maze = new TileNode[width, length];
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
public class TileNode
{
    TileHelper tile;
    WallHelper north;
    WallHelper east;
    WallHelper south;
    WallHelper west;
    public TileNode(GameObject tile, GameObject north, GameObject east, GameObject south, GameObject west)
    {
        this.tile = tile.GetComponent<TileHelper>();
        this.north = tile.GetComponent<WallHelper>();
        this.east = east.GetComponent<WallHelper>();
        this.south = south.GetComponent<WallHelper>();
        this.west = west.GetComponent<WallHelper>();
    }
    public bool isConnected(Vector2 direction)
    {
        if (direction.x != 0 && direction.y != 0 || direction.magnitude == 0)
            Debug.LogError("Bad direction passed into isConnected");
        direction.Normalize();
        WallHelper wall = direction.x == 1 ? east : direction.x == -1 ? west : direction.y == 1 ? north : south;
        return wall == null? false : !wall.getIsActive();
    }
}
