using System;
using UnityEngine;
using UnityEngine.AI;

public class AndreaController : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent character;
    public Animator characterAnimator;
    public GameObject targetDest;

    private int layerGround = 1 << 7;

    public static AndreaController Andrea;
    private Lantern lantern;

    public bool HasLantern { get; internal set; }

    public void Start()
    {
        Andrea = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitPoint;

            if (Physics.Raycast(ray, out hitPoint, 1000f, layerGround))
            {
                targetDest.transform.position = hitPoint.point;
                character.SetDestination(hitPoint.point);
            }
        }

        if (character.velocity != Vector3.zero)
        {
            characterAnimator.SetBool("isWalking", true);
        }
        else if (character.velocity == Vector3.zero)
        {
            characterAnimator.SetBool("isWalking", false);
        }
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("")
    //    {
    //    }
    //}

    public void GetLight(Flame light)
    {
        if(HasLantern) 
        {
            light.IsVisible = false;
            lantern.AddLightScore(10);
        }
    }

    public void GetLantern(Lantern lanternObj)
    {
        //lanternObj.gameObject.SetActive(false);
        lantern = lanternObj;
        HasLantern = true;
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.CompareTag("Flame"))
    //    {
    //        Debug.Log("Ciao, fiamma.");
    //        //AndreaController.Andrea.GetLight(this.gameObject);
    //    }
    //    Debug.Log("Ciao, fiamma.");
    //}
}
