using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StreetLamp : MonoBehaviour
{
    private int lightCost;
    private int damage;
    private float shotFrequency = 3.0f;
    private float rechargeTime = 0.0f;

    public GameObject lightSphere;
    public GameObject startSpawnShot;

    public GameObject hitPointsBar;
    private int hitPoints;
    private int startHitPoints;
    private Image hitPointsImg;

    private Vector3 startShootPosition;
    private GameObject currentTarget;

    private Camera cameraToLookAt;


    // Start is called before the first frame update
    void Start()
    {
        startHitPoints = hitPoints = 100;
        damage = 1;
        cameraToLookAt = Camera.main;
        hitPointsImg = hitPointsBar.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = cameraToLookAt.transform.position - hitPointsBar.transform.position;
        v.x = v.z = 0.0f;
        hitPointsBar.transform.LookAt(cameraToLookAt.transform.position - v);
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

    public void LoseHitPoints(int damage)
    {
        this.hitPoints -= damage;
        hitPointsImg.fillAmount = (float)hitPoints / startHitPoints;
        if (hitPoints <= 0)
        {
            Destroy(gameObject);
        }
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
        if (other.gameObject.tag == "Andrea")
        {
            AndreaController.Andrea.InPlacement(true);
        }
        else if (other.gameObject.tag == "Enemy" && other is CapsuleCollider)
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
        GameObject shotObj = Instantiate(lightSphere, startSpawnShot.transform, false);

        LightShot lightShot = shotObj.GetComponent<LightShot>();
        Vector3 shootDir = (currentTarget.transform.position - shotObj.transform.position).normalized;
        lightShot.Setup(shootDir, damage);
    }

}
