using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public NavMeshAgent character;
    //public Animator characterAnimator;
    public GameObject targetDest;

    // Start is called before the first frame update
    void Start()
    {
        character.SetDestination(targetDest.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
