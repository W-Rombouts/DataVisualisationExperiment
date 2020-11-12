using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimonSays : MonoBehaviour
{
    public GameObject OrbLeft;
    public GameObject OrbMiddle;
    public GameObject OrbRight;
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = AudioManager.Instance;
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
