using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLock : MonoBehaviour
{

    public float x;
    public float y;
    public float z;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         transform.rotation = Quaternion.Euler(x, y, z);
    }
}
