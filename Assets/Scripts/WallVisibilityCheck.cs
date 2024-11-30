using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallVisibilityCheck : MonoBehaviour
{
    [SerializeField]
    GameObject player;

    private LayerMask layerMask;

    // Start is called before the first frame update
    void Start()
    {
        layerMask = LayerMask.GetMask("Wall");
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
        
        Debug.DrawRay(transform.position, direction * 10000, Color.red);
        if (Physics.Raycast(transform.position, direction, out hit, layerMask)) 
        {
            //Material[] materials = hit.transform.gameObject.GetComponent<Renderer>().materials;

            if (true)
            {
                Collider[] hitColliders = Physics.OverlapSphere(hit.transform.position, 0.1f, layerMask);
                Debug.Log(hitColliders[0].transform.gameObject, hitColliders[1].transform.gameObject);
                foreach (var hitCollider in hitColliders)
                {
                    Material[] materials1 = hitCollider.transform.gameObject.GetComponent<Renderer>().materials;
                    materials1[0].color = new Color(materials1[0].color.r, materials1[0].color.g, materials1[0].color.b, 0.25f);
                   
                }


               
            }
            
            //Debug.DrawRay(transform.position, direction * hit.distance, Color.red);
        }
    }
}
