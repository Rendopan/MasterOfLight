using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flame : MonoBehaviour
{
    private SpriteRenderer flameRenderer;

    private Camera cameraToLookAt;

    private float offTime = 3.0f;
    public float timePassed = 0;

    void Awake()
    {
        cameraToLookAt = Camera.main;
        flameRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if(!IsVisible)
        {
            timePassed += Time.deltaTime;
            if(timePassed > offTime)
            {
                IsVisible = true;
                timePassed = 0.0f;
            }
        }
        Vector3 v = cameraToLookAt.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(cameraToLookAt.transform.position - v);
    }

    public bool IsVisible
    {
        get { return flameRenderer.enabled;  }
        set { flameRenderer.enabled = value; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Andrea")
        {
            if (IsVisible)
            {
                AndreaController.Andrea.GetLight(this);
            }
        }
    }

}
