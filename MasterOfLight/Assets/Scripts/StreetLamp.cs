using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLamp : MonoBehaviour
{
    private int lightCost;

    // Start is called before the first frame update
    void Start()
    {
        
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
        }
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.gameObject.tag == "Andrea")
        {
            AndreaController.Andrea.InPlacement(false);
        }
    }


}
