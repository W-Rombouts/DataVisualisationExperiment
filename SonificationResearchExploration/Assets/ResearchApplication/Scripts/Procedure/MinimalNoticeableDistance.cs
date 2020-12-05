using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class MinimalNoticeableDistance : MonoBehaviour
{
    public GameObject node;
    public bool isNoticeable = true;
    public bool isAnswered = false;
    private MainScript mainScript;
    private int leftPosition =90;
    private int rightPosition= 120;
    private List<GameObject> liveNodes;
    // Start is called before the first frame update
    void Start()
    {
        liveNodes = new List<GameObject>();
        mainScript = MainScript.Instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DoMNDSequence()
    {
        if (isAnswered)
        {
            isAnswered = false;
            DestroyNodes();
            if (isNoticeable)
            {
                leftPosition-=5;
                rightPosition-=5;
                SpawnNodes();
                
            }
            else
            {
                //TODO:Log distance
                float MND = Vector3.Distance(mainScript.GetPositionFromDegree(leftPosition), mainScript.GetPositionFromDegree(rightPosition));
                mainScript.UpdatePhase();
            }
        }
    }

    public void Answer(bool wasNoticable)
    {
        isAnswered = true;
        isNoticeable = wasNoticable;
    }

    private void SpawnNodes()
    {
        liveNodes.Add(Instantiate(node, mainScript.GetPositionFromDegree(leftPosition), Quaternion.identity));
        liveNodes.Add(Instantiate(node, mainScript.GetPositionFromDegree(leftPosition), Quaternion.identity));
    }

    private void DestroyNodes()
    {
        foreach (var item in liveNodes)
        {
            Destroy(item);
        }
    }



}
