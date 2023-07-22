using System;
using UnityEngine;
using UnityEngine.AI;

public class AndreaController : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent character;
    public Animator characterAnimator;
    public GameObject targetDest;

    public GameObject[] streetLampPrefabs;
    private GameObject streetLampToPlace;
    private StreetLamp streetLamp;

    public GameObject canvasPlacement;
    private int layerGround = 1 << 7;

    public static AndreaController Andrea;
    private Lantern lantern;


    private bool inDragging = false;
    private bool inPlacement = false;

    public bool HasLantern { get; internal set; }

    public void Start()
    {
        Andrea = this;
    }

    // Update is called once per frame
    void Update()
    {
        if (inDragging)
        {
            streetLampToPlace.transform.position = Input.mousePosition;
            
            if (lantern.LightScore <= streetLamp.LightCost)
                CancelLampPlacement();
        }
        else if(inPlacement) 
        {
            if (lantern.LightScore <= streetLamp.LightCost)
                CancelLampPlacement();
        }
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitPoint;

            if (Physics.Raycast(ray, out hitPoint, 1000f, layerGround))
            {
                targetDest.transform.position = hitPoint.point;
                if(inDragging) 
                {
                    inDragging = false;
                    inPlacement = true;
                    streetLampToPlace.transform.position = hitPoint.point;
                    canvasPlacement.transform.position = new Vector3(streetLampToPlace.transform.position.x, 3.0f, streetLampToPlace.transform.position.z);
                    canvasPlacement.SetActive(true);
                }
                else
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

    public void Generate_StreetLamp(int idLamp)
    {
        inDragging = true;
        switch (idLamp)
        {
            case 0:
                streetLampToPlace = Instantiate(streetLampPrefabs[idLamp], Input.mousePosition, streetLampPrefabs[idLamp].transform.rotation);
                streetLamp = streetLampToPlace.GetComponent<StreetLamp>();
                streetLamp.LightCost = 80;
                break;
            default:
                break;
        }
    }

    public void ConfirmPlacement()
    {
        inPlacement = false;
        canvasPlacement.SetActive(false);
    }

    public void CancelLampPlacement()
    {
        inPlacement = false;
        inDragging = false;
        canvasPlacement.SetActive(false);
        Destroy(streetLampToPlace);
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
