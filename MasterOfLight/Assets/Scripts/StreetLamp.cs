using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLamp : MonoBehaviour
{
    private int lightCost;
    public GameObject LightSphere;
    public GameObject startShootObject;

    private Vector3 startShootPosition;
    private GameObject currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        startShootPosition = startShootObject.transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RotateLamp() 
    {
        transform.Rotate(Vector3.up, 90.0f);
    }

    public int LightCost
    {
        get { return lightCost; } 
        set {  lightCost = value; } 
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Andrea")
        {
            AndreaController.Andrea.InPlacement(true);
            //currentTarget = other.gameObject;
            //InvokeRepeating("ShotLightSphere", 0.0f, 2f);
        }
        else if (other.gameObject.tag == "Enemy")
        {
            if (currentTarget == null)
            {
                currentTarget = other.gameObject;
                InvokeRepeating("ShotLightSphere", 0.0f, 2f);
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //GameObject shotObj = Instantiate(LightSphere, startShootPosition, Quaternion.identity, this.transform);
            //LightShot lightShot = shotObj.GetComponent<LightShot>();
            //Vector3 shootDir = (other.gameObject.transform.position - startShootPosition).normalized;
            //lightShot.Setup(shootDir);
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Andrea")
        {
            AndreaController.Andrea.InPlacement(false);
        }
    }

    private void ShotLightSphere()
    {
        GameObject shotObj = Instantiate(LightSphere, startShootObject.transform );

        LightShot lightShot = shotObj.GetComponent<LightShot>();
        Vector3 shootDir = (currentTarget.transform.position - startShootPosition).normalized;
        lightShot.Setup(shootDir);
    }

}
