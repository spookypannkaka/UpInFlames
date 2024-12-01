using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int roomPos;
    public Vector2Int globalPos;
    public Door[] doors;
    public bool flameable = true;
    public GameObject fire;
    // Start is called before the first frame update
    public void InitTile()
    {
        doors = GetComponentsInChildren<Door>();
        foreach(Door door in doors)
        {
            door.tile = this;
        }
    }
    public void FindGlobalPos()
    {
        globalPos = new Vector2Int(
            (int)Mathf.Round(transform.position.x / LevelGenerator.scale),
            (int)Mathf.Round(transform.position.z / LevelGenerator.scale)
            );
    }
    public void CatchFire()
    {
        flameable = false;
        Instantiate(fire, this.transform);
    }
}
