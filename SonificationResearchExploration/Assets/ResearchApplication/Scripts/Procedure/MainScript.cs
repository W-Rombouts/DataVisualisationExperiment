using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public GameObject BaseLine;
    public GameObject simonSays;
    SimonSays simonScript;
    AudioManager audioManager;
    public GameObject MinimalNoticeableDistance;
    MinimalNoticeableDistance noticeableDistance;
    public GameObject Fase1;
    Fase1 fase1Script;
    public GameObject Fase4;
    Fase4 fase4Script;
    public bool phasAnswerSync;
    public int phase = 0;
    private bool isQuestionAsked = true;
    private bool isQuestionAnswered = true;

    public static MainScript Instance { get; private set; }
    private void Awake()
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


    // Start is called before the first frame update
    void Start()
    {
        phasAnswerSync = false;
        simonScript = simonSays.GetComponent<SimonSays>();
        fase1Script = Fase1.GetComponent<Fase1>();
        fase4Script = Fase4.GetComponent<Fase4>();
        audioManager = AudioManager.Instance;
        noticeableDistance = MinimalNoticeableDistance.GetComponent < MinimalNoticeableDistance >();
    }

    public void UpdatePhase(bool askQuestion = false)
    {
        phase++;
        if (askQuestion)
        {
            isQuestionAnswered = false;
            phasAnswerSync = true;
            QuestionnaireManager.Instance.AskQuestion();
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        if (isQuestionAnswered)
        {
            if (phase == 0)
            {
                BaseLine.SetActive(true);
            }
            else if (phase == 1)
            {
                BaseLine.SetActive(true);
                simonSays.SetActive(true);
                simonScript.DoOneSequence();
            }
            else if (phase == 2)
            {
                BaseLine.SetActive(true);
                simonSays.SetActive(false);
                MinimalNoticeableDistance.SetActive(true);
                noticeableDistance.DoMNDSequence();
            }
            else if (phase == 3)
            {
                MinimalNoticeableDistance.SetActive(false);
                BaseLine.SetActive(false);
                Fase1.SetActive(true);
                fase1Script.DoSequenceFase1();
            }
            else if (phase == 4)
            {
                fase1Script.DoSequenceFase2();
            }
            else if (phase == 5)
            {
                fase1Script.DoSequenceFase3();
            }
            else if (phase == 6)
            {
                Fase1.SetActive(false);
                Fase4.SetActive(true);
                fase4Script.DoSequenceFase4();
            }
            else if (phase == 7)
            {
                if (!phasAnswerSync)
                {
                    QuestionnaireManager.Instance.SaveQuestionnaire();
                }

            }
        }


        if (phasAnswerSync == false)
        {
            isQuestionAnswered = true;
        }


    }
    public void setPhaseAnswerSync()
    {
        phasAnswerSync = false;
    }
    

    public Vector3 GetPositionFromDegree(int degree,int radius = 7)
    {
        float radiant = degree * (3.14f / 180);

        return new Vector3(Mathf.Cos(radiant) * radius, 0f, Mathf.Sin(radiant) * radius);
    }

    public void SetSoundofOrb(GameObject nodeGO, float freq = 400f, bool isFlare = false, float volume = 0.5f)
    {
        audioManager.SetAudioOnObject(nodeGO, audioManager.MakeAudioClip(freq), volume: volume);
        if (isFlare)
        {
            nodeGO.GetComponent<UpdateFlare>().EnableFlare();
        }
    }

}
