using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fase1 : MonoBehaviour
{
    MainScript mainScript;
    NetworkManager networkManager;
    static List<int> referenceList = new List<int>() { 0, 45, 90, 135, 180, 225, 270, 315 };
    static List<int> referenceListNearMiss = new List<int>() { 15, 60, 105, 150, 165, 210, 255, 300 };
    List<GameObject> nodeListGo = new List<GameObject>();
    List<GameObject> nodeListGoNearMiss = new List<GameObject>();
    private bool isGenerated = false;
    private bool isAnswered = false;
    int counter = 0;
    private int fase= 0;
    // Start is called before the first frame update
    void Start()
    {
        mainScript = MainScript.Instance;
        networkManager = NetworkManager.Instance;
        int i = 0;
        foreach (var item in referenceList)
        {
            GameObject tempGo = Instantiate(networkManager.NodeObject, mainScript.GetPositionFromDegree(item), Quaternion.identity);
            GameObject tempGoNM = Instantiate(networkManager.NodeObject, mainScript.GetPositionFromDegree(referenceListNearMiss[i]), Quaternion.identity);
            tempGo.GetComponent<UpdateFlare>().nodeMesh.enabled = false;
            tempGoNM.GetComponent<UpdateFlare>().nodeMesh.enabled = false;
            nodeListGo.Add(tempGo);
            nodeListGoNearMiss.Add(tempGoNM);

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
  
        if (isAnswered)
        {
            fase = 1;
            if (nodeListGo.Count == counter)
            {
                counter = 0;
                nodeListGo.Shuffle();
                mainScript.UpdatePhase();
            }
            else
            {
                mainScript.SetSoundofOrb(nodeListGo[counter]);
            }
            isAnswered = false;
        }

        
    }

    public void DoSequenceFase2()
    {
        if (fase != 2)
        {
            fase = 2;
            enableNodeMeshes();
        }
        if (isAnswered)
        {
            
            if (nodeListGo.Count == counter)
            {
                counter = 0;
                nodeListGo.Shuffle();
                mainScript.UpdatePhase();
            }
            mainScript.SetSoundofOrb(nodeListGo[counter], isFlare: true); ;
            isAnswered = false;
        }


    }

    public void DoSequenceFase3()
    {
        if (fase != 3)
        {
            fase = 3;
            enableNodeMeshes();
        }
        if (isAnswered)
        {

            if (nodeListGo.Count == counter)
            {
                for (int i = 0; i < nodeListGo.Count; i++)
                {
                    Destroy(nodeListGo[i]);
                    Destroy(nodeListGoNearMiss[i]);
                }
                mainScript.UpdatePhase();
            }
            mainScript.SetSoundofOrb(nodeListGoNearMiss[counter]); ;
            isAnswered = false;
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

        //TODO: save answer

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
    private float diffrence;

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