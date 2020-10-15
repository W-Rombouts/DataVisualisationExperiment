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

public class QuestionairManager : MonoBehaviour
{
    #region SINGLETON PATTERN
    public static QuestionairManager _instance;
    public static QuestionairManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<QuestionairManager>();

                if (_instance == null)
                {
                    GameObject container = new GameObject("QuestionairManager");
                    _instance = container.AddComponent<QuestionairManager>();
                }
            }

            return _instance;
        }
    }
    #endregion


    public QuestionManager questionManager;
    public GameObject questionairRenderer;
    public Questionair questionair;
    public questionInformation Qinfo;
    private List<Question> questionList;
    public string QuestionairName;
    ResearchDataCollector RDC;
    private string questionairPath = "Assets/ResearchDataCollector/Questionairs/";
    private string answerPath = "Assets/ResearchDataCollector/Questionair/Answers/";
    public string filename;
    private bool questionairIsLoaded = false;
    private List<Question> questionSequence = new List<Question>();//Incase questions are asked out of sequence to be matched with answerList.
    private MeshRenderer background;
    public List<string> answerList = new List<string>();
    private List<float> questionTime=new List<float>();
    private List<float> answerTime = new List<float>();
    private int repeatOffset = 0;
    bool isQuestionAnswered = true;

    //Questionair Specifics
   

    // Start is called before the first frame update
    void Start()
    {
        RDC = ResearchDataCollector.Instance;
        filename = RDC.researchID.ToString() + RDC.subjectID.ToString();
        Directory.CreateDirectory(answerPath);
        background = gameObject.GetComponent<MeshRenderer>();
        if (File.Exists(questionairPath+QuestionairName))
        {
            StreamReader reader = new StreamReader(questionairPath+QuestionairName);
            string jsonQuestionair = reader.ReadToEnd();
            questionair = JsonConvert.DeserializeObject<Questionair>(jsonQuestionair);
            questionManager.GetComponent<TextMeshProUGUI>().text = questionair.Title;
            Qinfo = GetInformationFromQuestionair(questionair);
            questionList = Qinfo.questionDict.Values.ToList();
            questionairIsLoaded = true;
        }
    }

    public void AskQuestion(Question toBeAskedQuestion = null,bool isRepeatQuestion = false)
    {
        if (isQuestionAnswered) //Make sure no questions can be asked while a question is not answered.
        {
            isQuestionAnswered = false;
            if (isRepeatQuestion) { repeatOffset += 1; };
            background.enabled = true;
            if (questionairIsLoaded)
            {
                questionairRenderer.SetActive(true);
                if ( answerList.Count - repeatOffset >= questionList.Count)
                {
                    background.enabled = false;
                    questionairRenderer.SetActive(false);
                    SaveQuestionair();
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
                    questionManager.AskQuestion(question);
                }
            }
            else
            {
                Question error = new Question();
                error.question = "No Questionair Loaded";
                questionManager.AskQuestion(error);
            }
        }

       
    }


    public questionInformation GetInformationFromQuestionair(Questionair questionair) 
    {
        questionInformation tempQInfo = new questionInformation();
        tempQInfo.questionBlockDict = new Dictionary<string, QuestionBlock>();
        tempQInfo.questionDict = new Dictionary<string, Question>();

        foreach (var questionBlock in questionair.QuestionBlocks)
        {
            tempQInfo.questionBlockDict[questionBlock.SubTitle]=questionBlock;
            foreach (var question in questionBlock.Questions)
            {
                tempQInfo.questionDict[question.question]=question;
            }
        }
        return tempQInfo;
    }


    public void RecordAnswer(string answer)
    {
        answerTime.Add(Time.realtimeSinceStartup);
        answerList.Add(answer);
        isQuestionAnswered = true;
        questionairRenderer.SetActive(false);
    }

    public void RecordAnswer(AudioClip answer,float startime)
    {
       
        
        answerTime.Add(startime);
        answerList.Add(filename+ answerList.Count.ToString()+".mp3");
        EncodeMP3.convert(answer,answerPath+filename + answerList.Count.ToString(), 128);
        isQuestionAnswered = true;
        questionairRenderer.SetActive(false);
    }


    public void AskThisQuestion(string questionString)
    {
        AskQuestion(toBeAskedQuestion: Qinfo.questionDict[questionString]);
    }

    public void AskThisQuestion(string questionString,questionInformation localQinfo)
    {
        AskQuestion(toBeAskedQuestion: localQinfo.questionDict[questionString]);
    }

    public class questionInformation
    {
        public string QuestionairName;
        public Dictionary<string, QuestionBlock> questionBlockDict;
        public Dictionary<string,Question> questionDict;
    }


    public void SaveQuestionair()
    {
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
    }

}

class Answer
{
    public string question;
    public string answer;
    public float questionAskTime;
    public float questionAnswerTime;
}

class AnswerForm
{
    public int subjectID;
    public string researchID;
    public DateTime dateTime;
    public List<Answer> answers;
}