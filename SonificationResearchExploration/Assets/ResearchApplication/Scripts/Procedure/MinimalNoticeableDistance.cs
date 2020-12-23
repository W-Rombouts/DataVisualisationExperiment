using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class MinimalNoticeableDistance : MonoBehaviour
{
    public GameObject node;
    public TempAnswerHolder tempAnsw;
    public bool isNoticeable = true;
    public bool isAnswered = false;
    private MainScript mainScript;
    private AudioManager audioManager;
    private int leftPosition =45;
    private int rightPosition= 135;
    private List<GameObject> liveNodes;
    bool done1 = false;
    bool done2 = false;
    float counter = 0f;
    bool isFirstQuestion = true;
    bool runFirstTime = true;
    bool isAsked = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DoMNDSequence()
    {
        counter += Time.deltaTime;
        if (runFirstTime)
        {
            liveNodes = new List<GameObject>();
            mainScript = MainScript.Instance;
            audioManager = AudioManager.Instance;
            SpawnNodes();
            mainScript.phasAnswerSync = true;
            runFirstTime = false;
        }
        if (!isAnswered&& !done1)
        {
            audioManager.SetAudioOnObject(liveNodes[0]);
            done1 = true;
        }
        else if (!isAnswered && !done2 && counter > 2f)
        {
            audioManager.SetAudioOnObject(liveNodes[1]);
            done2 = true;
            if (isFirstQuestion)
            {
                QuestionnaireManager.Instance.AskQuestion();
                isFirstQuestion = false;
            }
            else
            {
                QuestionnaireManager.Instance.AskQuestion(isRepeatQuestion: true);
            }
            
            
        }
        

        if (isAnswered)
        {
            isAnswered = false;
            DestroyNodes();
            counter = 0f;
            done1 = false;
            done2 = false;
            if (isNoticeable)
            {
                leftPosition-=5;
                rightPosition-=5;
                SpawnNodes();
                
            }
            else
            {
                float MND = Vector3.Distance(mainScript.GetPositionFromDegree(leftPosition), mainScript.GetPositionFromDegree(rightPosition));
                Answer answ = new Answer();
                answ.question = "What is the mimimal noticable diffrence for this participant?";
                answ.answer = MND.ToString();
                answ.questionAnswerTime = Time.realtimeSinceStartup;
                answ.questionAskTime = Time.realtimeSinceStartup;
                QuestionnaireManager.Instance.LogNonQuestionnaireAnswer(answ);
                mainScript.UpdatePhase();
            }
        }

        if (!mainScript.phasAnswerSync)
        {
            QuestionAnswer();
            mainScript.phasAnswerSync = true;
        }
    }



    


    public void QuestionAnswer()
    {
        if (MainScript.Instance.phase == 2)
        {
            isAsked = false ;
            isAnswered = true;
            Debug.Log(tempAnsw.answer);
            if (tempAnsw.answer == "No")
            {
                isNoticeable = false;
            }
            else
            {
                isNoticeable = true;
            }
            
        }

    }

    private void SpawnNodes()
    {
        liveNodes = new List<GameObject>();
        liveNodes.Add(Instantiate(node, mainScript.GetPositionFromDegree(leftPosition), Quaternion.identity));
        liveNodes[0].GetComponent<UpdateFlare>().nodeMesh.enabled = false;
        liveNodes.Add(Instantiate(node, mainScript.GetPositionFromDegree(rightPosition), Quaternion.identity));
        liveNodes[1].GetComponent<UpdateFlare>().nodeMesh.enabled = false;

    }

    private void DestroyNodes()
    {
        foreach (var item in liveNodes)
        {
            Destroy(item);
        }
    }



}
