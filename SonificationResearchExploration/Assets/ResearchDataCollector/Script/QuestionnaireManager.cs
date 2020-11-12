using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using UnityEditorInternal;
using TMPro;
using System.Linq;
using System;
using UnityEngine.Experimental.AI;

public class QuestionnaireManager : MonoBehaviour
{
    #region SINGLETON PATTERN
    public static QuestionnaireManager _instance;
    public static QuestionnaireManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<QuestionnaireManager>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("questionnaireManager");
                    _instance = container.AddComponent<QuestionnaireManager>();
                }
            }

            return _instance;
        }
    }
    #endregion


    public QuestionManager questionManager;
    public GameObject questionnaireRenderer;
    public Questionnaire questionnaire;
    public QuestionInformation Qinfo;
    private List<Question> questionList;
    public string questionnaireName;
    ResearchDataCollector RDC;
    private readonly string questionnairePath = "Assets/ResearchDataCollector/questionnaires/";
    private readonly string answerPath = "Assets/ResearchDataCollector/questionnaire/Answers/";
    public string filename;
    private bool questionnaireIsLoaded = false;
    private List<Question> questionSequence = new List<Question>();//Incase questions are asked out of sequence to be matched with answerList.
    private MeshRenderer background;
    public List<string> answerList = new List<string>();
    private List<float> questionTime=new List<float>();
    private List<float> answerTime = new List<float>();
    private int repeatOffset = 0;
    public bool isQuestionAnswered = true;
    private QuestionBlock block;

    //questionnaire Specifics


    // Start is called before the first frame update
    void Start()
    {
        //questionnaireRenderer = GameObject.Find("QuestionairRenderer");
        RDC = ResearchDataCollector.Instance;
        filename = RDC.researchID.ToString() + RDC.subjectID.ToString();
        Directory.CreateDirectory(answerPath);
        background = gameObject.GetComponent<MeshRenderer>();
        if (File.Exists(questionnairePath+questionnaireName))
        {
            StreamReader reader = new StreamReader(questionnairePath+questionnaireName);
            string jsonquestionnaire = reader.ReadToEnd();
            questionnaire = JsonConvert.DeserializeObject<Questionnaire>(jsonquestionnaire);
            questionManager.GetComponent<TextMeshProUGUI>().text = questionnaire.Title;
            Qinfo = GetInformationFromquestionnaire(questionnaire);
            questionList = Qinfo.questionDict.Values.ToList();
            questionnaireIsLoaded = true;
        }
    }

    public void AskQuestion(Question toBeAskedQuestion = null,bool isRepeatQuestion = false)
    {
        if (isQuestionAnswered) //Make sure no questions can be asked while a question is not answered.
        {
            isQuestionAnswered = false;
            if (isRepeatQuestion) { repeatOffset += 1; };
            background.enabled = true;
            if (questionnaireIsLoaded)
            {
                questionnaireRenderer.SetActive(true);
                if ( answerList.Count - repeatOffset >= questionList.Count)
                {
                    background.enabled = false;
                    questionnaireRenderer.SetActive(false);
                    SaveQuestionnaire();
                }
                else
                {
                    Question question;
                    if (toBeAskedQuestion == null)
                    {
                        question = questionList[answerList.Count];
                    }
                    else
                    {
                        question = toBeAskedQuestion;
                    }
                    questionTime.Add(Time.realtimeSinceStartup);
                    questionSequence.Add(question);
                    
                    foreach (var tempBlock in Qinfo.questionBlockDict.Values)
                    {
                        foreach (var blockQuestion in tempBlock.Questions)
                        {
                            if (blockQuestion == question)
                            {
                                block = tempBlock;
                            }
                        }
                    }
                    questionManager.AskQuestion(question, block);
                }
            }
            else
            {
                Question error = new Question
                {
                    question = "No questionnaire Loaded"
                };
                questionManager.AskQuestion(error,new QuestionBlock());
            }
        }

       
    }


    public QuestionInformation GetInformationFromquestionnaire(Questionnaire questionnaire) 
    {
        QuestionInformation tempQInfo = new QuestionInformation
        {
            questionBlockDict = new Dictionary<List<string>, QuestionBlock>(),
            questionDict = new Dictionary<string, Question>()
        };

        foreach (var questionBlock in questionnaire.QuestionBlocks)
        {
            List<string> questionList = new List<string>();
            foreach (var question in questionBlock.Questions)
            {
                tempQInfo.questionDict[question.question]=question;
                questionList.Add(question.question);
            }
            tempQInfo.questionBlockDict[questionList] = questionBlock;
        }
        return tempQInfo;
    }


    public void RecordAnswer(string answer)
    {
        answerTime.Add(Time.realtimeSinceStartup);
        answerList.Add(answer);
        isQuestionAnswered = true;
        questionnaireRenderer.SetActive(false);
    }

    public void RecordAnswer(AudioClip answer,float startime)
    {
        answerTime.Add(startime);
        answerList.Add(filename+ answerList.Count.ToString()+".mp3");
        EncodeMP3.convert(answer,answerPath+filename + answerList.Count.ToString(), 128);
        isQuestionAnswered = true;
        questionnaireRenderer.SetActive(false);
    }


    public void AskThisQuestion(string questionString)
    {
        AskQuestion(toBeAskedQuestion: Qinfo.questionDict[questionString]);
    }

    public void AskThisQuestion(string questionString,QuestionInformation localQinfo)
    {
        AskQuestion(toBeAskedQuestion: localQinfo.questionDict[questionString]);
    }

    public class QuestionInformation
    {
        public string questionnaireName;
        public Dictionary<List<string>, QuestionBlock> questionBlockDict;
        public Dictionary<string,Question> questionDict;
    }


    public void SaveQuestionnaire()
    {
        float starttime = Time.realtimeSinceStartup;
        AnswerForm form = new AnswerForm
        {
            dateTime = DateTime.Now,
            researchID = ResearchDataCollector.Instance.researchID,
            subjectID = ResearchDataCollector.Instance.subjectID
        };
        form.answers = new List<Answer>();
        for (int i = 0; i < questionSequence.Count; i++)
        {
            Answer answ = new Answer
            {
                question = questionSequence[i].question,
                answer = answerList[i],
                questionAskTime = questionTime[i],
                questionAnswerTime = answerTime[i]
            };
            form.answers.Add(answ);
        }
        File.WriteAllText(answerPath+ DateTime.Now.ToString("yyyyMMdd") + filename+".json", JsonConvert.SerializeObject(form));
        Debug.Log("Time Lost Saving:" + (Time.realtimeSinceStartup - starttime).ToString());
    }

}

class AnswerForm
{
    public int subjectID;
    public string researchID;
    public DateTime dateTime;
    public List<Answer> answers;
}

class Answer
{
    public string question;
    public string answer;
    public float questionAskTime;
    public float questionAnswerTime;
}

