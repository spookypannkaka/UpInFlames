using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    private List<Tile> burningTiles;
    public float burnFrequenzy;
    [Range(0f, 1f)]public float burnChance;
    private float colddown;
    public static FireManager instance;
    // Start is called before the first frame update
    void Awake()
    {
        burningTiles = new List<Tile>();
        colddown = burnFrequenzy;
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        colddown -= Time.deltaTime;
        if(colddown < 0)
        {
            colddown = burnFrequenzy;
            List<Tile> newFires = new List<Tile>();
            foreach(Tile tile in burningTiles)
            {
                foreach (Tile neigbor in LevelGenerator.instance.GetNeigbors(tile))
                {
                    if (neigbor.flameable && Random.Range(0f, 1f) < burnChance)
                    {
                        neigbor.CatchFire();
                        newFires.Add(neigbor);
                    }
                }
            }
            foreach (Tile tile in newFires)
            {
                burningTiles.Add(tile);
            }
        }
    }

    public void StartFire(Tile tile)
    {
        tile.CatchFire();
        burningTiles.Add(tile);
    }
}
