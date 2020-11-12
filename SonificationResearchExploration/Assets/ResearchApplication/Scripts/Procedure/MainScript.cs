using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    public GameObject BaseLine;
    public GameObject simonSays;
    SimonSays simonScript;
    public GameObject MinimalNoticeableDistance;
    public GameObject Fase1;
    public GameObject Fase4;

    bool done1 = false;
    bool done2 = false;
    bool done3 = false;

    private float counter;
    // Start is called before the first frame update
    void Start()
    {
        simonScript = simonSays.GetComponent<SimonSays>();
        
    }
    

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
        DoOneSequence();
    }
    #region Instruction

    #endregion

    #region SimonSays Tone
    public void DoOneSequence()
    {
        if (counter > 0 && !done1)
        {
            simonScript.SetSoundofOrb(SimonSays.SimonSaysOrb.OrbLeft,freq: 524f);
            done1 = true;
        }
        if (counter > 2 && !done2)
        {
            simonScript.SetSoundofOrb(SimonSays.SimonSaysOrb.OrbMiddle, freq: 1524f);
            
            done2 = true;
        }
        if (counter > 4 && !done3)
        {
            simonScript.SetSoundofOrb(SimonSays.SimonSaysOrb.OrbRight, freq: 264f);
            done3 = true;
        }


    }

    #endregion

    #region SimonSays Volume

    #endregion

    #region NinimalNoticeableDistance

    #endregion

    #region Fase 1 

    #endregion

    #region Fase 2

    #endregion

    #region Fase 3

    #endregion

    #region Fase 4

    #endregion

}
