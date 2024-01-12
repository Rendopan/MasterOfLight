using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PNGController : MonoBehaviour
{
    public GameObject lanternObj;
    private Lantern pngLantern;

    private const float BALLOON_WIDTH = 10.0f;
    public GameObject balloonBtnObj;
    

    private float balloonScale = BALLOON_WIDTH;
    private RectTransform balloonRectTransform;
    private Vector3 balloonScaleVector;

    private float speed = 10.0f;

    // Start is called before the first frame update
    void Start()
    {
        pngLantern = lanternObj.GetComponent<Lantern>();
        balloonRectTransform = balloonBtnObj.GetComponent<RectTransform>();
        balloonScaleVector = balloonRectTransform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if(TaleManager.Tale.IsDialoguePossible())
        {
            if (balloonScale < BALLOON_WIDTH) 
            { 
                balloonScale += Time.deltaTime*speed;
                balloonScaleVector.x = balloonScale;
                balloonScaleVector.y = balloonScale;
                balloonRectTransform.localScale = balloonScaleVector;
            }
        }
        else
        {
            if (balloonScale > 0)
            {
                balloonScale -= Time.deltaTime*speed;
                balloonScaleVector.x = balloonScale;
                balloonScaleVector.y = balloonScale;
                balloonRectTransform.localScale = balloonScaleVector;
            }
        }
    }

    public void GiveLantern()
    {
        pngLantern.TakeLantern();
    }
}
