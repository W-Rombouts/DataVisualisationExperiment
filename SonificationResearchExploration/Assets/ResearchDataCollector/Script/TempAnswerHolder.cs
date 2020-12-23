using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using Valve.VR;

public class TempAnswerHolder : MonoBehaviour
{
    public string answer;
    private float audioStart;
    private AudioClip AudioAnswer;
    public GameObject[] Questiontypes;
    public TextMeshProUGUI SubQuestion;
    public GameObject QuestionnaireRenderer;
    public MeshRenderer questionnaireMeshRenderer;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetAnswer(string answ)
    {
        answer = answ;
    }

    public void SetAnswer(AudioClip audioAnswer,float startAnswer)
    {
        AudioAnswer = audioAnswer;
        audioStart = startAnswer;

    }

    public void SaveAnswer()
    {
        if (AudioAnswer == null)
        {
            QuestionnaireManager.Instance.RecordAnswer(answer);
        }
        else
        {
            QuestionnaireManager.Instance.RecordAnswer(AudioAnswer, audioStart);
        }
        ResetForNextQuestion();
    }
    public void SetAnswer(TextMeshProUGUI textMesh)
    {
        answer = textMesh.text;
    }

    private void ResetForNextQuestion()
    {
        foreach (var gameobject in Questiontypes)
        {
            gameobject.SetActive(false);
        }
        SubQuestion.text = "";
        QuestionnaireRenderer.SetActive(false);
        questionnaireMeshRenderer.enabled = false;
    }

}
