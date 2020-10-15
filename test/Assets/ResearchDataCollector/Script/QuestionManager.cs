using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using TMPro;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public GameObject likertScale;
    public GameObject numberScale;
    public GameObject multiChoise;
    public GameObject numberPad;
    public GameObject openQuestion;


    public void AskQuestion(Question question)
    {
        questionText.text = question.question;

        switch (question.questionType)
        {
            case QuestionType.Likert:
                likertScale.SetActive(true);
                likertScale.GetComponent<LikertManager>().SetLikertSize(question.likertIs5);
                break;
            case QuestionType.NumberScale:
                numberScale.SetActive(true);
                numberScale.GetComponent<ScaleManager>().MaxScale = question.numberScaleMax;
                numberScale.GetComponent<ScaleManager>().MinScale = question.numberScaleMin;
                break;
            case QuestionType.MultiChoise:
                multiChoise.SetActive(true);
                multiChoise.GetComponent<MultiChoiseManager>().SetAnswers(question.multiAnswers);
                break;
            case QuestionType.NumberPad:
                numberPad.SetActive(true);
                numberPad.GetComponent<NumberPadManager>().DefaultValue = question.numPadDefaultValue;
                break;
            case QuestionType.OpenQuestion:
                openQuestion.SetActive(true);
                //openQuestion.GetComponent<OpenQuestionManager>().DefaultValue = question.numPadDefaultValue;
                break;
            default:
                break;
        }
    }
}
