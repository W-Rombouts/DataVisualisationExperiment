using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

public class NumberPadManager : MonoBehaviour
{
    public TextMeshProUGUI Display;
    public string DefaultValue;
    bool isDefaultValue= true;
    private TempAnswerHolder answHolder;

    private void Awake()
    {
        answHolder = GameObject.Find("NextQuestion").GetComponent<TempAnswerHolder>();
        Display.text = DefaultValue;
    }

    public void AddValue(string number)
    {
        if (isDefaultValue)
        {
            isDefaultValue = false;
            Display.text = number;
        }
        else
        {
            Display.text += number;
        }
        answHolder.answer = Display.text;

    }
    public void Undo()
    {
        if (Display.text.Length <= 1)
        {
            Display.text = DefaultValue;//TODO: Should this set to default or to 0 always or empty always?
            isDefaultValue = true;
        }
        else
        {
            Display.text = Display.text.Remove(Display.text.Length - 1, 1);
        }
        answHolder.answer = Display.text;
    }

    public void OK()
    {
        isDefaultValue = true;
        Display.text = DefaultValue;
    }
}
