using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaselineManager : MonoBehaviour
{

    public GameObject simonsSaysGO;
    public SimonSays simonSays;
    public GameObject noticeableDistanceGO;
    public MinimalNoticeableDistance noticeableDistance;

    MainScript mainScript;
    public int stageCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        mainScript = MainScript.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (stageCounter == 0 && mainScript.phasAnswerSync == false)
        {
            QuestionnaireManager.Instance.AskQuestion();
            stageCounter += 1;
            mainScript.phasAnswerSync = true;
        }
        else if (stageCounter == 1 && mainScript.phasAnswerSync == false)
        {
            QuestionnaireManager.Instance.AskQuestion();
            stageCounter += 1;
            mainScript.phasAnswerSync = true;
        }
        else if (stageCounter == 2 && mainScript.phasAnswerSync == false)
        {
            MainScript.Instance.UpdatePhase();
           // simonsSaysGO.SetActive(enabled);
            stageCounter += 1;
            //mainScript.phasAnswerSync = true;
        }
        else if (stageCounter == 3 && mainScript.phasAnswerSync == false)
        {
            //simonsSaysGO.SetActive(false);
            //noticeableDistanceGO.SetActive(true);
            //stageCounter += 1;
            //mainScript.phasAnswerSync = true;
        }

        if (stageCounter == 3 && mainScript.phasAnswerSync == true)
        {
            //simonSays.DoOneSequence();
        }
    }
    


}
