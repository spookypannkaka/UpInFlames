using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Door> doors;
    public Tile[] tiles;
    public float scale = 1f;
    // Start is called before the first frame update
    void Start()
    {
        doors = new List<Door>();
        InitiateRoom();
    }
    public void InitiateRoom()
    {
        tiles = GetComponentsInChildren<Tile>();
        foreach (Tile tile in tiles)
        {
            tile.InitTile();
            tile.roomPos = new Vector2Int((int)Mathf.Round(tile.transform.position.x / scale), (int)Mathf.Round(tile.transform.position.z / scale));
            foreach(Door door in tile.doors)
            {
                doors.Add(door);
            }
        }

        // Calculate neighbors
        foreach (Tile tile in tiles)
        {
            tile.neigbors = new Tile[4];
            foreach (Tile otherTile in tiles)
            {
                if(otherTile.roomPos.x == tile.roomPos.x + 1 && otherTile.roomPos.y == tile.roomPos.y)
                {
                    // Northen neighbor
                    if (tile.neigbors[0] != null)
                    {
                        Debug.LogError("Wierdness found when calculating north neighbors!", this);
                    }
                    tile.neigbors[0] = otherTile;
                }
                if (otherTile.roomPos.x == tile.roomPos.x && otherTile.roomPos.y == tile.roomPos.y - 1)
                {
                    // Eastern neighbor
                    if (tile.neigbors[1] != null)
                    {
                        Debug.LogError("Wierdness found when calculating east neighbors!", this);
                    }
                    tile.neigbors[1] = otherTile;
                }
                if (otherTile.roomPos.x == tile.roomPos.x - 1 && otherTile.roomPos.y == tile.roomPos.y)
                {
                    // Southern neighbor
                    if (tile.neigbors[2] != null)
                    {
                        Debug.LogError("Wierdness found when calculating south neighbors!", this);
                    }
                    tile.neigbors[2] = otherTile;
                }
                if (otherTile.roomPos.x == tile.roomPos.x && otherTile.roomPos.y == tile.roomPos.y + 1)
                {
                    // Western neighbor
                    if (tile.neigbors[3] != null)
                    {
                        Debug.LogError("Wierdness found when calculating west neighbors!", this);
                    }
                    tile.neigbors[3] = otherTile;
                }
            }
        }
    }
}
