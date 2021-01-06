using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fase4 : MonoBehaviour
{
    public PreloadedLayout network;
    MainScript mainScript;
    bool first = true;
    int counter = 0;
    private bool isAnswered = true;
    public GameObject Pointer;
    private float askedTime;

    #region SINGLETON PATTERN
    public static Fase4 Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion



    // Start is called before the first frame update
    void Start()
    {
        mainScript = MainScript.Instance;
        network.LockedNodesList.Shuffle();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DoSequenceFase4()
    {
        if (first)
        {
            mainScript.phasAnswerSync = true;
            QuestionnaireManager.Instance.AskQuestion();
            first = false;
        }
        if (!mainScript.phasAnswerSync && isAnswered)
        {
            if (network.LockedNodesList.Count - 1 == counter)
            {
                Pointer.SetActive(false);
                mainScript.UpdatePhase(true);
                mainScript.phasAnswerSync = true;
                
            }
            else
            {
                Pointer.SetActive(true);
                mainScript.SetSoundofOrb(network.LockedNodesList[counter],isFlare:true);
                isAnswered = false;
                counter++;
            }
            askedTime = Time.realtimeSinceStartup;
        }
    }
    public void AnswerFase(Vector3 selectedPosition)
    {
        phaseAnswer newAnswer = new phaseAnswer
        {
            faseNR = 4,
            selectedLocation = selectedPosition,
            ActualLocation = network.LockedNodesList[counter].transform.position
        };
        newAnswer.CalcDiffrence();

        Answer anws = new Answer()
        {
            question = "What is the error in condition " + newAnswer.faseNR + "?",
            answer = newAnswer.diffrence.ToString(),
            questionAnswerTime = Time.realtimeSinceStartup,
            questionAskTime = askedTime
        };
        QuestionnaireManager.Instance.LogNonQuestionnaireAnswer(anws);
        isAnswered = true;
    }
}
