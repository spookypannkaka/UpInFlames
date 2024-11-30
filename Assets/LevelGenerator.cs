using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Room[] roomPrefabs;
    public Room startRoom;
    public List<Door> closedDoors;
    private List<Room> existingRooms;
    public int targetRoomCount = 10;
    public int maxAttempts = 100;
    public GameObject outerWallPrefab;
    public GameObject innerWallPrefab;
    public static Vector2Int[] directions =
    {
        new Vector2Int(1, 0),
        new Vector2Int(0, -1),
        new Vector2Int(-1, 0),
        new Vector2Int(0, 1)

    };
    private Vector2Int lowerBonds;
    private Vector2Int upperBonds;
    public static float scale = 1f;
    // Start is called before the first frame update
    void Start()
    {
        lowerBonds = new Vector2Int(0, 0);
        upperBonds = new Vector2Int(0, 0);
        Room startRoomInstance = Instantiate(startRoom, this.transform);
        startRoomInstance.InitiateRoom();
        startRoomInstance.FindGlobalPos();
        foreach (Door door in startRoomInstance.doors)
        {
            closedDoors.Add(door);
        }
        existingRooms = new List<Room>();
        existingRooms.Add(startRoomInstance);
        TryGenerateRoom();
        GenerateMap();
        BuildSolidWalls();
    }

    private void GenerateMap()
    {
        int currentAttempt = 0;

        while(currentAttempt < maxAttempts && existingRooms.Count < targetRoomCount)
        {
            currentAttempt++;
            TryGenerateRoom();
        }
    }
    public void TryGenerateRoom()
    {
        if(closedDoors.Count == 0)
        {
            return;
        }
        int doorNumber = Random.Range(0, closedDoors.Count-1);
        int roomPrefabNumber = Random.Range(0, roomPrefabs.Length);
        Room newRoom = Instantiate(startRoom, this.transform);
        newRoom.InitiateRoom();
        int targetDoorNumber = Random.Range(0, newRoom.doors.Count);

        //How many 90 degrees clockwise should we rotate the new room? I hope this formula works.
        int targetRotation = ((int)closedDoors[doorNumber].direction - (int)newRoom.doors[targetDoorNumber].direction + 2) % 4;
        newRoom.RotateRoom(targetRotation);
        Debug.Log(doorNumber + "/" + closedDoors.Count + ", " + (int)closedDoors[doorNumber].direction);
        Vector3 targetPos = closedDoors[doorNumber].transform.position + new Vector3(
            directions[(int)closedDoors[doorNumber].direction].x * scale,
            0,
            directions[(int)closedDoors[doorNumber].direction].y * scale
            );
        Vector3 currentPos = newRoom.doors[targetDoorNumber].tile.transform.position;
        newRoom.transform.position = targetPos - currentPos;
        newRoom.FindGlobalPos();
        if (!CheckValidPos(newRoom))
        {
            //That was not a vaild position! Discard the room and stop.
            Destroy(newRoom.gameObject);
            return;
        }

        newRoom.doors[targetDoorNumber].Open();
        closedDoors[doorNumber].Open();
        closedDoors.RemoveAt(doorNumber);
        foreach(Door newDoor in newRoom.doors)
        {
            if(newDoor != newRoom.doors[targetDoorNumber])
            {
                closedDoors.Add(newDoor);
            }
        }
        existingRooms.Add(newRoom);

    }
    public bool CheckValidPos(Room newRoom)
    {
        foreach(Tile tile in newRoom.tiles)
        {
            foreach(Room room in existingRooms)
            {
                foreach(Tile otherTile in room.tiles)
                {
                    if(tile.globalPos == otherTile.globalPos)
                    {
                        return false;
                    }
                }
            }
            if(tile.globalPos.x < lowerBonds.x)
            {
                lowerBonds.x = tile.globalPos.x;
            }
            if (tile.globalPos.y < lowerBonds.y)
            {
                lowerBonds.y = tile.globalPos.y;
            }
            if (tile.globalPos.x > upperBonds.x)
            {
                upperBonds.x = tile.globalPos.x;
            }
            if (tile.globalPos.y > upperBonds.y)
            {
                upperBonds.y = tile.globalPos.y;
            }
        }
        return true;
    }
    private void BuildSolidWalls()
    {
        for(int x = lowerBonds.x-1; x <= upperBonds.x+1; x++)
        {
            for(int y = lowerBonds.y-1; y <= upperBonds.y+1; y++)
            {
                bool validPos = true;
                Tile[] neigbors = new Tile[4];
                Vector2Int thisPos = new Vector2Int(x, y);
                foreach (Room room in existingRooms)
                {
                    foreach (Tile otherTile in room.tiles)
                    {
                        if (otherTile.globalPos == thisPos)
                        {
                            validPos = false;
                            break;
                        }
                        for(int i = 0; i < 4; i++)
                        {
                            if (otherTile.globalPos == thisPos + directions[i])
                            {
                                neigbors[i] = otherTile;
                            }
                        }
                    }
                    if (!validPos)
                    {
                        break;
                    }
                }
                if (!validPos)
                {
                    continue;
                }
                Tile newWall = Instantiate(outerWallPrefab, transform).GetComponent<Tile>();
                newWall.globalPos = thisPos;
                newWall.transform.position = new Vector3(thisPos.x * scale, 0, thisPos.y * scale);
                newWall.neigbors = neigbors;
            }
        }
    }
}
