using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LikertManager : MonoBehaviour
{
    public GameObject size5;
    public GameObject size7;

    public void SetLikertSize(bool sizeIs5 = true)
    {
        size5.SetActive(sizeIs5);
        size7.SetActive(!sizeIs5);
    }

    public void AnswerQuestion(Text answer)
    {
        QuestionairManager.Instance.RecordAnswer(answer.text);
        gameObject.SetActive(false);
    }
}
