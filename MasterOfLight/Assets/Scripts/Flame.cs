using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Every POI has a different number of flames. 
/// Flame appear and disappear with a certain frequency.
/// When Andrea take a flame, with a lantern in hand, flame disappear and light goes to load the lantern.
/// Each flame is linked with a light of the area.
/// When a flame is off by an enemy it is lost and the linked light also disappear.
/// If all the flame of a POI are off game is over.
/// </summary>
public class Flame : MonoBehaviour
{
    public GameObject pointLight;                       // The pointlight linked to the flame that illuminate area.

    private SpriteRenderer flameRenderer;               // Sprite renderer to change flame visibility
    private float offTime = 3.0f;                       // Time a flame is off (sec).
    private float timePassed = 0;                       // Time passed from on / off.

    private Camera cameraToLookAt;                      // flame rotate towards camera.

    void Awake()
    {
        cameraToLookAt = Camera.main;
        flameRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        // Visibility frequency
        if(!IsVisible)
        {
            timePassed += Time.deltaTime;
            if(timePassed > OffTime)
            {
                IsVisible = true;
                timePassed = 0.0f;
            }
        }

        // Flame sprite directs towards camera.
        Vector3 v = cameraToLookAt.transform.position - transform.position;
        v.x = v.z = 0.0f;
        transform.LookAt(cameraToLookAt.transform.position - v);
    }

    /// <summary>
    /// Is flame visible?
    /// </summary>
    public bool IsVisible
    {
        get { return flameRenderer.enabled;  }
        set { flameRenderer.enabled = value; }
    }

    /// <summary>
    /// Off time of a flame. Can differ from POI.
    /// </summary>
    public float OffTime
    { 
        get { return offTime; } 
        set { offTime = value; }
    }

    /// <summary>
    /// Remove flame and its light.
    /// </summary>
    public void RemoveLight()
    {
        Destroy(pointLight);
        Destroy(this.gameObject);
    }

    /// <summary>
    /// If Andrea enter in flame area she collect light for the lantern.
    /// </summary>
    /// <param name="other"></param>
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
