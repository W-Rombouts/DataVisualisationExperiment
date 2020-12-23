using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fase1 : MonoBehaviour
{
    public MainScript mainScript;
    NetworkManager networkManager;
    public GameObject NodeObject;
    public GameObject Pointer;
    static List<int> referenceList = new List<int>() { 0, 45, 90, 135, 180, 225, 270, 315 };
    //static List<int> referenceListNearMiss = new List<int>() { 15, 60, 105, 150, 165, 210, 255, 300 };
    List<GameObject> nodeListGo = new List<GameObject>();
    List<GameObject> nodeListGoNearMiss = new List<GameObject>();
    private bool isGenerated = false;
    private bool isAnswered = true;
    private float askedTime;
    int counter = 0;
    public int fase= 0;


    #region SINGLETON PATTERN
    public static Fase1 Instance { get; private set; }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion




    // Start is called before the first frame update
    void Start()
    {
        networkManager = NetworkManager.Instance;
        //int i = 0;
        foreach (var item in referenceList)
        {
            GameObject tempGo = Instantiate(NodeObject, mainScript.GetPositionFromDegree(item), Quaternion.identity);
            GameObject tempGoNM = Instantiate(NodeObject, mainScript.GetPositionFromDegree(item+15), Quaternion.identity);
            tempGo.GetComponent<UpdateFlare>().nodeMesh.enabled = false;
            tempGoNM.GetComponent<UpdateFlare>().nodeMesh.enabled = false;
            nodeListGo.Add(tempGo);
            nodeListGoNearMiss.Add(tempGoNM);
            mainScript.phasAnswerSync = true;

        }
        nodeListGo.Shuffle();
        nodeListGoNearMiss.Shuffle();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void DoSequenceFase1()
    {
        if (fase != 1)
        {
            QuestionnaireManager.Instance.AskQuestion();
            fase = 1;
        }
  
        if (!mainScript.phasAnswerSync&& isAnswered)
        {

            if (nodeListGo.Count-1 == counter)
            {
                counter = 0;
                nodeListGo.Shuffle();
                mainScript.phasAnswerSync = true;
                mainScript.UpdatePhase(true);
                Pointer.SetActive(false);
            }
            else
            {
                mainScript.SetSoundofOrb(nodeListGo[counter]);
                counter++;
                isAnswered = false;
                Pointer.SetActive(true);
            }
            
            askedTime = Time.realtimeSinceStartup;
        }
    }

    public void DoSequenceFase2()
    {
        if (fase != 2)
        {
            mainScript.phasAnswerSync = true;
            QuestionnaireManager.Instance.AskQuestion();
            fase = 2;
            enableNodeMeshes();


        }
        if (!mainScript.phasAnswerSync && isAnswered)
        {
            
            if (nodeListGo.Count-1 == counter)
            {
                counter = 0;
                nodeListGo.Shuffle();
                mainScript.UpdatePhase(true);
                mainScript.phasAnswerSync = true;
                Pointer.SetActive(false);

            }
            else
            {
                Pointer.SetActive(true);
                mainScript.SetSoundofOrb(nodeListGo[counter], isFlare: true);
                isAnswered = false;
                counter++;
            }
           
            
            askedTime = Time.realtimeSinceStartup;
        }


    }

    public void DoSequenceFase3()
    {

        if (fase != 3)
        {
            mainScript.phasAnswerSync = true;
            QuestionnaireManager.Instance.AskQuestion();
            fase = 3;
            enableNodeMeshes();
        }
        if (!mainScript.phasAnswerSync && isAnswered)
        {

            if (nodeListGo.Count-1 == counter)
            {
                Pointer.SetActive(false);
                for (int i = 0; i < nodeListGo.Count; i++)
                {
                    Destroy(nodeListGo[i]);
                    Destroy(nodeListGoNearMiss[i]);
                }
                mainScript.UpdatePhase(true);
                mainScript.phasAnswerSync = true;
                fase = 0;

            }
            else
            {
                Pointer.SetActive(true);
                mainScript.SetSoundofOrb(nodeListGoNearMiss[counter]);
                isAnswered = false;
                counter++;
            }
            
            
            askedTime = Time.realtimeSinceStartup;
        }
    }

    public void AnswerFase(Vector3 selectedPosition)
    {
        phaseAnswer newAnswer = new phaseAnswer
        {
            faseNR = fase,
            selectedLocation = selectedPosition,
            ActualLocation = nodeListGo[counter].transform.position
        };
        newAnswer.CalcDiffrence();

        Answer anws = new Answer()
        {
            question = "What is the error in condition " + newAnswer.faseNR + "?",
            answer = newAnswer.diffrence.ToString(),
            questionAnswerTime = Time.realtimeSinceStartup,
            questionAskTime = askedTime
        };
        QuestionnaireManager.Instance.LogNonQuestionnaireAnswer(anws);
        isAnswered = true;

    }

    public void enableNodeMeshes()
    {
        foreach (var nodeGO in nodeListGo)
        {
            nodeGO.GetComponent<UpdateFlare>().nodeMesh.enabled = true;
        }
    }





}

public class phaseAnswer
{
    public int faseNR;
    public Vector3 selectedLocation;
    public Vector3 ActualLocation;
    public float diffrence;

    public void CalcDiffrence()
    {
        diffrence =  Vector3.Distance(selectedLocation, ActualLocation);
    }

}

public static class ListShuffle
{

    public static void Shuffle<T>(this IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }
}