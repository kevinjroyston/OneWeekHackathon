using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class MazeHandler : MonoBehaviour
{
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public int height;
    public int width;
    public TileNode[,] maze;

    // Use this for initialization
    void Start()
    {
        FindMaze();
    }

    public void FindMaze() //This is so hacky, im so sorry
    {
        maze = new TileNode[width, height];

        GameObject wall = Instantiate(wallPrefab);
        Vector3 wallScale = wall.transform.localScale;
        float wallWidth = wallScale.x;
        float wallHeight = wallScale.y;
        float wallLength = wallScale.z;
        Destroy(wall);
        Vector3 currentlocation = Vector2.zero;
        Vector3 rowStart = currentlocation;
        for (int j = 0; j < height; j++)
        {
            currentlocation = rowStart;
            for (int i = 0; i < width; i++)
            {
                GameObject tile = GameObject.Find((currentlocation + new Vector3(0, -.2f, 0)).ToString());
                GameObject north = GameObject.Find((currentlocation + new Vector3(0f, wallHeight/2f, -wallLength / 2f)).ToString());
                GameObject east = GameObject.Find((currentlocation + new Vector3(wallLength/2f, wallHeight / 2f, 0f)).ToString());
                GameObject south = GameObject.Find((currentlocation + new Vector3(0f, wallHeight / 2f, wallLength / 2f)).ToString());
                GameObject west = GameObject.Find((currentlocation + new Vector3(-wallLength / 2f, wallHeight / 2f, 0f)).ToString());
                

                TileNode currentTile = new TileNode(tile, north, east, south, west);
                currentlocation += new Vector3(1f, 0f, 0f) * wallLength;
                maze[i, j] = currentTile;
            }
            rowStart = rowStart + new Vector3(0f, 0f, -1f) * wallLength;
        }
        Debug.Log(maze);
    }

    public void BuildMaze()
    {

        maze = new TileNode[width, height];
        GameObject mazeFloorTilesParent = GameObject.Find("MazeTiles");

        GameObject wall = Instantiate(wallPrefab);
        Vector3 wallScale = wall.transform.localScale;
        float wallWidth = wallScale.x;
        float wallHeight = wallScale.y;
        float wallLength = wallScale.z;
        DestroyImmediate(wall);
        Vector3 currentlocation = Vector2.zero;
        Vector3 rowStart = currentlocation;
        for (int j = 0; j < height; j++)
        {
            currentlocation = rowStart;
            for (int i = 0; i < width; i++)
            {
                WallHelper north = SafeCheckIfWallExists(i, j, Vector2.up);
                WallHelper east = SafeCheckIfWallExists(i, j, Vector2.right);
                WallHelper south = SafeCheckIfWallExists(i, j, Vector2.down);
                WallHelper west = SafeCheckIfWallExists(i, j, Vector2.left);
                GameObject tile = (GameObject)PrefabUtility.InstantiatePrefab(floorPrefab);
                tile.transform.localScale = new Vector3(wallLength, .4f, wallLength);
                tile.transform.SetParent(mazeFloorTilesParent.transform);
                tile.transform.localPosition = currentlocation + new Vector3(0, -.2f, 0);
                tile.name = tile.transform.localPosition.ToString();
                if (north == null)
                {
                    north = MakeNewWall(currentlocation + new Vector3(0f, 0f, -wallLength / 2f), 90f).GetComponent<WallHelper>();
                }
                if (east == null)
                {
                    east = MakeNewWall(currentlocation + new Vector3(wallLength / 2f, 0f, 0f), 0f).GetComponent<WallHelper>();
                }
                if (south == null)
                {
                    south = MakeNewWall(currentlocation + new Vector3(0f, 0f, wallLength / 2f), 90f).GetComponent<WallHelper>();
                }
                if (west == null)
                {
                    west = MakeNewWall(currentlocation + new Vector3(-wallLength / 2f, 0f, 0f), 0f).GetComponent<WallHelper>();
                }
                
                TileNode currentTile = new TileNode(tile, north, east, south, west);
                currentlocation += new Vector3(1f, 0f, 0f) * wallLength;
                maze[i, j] = currentTile;
            }
            rowStart = rowStart + new Vector3(0f, 0f, -1f) * wallLength;
        }
    }
    private GameObject MakeNewWall(Vector3 currentPosition, float rotation)
    {
        GameObject newWall = (GameObject)PrefabUtility.InstantiatePrefab(wallPrefab);
        newWall.transform.SetParent(transform);
        currentPosition.y = newWall.transform.localScale.y / 2f;
        newWall.transform.localPosition = currentPosition;
        newWall.transform.localEulerAngles = new Vector3(0f, rotation, 0f);
        newWall.name = newWall.transform.localPosition.ToString();
        newWall.GetComponent<WallHelper>().CreateNewMaterialInstance();
        return newWall;
    }

    public TileNode SafeGetTile(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0 && y < height) ? maze[x, y] : null;
    }
    private WallHelper SafeCheckIfWallExists(int x, int y, Vector2 direction)
    {

        if (direction.x != 0 && direction.y != 0 || direction.magnitude == 0)
            Debug.LogError("Bad direction passed into safeCheckIfwallExists");
        direction.Normalize();
        TileNode neighborTile = SafeGetTile(x + (int)direction.x, y + (int)direction.y);
        return neighborTile == null ? null : neighborTile.GetWall(-1 * direction);
    }
    public void ClearMaze()
    {
        while (transform.childCount > 0)
        {
            Transform child = transform.GetChild(0);
            DestroyImmediate(child.gameObject);
        }
        GameObject mazeFloorTilesParent = GameObject.Find("MazeTiles");
        while (mazeFloorTilesParent.transform.childCount > 0)
        {
            Transform child = mazeFloorTilesParent.transform.GetChild(0);
            DestroyImmediate(child.gameObject);
        }
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
        this.north = north.GetComponent<WallHelper>();
        this.east = east.GetComponent<WallHelper>();
        this.south = south.GetComponent<WallHelper>();
        this.west = west.GetComponent<WallHelper>();
    }
    public TileNode(GameObject tile, WallHelper north, WallHelper east, WallHelper south, WallHelper west)
    {
        this.tile = tile.GetComponent<TileHelper>();
        this.north = north;
        this.east = east;
        this.south = south;
        this.west = west;
    }
    public bool isConnected(Vector2 direction)
    {
        WallHelper wall = GetWall(direction);
        return wall == null? false : !wall.IsToggled();
    }
    
    public WallHelper GetWall(Vector2 direction)
    {
        if (direction.x != 0 && direction.y != 0 || direction.magnitude == 0)
        {
            Debug.LogError("Bad direction passed into GetWall");
            return null;
        }
        direction.Normalize();
        WallHelper wall = direction.x == 1 ? east : direction.x == -1 ? west : direction.y == 1 ? north : south;

        return wall;
    }

}
