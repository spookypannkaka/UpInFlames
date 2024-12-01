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
    public GameObject upStairs;
    private List<Tile> wallTiles;
    private Tile[][] tiles;
    public static LevelGenerator instance;
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
        instance = this;
        lowerBonds = new Vector2Int(0, 0);
        upperBonds = new Vector2Int(0, 0);
        wallTiles = new List<Tile>();
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
        CalcNeigbors();
        FireManager.instance.StartFire(GetTileFromGlobalPos(new Vector2Int(0, 0)));
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
                wallTiles.Add(newWall);
            }
        }
    }
    private void CalcNeigbors()
    {
        tiles = new Tile[upperBonds.x + 3 - lowerBonds.x][];
        for(int i = 0; i < tiles.Length; i++)
        {
            tiles[i] = new Tile[upperBonds.y + 3 - lowerBonds.y];
        }
        foreach (Room room in existingRooms)
        {
            foreach (Tile targetTile in room.tiles)
            {
                Vector2Int vectorPos = GlobalPosToVectorPos(targetTile.globalPos);
                tiles[vectorPos.x][vectorPos.y] = targetTile;
            }
        }
        //Debug.Log("Sorting tiles: " + lowerBonds.x + ", " + lowerBonds.y + ": " + upperBonds.x + ", " + upperBonds.y);
        foreach (Tile targetTile in wallTiles)
        {
            Vector2Int vectorPos = GlobalPosToVectorPos(targetTile.globalPos);
            tiles[vectorPos.x][vectorPos.y] = targetTile;
        }
        /*for (int i = 0; i < tiles.Length; i++)
        {
            for (int j = 0; j < tiles[i].Length; j++)
            {
                Debug.Log(tiles[i][j].globalPos);
            }
        }*/
    }
    private Vector2Int GlobalPosToVectorPos(Vector2Int input)
    {
        return new Vector2Int(input.x - lowerBonds.x + 1, input.y - lowerBonds.y + 1);
    }
    public Tile GetTileFromGlobalPos(Vector2Int pos)
    {
        Vector2Int targetPos = GlobalPosToVectorPos(pos);
        if(targetPos.x < 0 || targetPos.y < 0 || targetPos.x >= tiles.Length || targetPos.y >= tiles[0].Length)
        {
            return null;
        }
        return tiles[targetPos.x][targetPos.y];
    }
    public List<Tile> GetNeigbors(Tile tile)
    {
        List<Tile> neighbors = new List<Tile>();
        foreach(Vector2Int direction in directions)
        {
            if(GetTileFromGlobalPos(direction + tile.globalPos) != null)
            {
                neighbors.Add(GetTileFromGlobalPos(direction + tile.globalPos));
            }
        }
        return neighbors;
    }
}
