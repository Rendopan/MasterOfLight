using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TaleManager : MonoBehaviour
{
    private int currentStroke = 0;

    public GameObject storyPanel;
    public GameObject faceCameraObj;

    public static TaleManager Tale;
    private TextMeshProUGUI strokeTxt;
    // Start is called before the first frame update
    void Start()
    {
        Tale = this;
        strokeTxt = storyPanel.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        { 
            Application.Quit();
        }
    }
    public int CurrentStroke
    { 
        get { return currentStroke; } 
        set { currentStroke = value; } 
    }

    public void SetStroke(string stroke)
    {
        strokeTxt.text = stroke;
        StartCoroutine(TimedMessage());
    }

    //public void StartTalk()
    //{
    //    StartCoroutine(TimedMessage());
    //}

    private IEnumerator TimedMessage()
    {
        storyPanel.SetActive(true);
        faceCameraObj.SetActive(true);
        yield return new WaitForSeconds(5);
        storyPanel.SetActive(false);
        faceCameraObj.SetActive(false);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
