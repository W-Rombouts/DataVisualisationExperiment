using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class ScaleManager : MonoBehaviour
{
    public TempAnswerHolder tmpAnsw;
    public Slider Scale;
    public TextMeshProUGUI Value;
    public int MaxScale;
    public int MinScale=1;
    // Start is called before the first frame update
    void Start()
    {
        Value.text = (Scale.normalizedValue * ( MaxScale- MinScale)).ToString("F0");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void UpdateValue()
    {
        Value.text = (Scale.normalizedValue * (MaxScale-MinScale)).ToString("F0");
        tmpAnsw.SetAnswer((Scale.normalizedValue * (MaxScale-MinScale)).ToString("F0"));
    }


}
