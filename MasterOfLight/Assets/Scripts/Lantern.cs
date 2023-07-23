using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.Impl;

public class Lantern : MonoBehaviour
{
    public GameObject lanternUI;
    private Image filledLevel;
    public GameObject usableLantern; 
    public GameObject streetLampUI;

    public GameObject lightObj;
    public TextMeshProUGUI lightScoreTxt;
    public Transform lanternParentTransform;

    private int currentLightScore;
    private Light lanternLight;
    private int layerLantern = 1 << 6;
    //private int currentLight;
    private int maxLight = 100;

    private float flashTime = 3.0f;
    private float decreaseTime = 3.0f;
    private float currentTime = 0.0f;

    private bool inOn = false;

    // Start is called before the first frame update
    void Start()
    {
        lanternLight = lightObj.GetComponent<Light>();
        filledLevel = lanternUI.GetComponent<Image>();
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
                    filledLevel.fillAmount = (float)currentLightScore / maxLight;
                }
            }
            if(AndreaController.Andrea.IsPlacing)
            {
                streetLampUI.SetActive(false);
            }
            else if (currentLightScore > 80)
            {
                streetLampUI.SetActive(true);
                usableLantern.SetActive(true);
            }
            else 
            {
                streetLampUI.SetActive(false);
                usableLantern.SetActive(false);
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

        if(currentLightScore > maxLight)
            currentLightScore = maxLight;

        lanternLight.intensity = currentLightScore / 100f;
        lightScoreTxt.text = currentLightScore.ToString();
        filledLevel.fillAmount = (float)currentLightScore / maxLight;

        if (currentLightScore > 80)
        {
            usableLantern.SetActive(true);
            streetLampUI.SetActive(true);
        }
        
    }

    public int LightScore
    { get { return currentLightScore; } }

}
