using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightShot : MonoBehaviour
{
    private Vector3 shootDir;
    private float speed = 2f;
    public void Setup(Vector3 dir) 
    {
        shootDir = dir;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += shootDir * speed * Time.deltaTime;
        if(transform.position.y < 0) 
        { 
            Destroy(gameObject);
        }
    }
}
