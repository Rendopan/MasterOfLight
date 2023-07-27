using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StreetLamp : MonoBehaviour
{
    private int lightCost;
    private int damage;
    private int hitPoints;
    private float shotFrequency = 3.0f;
    private float rechargeTime = 0.0f;

    public GameObject LightSphere;
    public GameObject startShootObject;

    private Vector3 startShootPosition;
    private GameObject currentTarget;


    // Start is called before the first frame update
    void Start()
    {
        damage = 1;
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
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            if (currentTarget == null)
            {
                if (TaleManager.Tale.CurrentStroke == 4)
                {
                    TaleManager.Tale.SetStroke("And who are you now?");
                    TaleManager.Tale.CurrentStroke++;
                }
                currentTarget = other.gameObject;
                ShotLightSphere();
            }
            else if (currentTarget.Equals(other.gameObject))
            {
                rechargeTime += Time.deltaTime;
                if (rechargeTime > shotFrequency)
                {
                    rechargeTime = 0.0f;
                    ShotLightSphere();
                }
            }
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
        GameObject shotObj = Instantiate(LightSphere, startShootObject.transform, false );

        LightShot lightShot = shotObj.GetComponent<LightShot>();
        Vector3 shootDir = (currentTarget.transform.position - shotObj.transform.position).normalized;
        lightShot.Setup(shootDir, damage);
    }

}
