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
    float time1 = 0;
    float time2 = 2;
    float time3 = 4;
    bool done1 = false;
    bool done2 = false;
    bool done3 = false;

    private float counter;
    List<string> answer = new List<string>();

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
    }

    public void DoOneSequence()
    {
        if (counter > time1 && !done1)
        {
            SetSoundofOrb(SimonSays.SimonSaysOrb.OrbLeft, freq: 524f);
            done1 = true;
        }
        if (counter > time2 && !done2)
        {
            SetSoundofOrb(SimonSays.SimonSaysOrb.OrbMiddle, freq: 1524f);

            done2 = true;
        }
        if (counter > time3 && !done3)
        {
            SetSoundofOrb(SimonSays.SimonSaysOrb.OrbRight, freq: 264f);
            done3 = true;
        }

        if (answer.Count == 3)
        {
            //TODO: report the answer
            answer = new List<string>();
            itteration++;
            if (itteration == 1)
            {
                time1 = 4f;
                time2 = 2f;
                time3 = 0f;
            }
            else if (itteration == 2)
            {
                time1 = 2f;
                time2 = 4f;
                time3 = 0f;
            }
            else if (itteration == 3)
            {
                time1 = 2f;
                time2 = 0f;
                time3 = 4f;
            }
            else if (itteration == 4)
            {
                time1 = 2f;
                time2 = 0f;
                time3 = 4f;
            }
            else
            {
                mainScript.UpdatePhase();
            }
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
