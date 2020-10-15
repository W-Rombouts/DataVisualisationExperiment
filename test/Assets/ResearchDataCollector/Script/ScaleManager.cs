using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class ScaleManager : MonoBehaviour
{
    public Slider Scale;
    public TextMeshProUGUI Value;
    public int MaxScale;
    public int MinScale=1;
    //public float WaitTimeAfterAnswer = 1f;
    bool valueChanged = false;
    float valueChangedTimer =0;
    // Start is called before the first frame update
    void Start()
    {
        Value.text = (Scale.normalizedValue * ( MaxScale- MinScale)).ToString("F0");
    }

    // Update is called once per frame
    void Update()
    {
        if (valueChanged)
        {
            valueChangedTimer += Time.deltaTime;
            if (SteamVR_Actions.Questionair_Control.SelectAnswer.GetStateUp(SteamVR_Input_Sources.RightHand))
            {
                QuestionairManager.Instance.RecordAnswer(Value.text);
                gameObject.SetActive(false);
            }
        }
    }

    public void UpdateValue()
    {
        valueChanged = true;
        valueChangedTimer = 0;
        Value.text = (Scale.normalizedValue * (MaxScale-MinScale)).ToString("F0");
    }
}
