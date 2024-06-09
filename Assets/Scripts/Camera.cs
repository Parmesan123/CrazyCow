using Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    void Start()
    {
        Transform target = Object.FindObjectOfType<PlayerMovement>().transform;
        
        GetComponent<UnityEngine.Camera>().gameObject.transform.SetParent(target);
    }
}
