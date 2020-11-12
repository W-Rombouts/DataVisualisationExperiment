using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LikertManager : MonoBehaviour
{
    public GameObject size5;
    public GameObject size7;
    public TextMeshProUGUI blockQuestionText;
    public TextMeshProUGUI TextMin5;
    public TextMeshProUGUI TextMax5;
    public TextMeshProUGUI TextMin7;
    public TextMeshProUGUI TextMax7;

    public void SetLikertSize(string minText,string maxText,string blockQuestion = "",bool sizeIs5 = true)
    {
        blockQuestionText.text = blockQuestion;
        size5.SetActive(sizeIs5);
        size7.SetActive(!sizeIs5);
        if (sizeIs5)
        {
            TextMin5.transform.parent.gameObject.SetActive(true);
            TextMax5.transform.parent.gameObject.SetActive(true);
            TextMin7.transform.parent.gameObject.SetActive(false);
            TextMax7.transform.parent.gameObject.SetActive(false);
            TextMin5.text = minText;
            TextMax5.text = maxText;
        }
        else
        {
            TextMin5.transform.parent.gameObject.SetActive(true);
            TextMax5.transform.parent.gameObject.SetActive(true);
            TextMin7.transform.parent.gameObject.SetActive(false);
            TextMax7.transform.parent.gameObject.SetActive(false);
            TextMin7.text = minText;
            TextMax7.text = maxText;
        }
    }
}
