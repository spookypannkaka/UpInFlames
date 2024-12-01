using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public List<Door> doors;
    public Tile[] tiles;
    // Start is called before the first frame update
    public void InitiateRoom()
    {
        doors = new List<Door>();
        tiles = GetComponentsInChildren<Tile>();
        foreach (Tile tile in tiles)
        {
            tile.InitTile();
            tile.roomPos = new Vector2Int((int)Mathf.Round(tile.transform.position.x / LevelGenerator.scale), (int)Mathf.Round(tile.transform.position.z / LevelGenerator.scale));
            foreach(Door door in tile.doors)
            {
                doors.Add(door);
            }
        }
    }

    public void RotateRoom(int rotation)
    {
        transform.Rotate(0, 90 * rotation, 0);
        foreach(Door door in doors)
        {
            door.direction = (Door.Direction)(((int)door.direction + rotation + 4) % 4);
        }
    }

    public void FindGlobalPos()
    {
        foreach(Tile tile in tiles)
        {
            tile.FindGlobalPos();
        }
    }
}
