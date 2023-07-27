using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent character;
    private int damage;
    private int hitPoints;

    //public Animator characterAnimator;
    //public List<Transform> targetDest;

    private Transform currentTarget;

    // Start is called before the first frame update
    void Start()
    {
        hitPoints = 10;
        //currentTarget = this.transform;
        //character.SetDestination(targetDest.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //public void AddTarget(Transform targetObject)
    //{
    //    targetDest.Add(targetObject);
    //}
    public void StartTarget(Transform startTarget)
    {
        currentTarget = startTarget;
    }


    public void ChooseNewTarget(Transform newTarget)
    {

        float newDist = Vector3.Distance(transform.position, newTarget.position);
        float currentDist = Vector3.Distance(transform.position, currentTarget.position);

        if(newDist < currentDist) 
        {
            currentTarget = newTarget;
        }

        character.SetDestination(currentTarget.position);
    }

    public void LoseHitPoints(int damage)
    {
        this.hitPoints -= damage;
        if(hitPoints <= 0) 
        { 
            Destroy(gameObject);
        }
    }
}
