using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSays : MonoBehaviour
{
    public GameObject OrbLeft;
    public GameObject OrbMiddle;
    public GameObject OrbRight;
    private AudioManager audioManager;
    private MainScript mainScript;
    int itteration = 0;
    float freq1 = 524f;
    float freq2 = 1524f;
    float freq3 = 264f;
    bool done1 = false;
    bool done2 = false;
    bool done3 = false;
    List<float> freqList = new List<float>() { 264f, 524f, 1524f };
    private float counter;
    List<string> answer = new List<string>();
    List<string> checkList = new List<string>();

    private void Awake()
    {
        audioManager = AudioManager.Instance;
        mainScript = MainScript.Instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void SetSoundofOrb(SimonSaysOrb orb, float volume = 0.5f)
    {
        audioManager.SetAudioOnObject(getOrbFromEnum(orb), volume: volume);
    }
    public void SetSoundofOrb(SimonSaysOrb orb,float freq,float volume = 0.5f)
    {
        audioManager.SetAudioOnObject(getOrbFromEnum(orb), audioManager.MakeAudioClip(freq), volume:volume);
        getOrbFromEnum(orb).GetComponent<UpdateFlare>().EnableFlare();
    }


    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
    }

    public void RecordSimonSaysAnswer(GameObject go)
    {
        string orbName;
        if (go == OrbLeft)
        {
            orbName = "Left";
        }
        else if (go == OrbMiddle)
        {
            orbName = "Middle";
        }
        else if (go == OrbRight)
        {
            orbName = "Right";
        }
        else
        {
            orbName = "error";
        }
        answer.Add(orbName);
        foreach (var item in answer)
        {
            Debug.Log(item);
        }
       
    }

    public void DoOneSequence()
    {
        if (counter > 0f && !done1)
        {
            SetSoundofOrb(SimonSays.SimonSaysOrb.OrbLeft, freq: freq1);
            done1 = true;
            
        }
        if (counter > 2f && !done2)
        {
            SetSoundofOrb(SimonSays.SimonSaysOrb.OrbMiddle, freq: freq2);

            done2 = true;
        }
        if (counter > 4f && !done3)
        {
            SetSoundofOrb(SimonSays.SimonSaysOrb.OrbRight, freq: freq3);
            done3 = true;
        }

        if (answer.Count == 3)
        {
            if (answer == checkList)
            {
                Answer answ = new Answer()
                {
                    question = "Can the participant solve sequence " + (itteration + 1).ToString() + " correctly?",
                    answer = "Yes",
                    questionAnswerTime = Time.realtimeSinceStartup,
                    questionAskTime = Time.realtimeSinceStartup
                };
                QuestionnaireManager.Instance.LogNonQuestionnaireAnswer(answ);
            }
            else
            {
                Answer answ = new Answer()
                {
                    question = "Can the participant solve sequence " + (itteration + 1).ToString() + " correctly?",
                    answer = "No",
                    questionAnswerTime = Time.realtimeSinceStartup,
                    questionAskTime = Time.realtimeSinceStartup
                };
                QuestionnaireManager.Instance.LogNonQuestionnaireAnswer(answ);
            }
            

            counter = 0;
            answer = new List<string>();
            itteration++;
            if (itteration == 1)
            {
                freq1 = freqList[0];
                freq2 = freqList[1];
                freq3 = freqList[2];
                checkList = new List<string>() { "Left", "Middle", "Right" };
            }
            else if (itteration == 2)
            {
                freq1 = freqList[0]; 
                freq2 = freqList[2];
                freq3 = freqList[1];
                checkList = new List<string>() { "Left",  "Right","Middle" };
            }
            else if (itteration == 3)
            {
                freq1 = freqList[1]; 
                freq2 = freqList[0]; 
                freq3 = freqList[2];
                checkList = new List<string>() {  "Middle", "Left", "Right" };
            }
            else if (itteration == 4)
            {
                freq1 = freqList[2];
                freq2 = freqList[1]; 
                freq3 = freqList[0];
                checkList = new List<string>() {"Right", "Middle", "Left" };
            }
            else
            {
                mainScript.UpdatePhase();
            }
            done1 = false;
            done2 = false;
            done3 = false;
            counter = 0;
        }

    }


    public enum SimonSaysOrb{
        OrbLeft,
        OrbMiddle,
        OrbRight
    }

    private GameObject getOrbFromEnum(SimonSaysOrb orb)
    {
        GameObject GO;
        if (orb == SimonSaysOrb.OrbLeft)
        {
            GO = OrbLeft;
        }
        else if (orb == SimonSaysOrb.OrbMiddle)
        {
            GO = OrbMiddle;
        }
        else
        {
           GO = OrbRight;
        }
        return GO;
    }
}
