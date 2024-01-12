using GLTFast.Schema;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TaleManager : MonoBehaviour
{
    const float SLOW = .2f;
    const float NORMAL = .1f;
    const float RAPID = .05f;
    const float FULLSTOP = .2f;
    const float COMMA = .1f;
    const float SHOWLINETIME = 3.0f;
    //const float DIALOGUELINE = .5f;

    public static TaleManager Tale;
    public GameObject faceCameraObj;

    private XMLSceneLoader sceneLoader;

    private int currentSceneIndex = 0;

    public PNGController pngController;

    // For thoughts lines.
    public GameObject thoughtsPanel;
    private TextMeshProUGUI thoughtTxt;
    private int currentThoughtIndex = 0;

    private float thoughtFrequency = 4.0f;
    private float thoughtTime = 0.0f;

    private bool isThoughtsActive = false;

    public GameObject responsePanel;
    public List<Button> responsesBtn;
    TextMeshProUGUI responseTxt1, responseTxt2;

    // For dialogues lines.
    public GameObject dialoguePanel;
    private TextMeshProUGUI dialogueTxt;
    private float lineSpeed = RAPID;

    public GameObject dialogueBtn;

    private bool isDialogueActive = false;
    private bool isDialoguePossible = false;

    // Start is called before the first frame update
    void Start()
    {
        Tale = this;
        
        //fullLine = "Oh hey, can you come for a moment? >";
        thoughtTxt = thoughtsPanel.GetComponentInChildren<TextMeshProUGUI>();
        dialogueTxt = dialoguePanel.GetComponentInChildren<TextMeshProUGUI>();

        responseTxt1 = responsesBtn[0].gameObject.GetComponentInChildren<TextMeshProUGUI>();
        responseTxt2 = responsesBtn[1].gameObject.GetComponentInChildren<TextMeshProUGUI>();
        responsesBtn[0].onClick.AddListener(() => FirstCharacterTalk(0));
        responsesBtn[1].onClick.AddListener(() => FirstCharacterTalk(1));

        sceneLoader = new XMLSceneLoader();

        // Load dialogues from the XML file
        sceneLoader.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        { 
            Application.Quit();
        }

        // Check if the dialogue is active and player presses a key to advance the dialogue
        if (isThoughtsActive && !isDialogueActive)
        {
            thoughtTime += Time.deltaTime;
            if(thoughtTime > thoughtFrequency)
            {
                thoughtTime = 0.0f;
                AdvanceThoughts();
            }
        }
    }

    public void AdvanceDialogue()
    {
        dialogueBtn.SetActive(false);
        currentThoughtIndex++;
        AdvanceDialogue(currentThoughtIndex);
    }

    public void AdvanceDialogue(int line)
    {
        currentThoughtIndex = line;
        isDialoguePossible = false;
        isDialogueActive = true;
        SceneNode currentNode = GetCurrentNode();

        // Display the next line of thought if there is a thought.
        string thought = currentNode.thought;

        if (thought != null)
        {
            thoughtTxt.text = thought;
            StartCoroutine(TimedThoughtLine());
        }


        if (currentNode.lineFirstCharacter != null)
        {
            dialogueTxt.alignment = TextAlignmentOptions.Left;
            StartCoroutine(TimedDialogueLine(currentNode.lineFirstCharacter));
        }
        else if (currentNode.lineSecondCharacter != null)
        {
            dialogueTxt.alignment = TextAlignmentOptions.Right;
            StartCoroutine(TimedDialogueLine(currentNode.lineSecondCharacter));
        }
        
        if (currentNode.thoughtOptions.Count > 0)
        {
            ShowResponses(currentNode);
        }
    }

    public void AdvanceThoughts()
    {
        //isDialogueActive = true;
        isThoughtsActive = true;
        // Display the next line of thought.
        thoughtTxt.text = GetCurrentNode().thought;

        StartCoroutine(TimedThoughtLine());

        // Move to the next line of scene thoughts
        currentThoughtIndex++;

        if (GetCurrentNode().nodeType == NodeType.START_DIALOGUE)
        {
            isDialoguePossible = true;
            isThoughtsActive = false;
        }
    }

    public void AdvanceThoughts(int thoughtNode)
    {
        currentThoughtIndex = thoughtNode;

        // Display the next line of thought.
        thoughtTxt.text = GetCurrentNode().thought;

        StartCoroutine(TimedThoughtLine());
    }

    private SceneNode GetCurrentNode()
    {
        return sceneLoader.sceneLines[currentSceneIndex].sceneNodes[currentThoughtIndex];
    }

    public bool IsDialogueActive()
    {
        return isDialogueActive;
    }

    public bool IsDialoguePossible()
    {
        return isDialoguePossible;
    }

    void ShowResponses(SceneNode node)
    {
        thoughtsPanel.SetActive(true);
        faceCameraObj.SetActive(true);
        responsePanel.SetActive(true);
        
        responseTxt1.text = node.thoughtOptions[0].text;
        responseTxt2.text = node.thoughtOptions[1].text;
    }

    public void FirstCharacterTalk(int responseOptionIdx)
    {
        thoughtsPanel.SetActive(false);
        faceCameraObj.SetActive(false);
        responsePanel.SetActive(false);

        SceneNode node = GetCurrentNode();
        ThoughtOption optionChose = node.thoughtOptions[responseOptionIdx];

        dialogueTxt.alignment = TextAlignmentOptions.Left;
        StartCoroutine(TimedDialogueLine(optionChose.text));
        currentThoughtIndex = optionChose.nextNode - 1;
    }

    public int CurrentStroke
    { 
        get { return currentThoughtIndex; } 
        set { currentThoughtIndex = value; } 
    }

    public void SetStroke(string stroke)
    {
        thoughtTxt.text = stroke;
        StartCoroutine(TimedThoughtLine());
        //StartCoroutine(TimedDialogueLine(stroke));
    }


    private IEnumerator TimedDialogueLine(string fullLine) 
    {
        dialogueTxt.text = "\n";
        dialoguePanel.SetActive(true);
        int characterIdx = 0;
        while (characterIdx < fullLine.Length) 
        {
            if (fullLine[characterIdx] == ',')
            {
                lineSpeed += COMMA;
            }
            else
                lineSpeed = RAPID;

            dialogueTxt.text += fullLine[characterIdx];
            characterIdx++;
            yield return new WaitForSeconds(lineSpeed);
        }
        //yield return new WaitForSeconds(show);

        if(!isDialogueActive)
            dialoguePanel.SetActive(false);
        dialogueBtn.SetActive(true);
    }

    private IEnumerator TimedThoughtLine()
    {
        thoughtsPanel.SetActive(true);
        faceCameraObj.SetActive(true);

        yield return new WaitForSeconds(SHOWLINETIME);

        SceneNode currentNode = GetCurrentNode();
        if(currentNode.nodeType == NodeType.END_DIALOGUE)
        {
            dialoguePanel.SetActive(false);
            thoughtsPanel.SetActive(false);
            faceCameraObj.SetActive(false);
        }
        else
        {
            dialogueBtn.SetActive(true);
            thoughtsPanel.SetActive(false);
            faceCameraObj.SetActive(false);
        }

        // If Andrea get the lantern. Any PNG disappear and he takes it.
        if (currentNode.nodeGift == Gift.LANTERN)
        {
            // Png gives the lantern to Andrea.
            pngController.GiveLantern();

            // Remove the png.
            Destroy(pngController.gameObject);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
