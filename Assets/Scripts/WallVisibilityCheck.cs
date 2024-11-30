using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallVisibilityCheck : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SendRayCheck();

    }

    void SendRayCheck()
    {
        RaycastHit hit;
        Vector3 direction = player.transform.position - transform.position;
        LayerMask layerMask = LayerMask.GetMask("Wall");

        if (Physics.Raycast(transform.position, direction, out hit, layerMask)) 
        {
            Material[] materials = hit.transform.gameObject.GetComponent<Renderer>().materials;
            if (materials[0].color.a != 0.25f)
            {
                materials[0].SetColor("_Color", new Color(materials[0].color.r, materials[0].color.g, materials[0].color.b, 0.25f));
            }
            
            Debug.DrawRay(transform.position, direction * hit.distance, Color.red);
        }
    }
}
