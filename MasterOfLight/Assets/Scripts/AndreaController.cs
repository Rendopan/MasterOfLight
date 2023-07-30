using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class AndreaController : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent character;
    public Animator characterAnimator;
    public GameObject targetDest;

    //public GameObject storyPanel;
    //public GameObject faceCameraObj;

    public GameObject[] streetLampPrefabs;
    private GameObject streetLampToPlace;
    private StreetLamp streetLamp;

    public GameObject canvasPlacement;
    private int layerGround = 1 << 7;

    public static AndreaController Andrea;
    private Lantern lantern;


    private bool inDragging = false;
    //private bool inPlacement = false;

    public bool HasLantern { get; internal set; }

    public bool IsPlacing {  get; set; }

    public void Start()
    {
        Andrea = this;
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitPoint;
        bool isOverUI = UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject();

        if (Physics.Raycast(ray, out hitPoint, 1000f, layerGround) && !isOverUI)
        {
            if (hitPoint.collider.gameObject.tag == "Ground")
            {
                targetDest.transform.position = hitPoint.point;
                if (Input.GetMouseButtonDown(0) && !inDragging)
                {
                    character.SetDestination(hitPoint.point);
                }
                if (Input.GetMouseButtonDown(0) && inDragging)
                {
                    inDragging = false;
                    IsPlacing = true;
                    streetLampToPlace.transform.position = hitPoint.point;
                    character.SetDestination(hitPoint.point);
                    //canvasPlacement.transform.position = new Vector3(streetLampToPlace.transform.position.x, 3.0f, streetLampToPlace.transform.position.z);
                    //canvasPlacement.SetActive(true);
                }
                else if (inDragging)
                {
                    streetLampToPlace.transform.position = hitPoint.point;
                    if (lantern.LightScore <= streetLamp.LightCost)
                        CancelLampPlacement();
                }
                else if (IsPlacing)
                {
                    if (lantern.LightScore <= streetLamp.LightCost)
                        CancelLampPlacement();
                }
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

    
    //public void StartTalk()
    //{
    //    StartCoroutine(TimedMessage());
    //}

    //private IEnumerator TimedMessage() 
    //{
    //    storyPanel.SetActive(true);
    //    faceCameraObj.SetActive(true);
    //    yield return new WaitForSeconds(5);
    //    storyPanel.SetActive(false);
    //    faceCameraObj.SetActive(false);
    //}
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
                streetLamp = streetLampToPlace.GetComponentInChildren<StreetLamp>();
                streetLamp.LightCost = 80;
                Camera.main.orthographicSize = 10;
                break;
            default:
                break;
        }
    }

    public void ConfirmPlacement()
    {
        IsPlacing = false;
        inDragging = false;

        // Subtract light from lantern.
        lantern.AddLightScore(-streetLamp.LightCost);

        // Andrea tought
        TaleManager.Tale.SetStroke("Oh well, I remeber this was there…");

        // Start first enemy wave when first streetLamp is placed.
        if (EnemiesWavesManager.enemiesWave.CurrentWave == 0)
            EnemiesWavesManager.enemiesWave.StartWave(1);

        // Start first enemy wave when first streetLamp is placed.
        EnemiesWavesManager.enemiesWave.AddTargetPoint(streetLampToPlace.transform);

        // Hide UI to edit lantern.
        canvasPlacement.SetActive(false);

        // Zoom out camera.
        Camera.main.orthographicSize = 5;
    }

    public void RotateLamp()
    {
        streetLamp.RotateLamp();
    }

    public void CancelLampPlacement()
    {
        IsPlacing = false;
        inDragging = false;
        canvasPlacement.SetActive(false);
        Destroy(streetLampToPlace);
        Camera.main.orthographicSize = 5;
    }

    public void InPlacement(bool isPlacing)
    {
        if(IsPlacing)
        {
            canvasPlacement.SetActive(isPlacing);
        }
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
