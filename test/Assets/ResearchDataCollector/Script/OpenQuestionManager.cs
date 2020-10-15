using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEditor.Hardware;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class OpenQuestionManager : MonoBehaviour
{
    public AudioSource recordedAnswer;
    public GameObject recordButton;
    Text recordButtonText;
    string microphoneVive = "Microphone (USB Audio Device)";
    private bool micConnected = false;
    private int minFreq;
    private int maxFreq;
    private bool answerRecorded = false;
    private float recordStart;


    // Start is called before the first frame update
    void Awake()
    {
        recordButtonText = recordButton.GetComponentInChildren<Text>();
        //Check if there is at least one microphone connected  
        if (Microphone.devices.Length <= 0)
        {
            //Throw a warning message at the console if there isn't  
            Debug.LogWarning("Microphone not connected!");
        }
        else //At least one microphone is present  
        { 
            micConnected = true;

            Microphone.GetDeviceCaps(microphoneVive, out minFreq, out maxFreq);            
        }
    }

    public void Done()
    {
        QuestionairManager.Instance.RecordAnswer(recordedAnswer.clip,recordStart);
        gameObject.SetActive(false);
    }
    public void Redo()
    {
        answerRecorded = false;
        recordButtonText.text = "Record";
    }

    public void Record()
    {
        if (micConnected)
        {
            if (!Microphone.IsRecording(microphoneVive) && !answerRecorded)
            {
                recordStart = Time.realtimeSinceStartup;
                recordedAnswer.clip = Microphone.Start(microphoneVive, true,20, maxFreq*2);
                recordButtonText.text = "Stop";
            }
            else
            {
                Microphone.End(microphoneVive);
                recordButtonText.text = "Play";
                answerRecorded = true;
            }
            if (answerRecorded)
            {
                if (recordedAnswer.isPlaying)
                {
                    recordedAnswer.Stop();
                    recordButtonText.text = "Play";
                }
                else
                {
                    recordedAnswer.Play();
                    recordButtonText.text = "Stop";
                }
            }
        }
    }
}
