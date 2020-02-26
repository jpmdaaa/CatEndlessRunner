using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    private Transform player;
    private Vector3 offSet;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        offSet = transform.position - player.position;
    }

   
    void LateUpdate()
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y,player.position.z+ offSet.z);
        transform.position = newPosition;
    }
}
