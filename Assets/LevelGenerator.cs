using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public Room[] rooms;
    public Room startRoom;
    public List<Door> closedDoors;
    // Start is called before the first frame update
    void Start()
    {
        Room startRoomInstance = Instantiate(startRoom, this.transform);
        foreach(Door door in startRoomInstance.doors)
        {
            closedDoors.Add(door);
        }
    }
}
