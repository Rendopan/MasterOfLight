using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent character;
    public GameObject hitPointsBar;
    public GameObject darkSphere;
    public GameObject startSpawnShot;

    private int damage;
    private int hitPoints;
    private int startHitPoints;
    private Image hitPointsImg;

    private float shotFrequency = 3.0f;
    private float rechargeTime = 0.0f;

    private Camera cameraToLookAt;

    //public Animator characterAnimator;
    //public List<Transform> targetDest;

    private GameObject currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        hitPoints = 10;
        damage = 5;
        rechargeTime = shotFrequency;
        startHitPoints = hitPoints;
        cameraToLookAt = Camera.main;
        hitPointsImg = hitPointsBar.GetComponent<Image>();
        //currentTarget = this.transform;
        //character.SetDestination(targetDest.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = cameraToLookAt.transform.position - hitPointsBar.transform.position;
        v.x = v.z = 0.0f;
        hitPointsBar.transform.LookAt(cameraToLookAt.transform.position - v);
    }

    public void StartTarget(GameObject startTarget)
    {
        currentTarget = startTarget;
    }


    public void ChooseNewTarget(Transform newTarget)
    {
        float newDist = Vector3.Distance(transform.position, newTarget.position);
        
        float currentDist = Vector3.Distance(transform.position, currentTarget.transform.position);

        if(newDist < currentDist) 
        {
            currentTarget = newTarget.gameObject;
        }

        character.SetDestination(currentTarget.transform.position);
    }

    public void LoseHitPoints(int damage)
    {
        this.hitPoints -= damage;
        hitPointsImg.fillAmount = (float)hitPoints / startHitPoints;
        if (hitPoints <= 0) 
        {
            EnemiesWavesManager.enemiesWave.RemoveEnemyInWave(this);
            Destroy(gameObject);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        // Shot a dark sphere.
        if (other.gameObject.tag == "Light")
        {
            if (currentTarget == null)
            {
                currentTarget = other.gameObject;
                ShotDarkSphere();
            }
            if (currentTarget.gameObject.Equals(other.gameObject))
            {
                rechargeTime += Time.deltaTime;
                if (rechargeTime > shotFrequency)
                {
                    rechargeTime = 0.0f;
                    ShotDarkSphere();
                }
            }
        }
    }

    //private void OnCollisionStay(Collider other)
    //{
    //    if (other.gameObject.tag == "StreetLamp")
    //    {
    //        Debug.Log("shot a dark sphere");
    //        if (currentTarget.gameObject.Equals(other.gameObject))
    //        {
    //            rechargeTime += Time.deltaTime;
    //            if (rechargeTime > shotFrequency)
    //            {
    //                rechargeTime = 0.0f;
    //                ShotDarkSphere();
    //            }
    //        }
    //    }
    //}

    private void ShotDarkSphere()
    {
        // A Dark Sphere is instatiated.
        GameObject shotObj = Instantiate(darkSphere, startSpawnShot.transform, false);

        DarkShot darkShot = shotObj.GetComponent<DarkShot>();
        Vector3 shootDir = (currentTarget.transform.position - shotObj.transform.position).normalized;
        darkShot.Setup(shootDir, damage);
    }
}
