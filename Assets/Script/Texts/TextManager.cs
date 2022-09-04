using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml;
using TMPro;

public class TextManager : MonoBehaviour
{
    static TextManager instance;
    public static TextManager Instance { get => instance; }

    [SerializeField] private TextMeshProUGUI TMPText;
    [SerializeField] private int totalSentences;

    private static Dictionary<string, List<string>> optionsByScene;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        TMPText.transform.parent.gameObject.SetActive(false);
        LoadSceneData();
    }

    public TextMeshProUGUI GetText()
    {
        return TMPText;
    }

    public int TotalSentences()
    {
        return totalSentences;
    }

    private void LoadSceneData()
    {
        optionsByScene = new Dictionary<string, List<string>>();

        TextAsset xmlData = (TextAsset)Resources.Load("texts");
        XmlDocument xmlDocument = new XmlDocument();
        xmlDocument.LoadXml(xmlData.text);

        foreach (XmlNode scene in xmlDocument["scenes"].ChildNodes)
        {
            string sceneName = scene.Attributes["name"].Value;

            List<string> texts = new List<string>();

            foreach (XmlNode text in scene["texts"].ChildNodes)
            {
                texts.Add(text.InnerText);
            }

            optionsByScene[sceneName] = texts;
        }
    }

    //private void Update()
    //{
    //    foreach (KeyValuePair<string, List<string>> optionsByScene in optionsByScene)
    //    {
    //        for(int i = 0; i < optionsByScene.Value.Count; ++i)
    //        {
    //            Debug.Log($"Key : {optionsByScene.Key}  y value: {optionsByScene.Value[i]}");
    //        }
    //    }
    //}

    public string PopulateText(int option)
    {
        foreach (KeyValuePair<string, List<string>> optionsByScene in optionsByScene)
        {
            Debug.Log(optionsByScene.Value[option]);
            return optionsByScene.Value[option];
        }
        return "NO Frase";
    }
}
