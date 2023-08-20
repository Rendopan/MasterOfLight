using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class POI : MonoBehaviour
{
    // POI defence variables ------------------------------------------------------------------------------
    public GameObject hitPointsBar;                     // Hit point game objcect reference.
    private int hitPoints;                              // Hit points POI value.
    private int startHitPoints;                         // Hit point starting value, useful to reset POI and to change hitpoint Bar UI.
    private Image hitPointsImg;                         // Image for hitPoints bar.

    private Camera cameraToLookAt;                      // hitPoint bar rotate towards camera.

    public List<GameObject> Flames;
    private int flameHitPoint;
    private int flameLost;

    public GameObject GameOverPnl;

    void Start()
    {
        startHitPoints = hitPoints = 500;
        cameraToLookAt = Camera.main;
        hitPointsImg = hitPointsBar.GetComponent<Image>();
        flameLost = 1;
        flameHitPoint = startHitPoints / Flames.Count;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 v = cameraToLookAt.transform.position - hitPointsBar.transform.position;
        v.x = v.z = 0.0f;
        hitPointsBar.transform.LookAt(cameraToLookAt.transform.position - v);
    }

    /// <summary>
    /// When a POI is damaged it lose its flames and it can't restore it.
    /// At the same time the area lose a little bit of light.
    /// When all the flames are off POI disappear and player lose the game.
    /// </summary>
    /// <param name="damage"></param>
    public void LoseHitPoints(int damage)
    {
        this.hitPoints -= damage;
        hitPointsImg.fillAmount = (float)hitPoints / startHitPoints;
        
        int damageLost = startHitPoints - hitPoints;
        if (damageLost > flameHitPoint * flameLost)
        {
            // Remove the first flame in list
            Flame flame = Flames[0].GetComponent<Flame>();
            flame.RemoveLight();
            Flames.RemoveAt(0);
            
            // Increment the flame lost.
            flameLost++;

        }
        if (Flames.Count == 0)
        {
            Debug.Log("It's all darkness");
            GameOverPnl.SetActive(true);
        }
        //if (hitPoints <= 0)
        //{
        //    Destroy(gameObject);
        //}
    }

    /// <summary>
    /// Each POI has an area. It illuminates that area.
    /// First Time Andrea enter in well Area, it start the dialogue.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Andrea")
        {
            if (TaleManager.Tale.CurrentStroke == 0)
            {
                TaleManager.Tale.SetStroke("I cannot remember but I was here before…");
                TaleManager.Tale.CurrentStroke++;
            }
        }
    }
}
