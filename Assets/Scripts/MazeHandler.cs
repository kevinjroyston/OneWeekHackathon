using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void BuildMaze(int width = -1, int height = -1)
    {
        width = width == -1 ? this.width : width;
        height = height == -1 ? this.height : height;

        maze = new TileNode[width, height];

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
                GameObject tile = (GameObject)PrefabUtility.InstantiatePrefab(wallPrefab);
                tile.transform.localScale = new Vector3(wallLength, .4f, wallLength);
                tile.transform.SetParent(transform);
                tile.transform.localPosition = currentlocation + new Vector3(0, -.2f, 0);

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

                if (south == null)
                    Debug.LogWarning("oops");


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
        foreach (Transform child in transform)
        {
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
        return wall == null? false : !wall.getIsActive();
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
