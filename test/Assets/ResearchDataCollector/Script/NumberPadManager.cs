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

    private void Awake()
    {
        Display.text = DefaultValue;
    }

    public void AddValue(GameObject numberButton)
    {
        if (isDefaultValue)
        {
            isDefaultValue = false;
            Debug.Log(Display);
            Debug.Log(numberButton.name);
            Display.text = numberButton.GetComponentInChildren<Text>().text;
        }
        else
        {
            Display.text += numberButton.GetComponentInChildren<Text>().text;
        }
        
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
    }

    public void OK()
    {
        QuestionairManager.Instance.RecordAnswer(Display.text);
        isDefaultValue = true;
        Display.text = DefaultValue;
        gameObject.SetActive(false);
    }
}
