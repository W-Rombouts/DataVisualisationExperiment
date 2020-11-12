using System;
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
    TempAnswerHolder tempAnsw;
    public AudioSource recordedAnswer;
    public GameObject recordButton;
    UnityEngine.UI.Image recordButtonImage;
    float recordStop;
    Text recordButtonText;
    readonly string microphoneVive = "Microphone (USB Audio Device)";
    private bool micConnected = false;
    private int minFreq;
    private int maxFreq;
    private bool answerRecorded = false;
    private float recordStart;
    public int MaxRecordingDuration = 1800;
    public Sprite recording;
    public Sprite stop;
    public Sprite play;
    public Sprite record;


    // Start is called before the first frame update
    void Awake()
    {
        tempAnsw = GameObject.Find("NextQuestion").GetComponent<TempAnswerHolder>();
        recordButtonText = recordButton.GetComponentInChildren<Text>();
        recordButtonImage = recordButton.GetComponentInChildren<UnityEngine.UI.Image>();
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

    public void Redo()
    {
        answerRecorded = false;
        recordButtonText.text = "Record";
        recordButtonImage.sprite = record;
    }

    public void Record()
    {
        if (micConnected)
        {
            if (!answerRecorded)
            {
                if (!Microphone.IsRecording(microphoneVive))
                {
                    recordStart = Time.realtimeSinceStartup;
                    recordedAnswer.clip = Microphone.Start(microphoneVive, true, MaxRecordingDuration, maxFreq*2);
                    recordButtonText.text = "Stop Recording";
                    recordButtonImage.sprite = stop;
                }
                else
                {
                    recordStop = Time.realtimeSinceStartup;
                    Microphone.End(microphoneVive);
                    int usefullSamplesCount = (int)(((recordStop - recordStart) / MaxRecordingDuration) * recordedAnswer.clip.samples);
                    float[] samples = new float[recordedAnswer.clip.samples];
                    recordedAnswer.clip.GetData(samples, 0);
                    float[] usefullSamples = new float[usefullSamplesCount+1];
                    Array.Copy(samples, usefullSamples, usefullSamplesCount-1);
                    AudioClip trimmedClip = AudioClip.Create("RecordedAnswer", usefullSamples.Length, recordedAnswer.clip.channels, recordedAnswer.clip.frequency, false);
                    trimmedClip.SetData(usefullSamples, 0);
                    recordedAnswer.clip = trimmedClip;
                    tempAnsw.SetAnswer(trimmedClip,recordStart);
                    recordButtonText.text = "Play Recording";
                    recordButtonImage.sprite = play;
                    answerRecorded = true;
                }
            }
            else
            {
                if (recordedAnswer.isPlaying)
                {
                    recordedAnswer.Stop();
                    recordButtonText.text = "Play Recording";
                    recordButtonImage.sprite = play;
                }
                else
                {
                    recordedAnswer.Play();
                    recordButtonText.text = "Stop Playback";
                    recordButtonImage.sprite = stop;
                }
            }
        }
    }
}
