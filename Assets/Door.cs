using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject open;
    public GameObject closed;
    public Tile tile;
    public enum Direction
    {
        north = 0,
        east = 1,
        south = 2,
        west = 3
    }
    public Direction direction;
}
