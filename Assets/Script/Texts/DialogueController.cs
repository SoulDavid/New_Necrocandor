using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueController : MonoBehaviour
{
    private int option = 0;
    [SerializeField] private float DialogueSpeed;
    private PlayerCollider colliderText;
    [SerializeField] private bool sentenceHasFinished;
    private bool StartFromScratch;

    // Start is called before the first frame update
    void Start()
    {
        colliderText = GetComponent<PlayerCollider>();
        sentenceHasFinished = true;
        StartFromScratch = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStartFromScratch(bool _newState)
    {
        StartFromScratch = _newState;
    }

    public void WantToKnowHistory(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            if (colliderText.GetCanDialogue())
            {
                TextManager.Instance.GetText().transform.parent.gameObject.SetActive(true);
                if (sentenceHasFinished)
                {
                    NextSentence();
                }
            }
        }
    }

    private void NextSentence()
    {
        if (StartFromScratch)
        {
            option = 0;
        }

        if (option <= TextManager.Instance.TotalSentences() - 1)
        {
            TextManager.Instance.GetText().text = "";
            sentenceHasFinished = false;
            StartCoroutine(Sentence());
        }
        else
        {
            TextManager.Instance.GetText().transform.parent.gameObject.SetActive(false);
        }
        

    }

    private IEnumerator Sentence()
    {
        string frase = TextManager.Instance.PopulateText(option);

        foreach(char Character in frase.ToCharArray())
        {
            TextManager.Instance.GetText().text += Character;
            yield return new WaitForSeconds(DialogueSpeed);
        }

        StartFromScratch = false;
        sentenceHasFinished = true;
        option++;
    }

}
