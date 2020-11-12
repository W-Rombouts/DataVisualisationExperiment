using System.Collections;
using System.Collections.Generic;
using System.IO.Pipes;
using TMPro;
using UnityEngine;

public class QuestionManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI questionBlockText;
    public GameObject likertScale;
    public GameObject numberScale;
    public GameObject multiChoice;
    public GameObject numberPad;
    public GameObject openQuestion;


    public void AskQuestion(Question question,QuestionBlock questionBlock)
    {
        questionText.text = question.question;
        questionBlockText.text = questionBlock.SubTitle;

        switch (question.questionType)
        {
            case QuestionType.Likert:
                likertScale.SetActive(true);
                likertScale.GetComponent<LikertManager>().SetLikertSize(question.likertLowestText,question.likertHighestText,questionBlock.BlockQuestion,question.likertIs5);

                break;
            case QuestionType.NumberScale:
                numberScale.SetActive(true);
                numberScale.GetComponent<ScaleManager>().MaxScale = question.numberScaleMax;
                numberScale.GetComponent<ScaleManager>().MinScale = question.numberScaleMin;
                break;
            case QuestionType.MultiChoice:
                multiChoice.SetActive(true);
                multiChoice.GetComponent<MultiChoiceManager>().SetAnswers(question.multiAnswers);
                break;
            case QuestionType.NumberPad:
                numberPad.SetActive(true);
                numberPad.GetComponent<NumberPadManager>().DefaultValue = question.numPadDefaultValue;
                break;
            case QuestionType.OpenQuestion:
                openQuestion.SetActive(true);
                break;
            default:
                break;
        }
    }
}
