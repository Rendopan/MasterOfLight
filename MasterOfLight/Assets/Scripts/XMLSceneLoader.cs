using System.Collections.Generic;
using System.Xml;
using System.IO;
using UnityEngine;

//[System.Serializable]
//public class DialogueLine
//{
//    public string text;
//    public List<ThoughtOption> responses = new List<ThoughtOption>();
//}

public enum NodeType
{
    NORMAL = 0,
    START_DIALOGUE,
    END_DIALOGUE
};

public enum Gift
{
    NONE = 0,
    LANTERN,
    SHEET
};

[System.Serializable]
public class ThoughtOption
{
    public string text;
    public int index;
    public int nextNode;
}

[System.Serializable]
public class Scene
{
    public string sceneName;
    public List<SceneNode> sceneNodes = new List<SceneNode>();
}

[System.Serializable]
public class SceneNode
{
    public int index;
    public NodeType nodeType;  
    public Gift nodeGift;
    public int giftIndex;
    public string thought;
    public string lineFirstCharacter;
    public string lineSecondCharacter;
    public List<ThoughtOption> thoughtOptions = new List<ThoughtOption>();
}

public class XMLSceneLoader : MonoBehaviour
{
    //public TextAsset scenesXML; // Reference to the XML file containing dialogues

    public List<Scene> sceneLines;

    public void Start()
    {
        sceneLines = LoadDialoguesFromXML();
    }

    List<Scene> LoadDialoguesFromXML()
    {
        List<Scene> sceneList = new List<Scene>();
        //TextAsset xmlFile = (TextAsset)Resources.Load<TextAsset>("CorinaldoScene1.xml");

        // Specify the path to the XML file inside the "StreamingAssets" folder
        string xmlFile = Path.Combine(Application.streamingAssetsPath, "CorinaldoScene1.xml");

        if (xmlFile != null)
        {
            XmlDocument xmlDoc = new XmlDocument();

            //TextAsset textAsset = Resources.Load<TextAsset>("CorinaldoScene1.xml");

            xmlDoc.Load(xmlFile);

            XmlNodeList scenes = xmlDoc.SelectNodes("/Tale/Scene");

            foreach (XmlNode xmlScene in scenes)
            {
                Scene scene = new Scene();
                scene.sceneName = xmlScene.SelectSingleNode("SceneName").InnerText;

                scene.sceneNodes = new List<SceneNode>();

                XmlNodeList sceneNodes = xmlScene.SelectNodes("SceneTree/Node");

                foreach (XmlNode xmlSceneNode in sceneNodes)
                {
                    SceneNode sceneNode = new SceneNode();

                    sceneNode.index = int.Parse(xmlSceneNode.Attributes["index"].Value);
                    sceneNode.nodeType = (NodeType)int.Parse(xmlSceneNode.Attributes["type"].Value);

                    if (xmlSceneNode.SelectSingleNode("Thought") != null)
                        sceneNode.thought = xmlSceneNode.SelectSingleNode("Thought").InnerText;
                    else
                        sceneNode.thought = null;

                    XmlNode textNode = xmlSceneNode.SelectSingleNode("Text");
                    if (textNode != null)
                    {
                        if (textNode.Attributes["name"].Value == "Aria")
                        {
                            sceneNode.lineFirstCharacter = textNode.InnerText;
                            sceneNode.lineSecondCharacter = null;
                        }
                        else
                        {
                            sceneNode.lineFirstCharacter = null;
                            sceneNode.lineSecondCharacter = textNode.InnerText;
                        }
                    }
                    else
                    {
                        sceneNode.lineFirstCharacter = null;
                        sceneNode.lineSecondCharacter = null;
                    }

                    textNode = xmlSceneNode.SelectSingleNode("Gift");
                    if(textNode != null) 
                    {
                        if (int.Parse(textNode.Attributes["type"].Value) == 0)
                        {
                            sceneNode.nodeGift = Gift.LANTERN;
                        }
                        else
                        {
                            sceneNode.nodeGift = Gift.SHEET;
                        }
                        sceneNode.giftIndex = int.Parse(textNode.Attributes["id"].Value);
                    }
                    else
                    {
                        sceneNode.nodeGift = Gift.NONE;
                    }
                    
                    XmlNodeList responseNodes = xmlSceneNode.SelectNodes("Responses/Response");
                    foreach (XmlNode responseNode in responseNodes)
                    {
                        ThoughtOption responseOption = new ThoughtOption();
                        responseOption.text = responseNode.InnerText;
                        responseOption.index = int.Parse(responseNode.Attributes["index"].Value);
                        responseOption.nextNode = int.Parse(responseNode.Attributes["nextNode"].Value);
                        
                        sceneNode.thoughtOptions.Add(responseOption);
                    }
                    scene.sceneNodes.Add(sceneNode);
                }
                sceneList.Add(scene);
            }
        }
        return sceneList;
    }
}
