using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class Lantern : MonoBehaviour
{
    public GameObject lanternUI;
    public GameObject streetLampUI;

    public GameObject lightObj;
    public TextMeshProUGUI lightScoreTxt;
    public Transform lanternParentTransform;

    private int currentLightScore;
    private Light lanternLight;
    private int layerLantern = 1 << 6;
    //private int currentLight;
    //private int maxLight;

    private float flashTime = 3.0f;
    private float decreaseTime = 3.0f;
    private float currentTime = 0.0f;

    private bool inOn = false;

    // Start is called before the first frame update
    void Start()
    {
        lanternLight = lightObj.GetComponent<Light>();
        lanternLight.intensity = 0.0f;
        inOn = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (AndreaController.Andrea.HasLantern)
        {
            if (currentLightScore > 0)
            {
                currentTime += Time.deltaTime;
                if (currentTime > decreaseTime)
                {
                    currentTime = 0f;
                    currentLightScore--;
                    lanternLight.intensity = currentLightScore / 100f;
                    lightScoreTxt.text = currentLightScore.ToString();
                    
                }
            }
            if(AndreaController.Andrea.IsPlacing)
            {
                streetLampUI.SetActive(false);
            }
            else if (currentLightScore > 80)
            {
                streetLampUI.SetActive(true);
            }
            else 
            {
                streetLampUI.SetActive(false);
            }
        }
        else
        {
            if (inOn)
            {
                lanternLight.intensity += Time.deltaTime;
                if (lanternLight.intensity > flashTime)
                    inOn = false;
            }
            else
            {
                lanternLight.intensity -= Time.deltaTime;
                if (lanternLight.intensity <= 0.0f)
                    inOn = true;
            }
        }

        

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, 1000f, layerLantern))
            {
                if (hit.transform != null)
                {
                    lanternUI.SetActive(true);
                    AndreaController.Andrea.GetLantern(this);
                    this.gameObject.transform.parent = lanternParentTransform;
                    this.gameObject.transform.localPosition = Vector3.zero;
                    //Object.Destroy(this.gameObject);
                }
            }
        }
    }

    /// <summary>
    /// Add light to the lantern.
    /// </summary>
    /// <param name="s"></param>
    public void AddLightScore(int s)
    {
        currentLightScore += s;
        lanternLight.intensity = currentLightScore / 100f;
        lightScoreTxt.text = currentLightScore.ToString();

        if(currentLightScore > 80)
        {
            streetLampUI.SetActive(true);
        }
    }

    public int LightScore
    { get { return currentLightScore; } }

}
