using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PreloadedLayout : MonoBehaviour
{
    public bool loadpreloadedLayout;
    public bool isLoaded = false;
    public List<GameObject> LockedNodesList;

    NetworkManager networkManager;
    GameObject node;
    GameObject edge;
    int counter = 0;
    // Start is called before the first frame update
    void Start()
    {
        List<GameObject> LockedNodesList = new List<GameObject>();
        networkManager = NetworkManager.Instance;
        node = networkManager.NodeObject;
        edge = networkManager.EdgeObject;
        if (loadpreloadedLayout)
        {
            networkManager.NetworkOnStart = false;
            networkManager.DestroyNetwork();
            networkManager.isApplyForce = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!isLoaded && loadpreloadedLayout)
        {
            LoadNetworkObjectContainer();
            isLoaded = true;
            Debug.Log(counter);
        }
    }


    public void LoadNetworkObjectContainer()
    {
        StreamReader reader = new StreamReader("Assets/ResearchApplication/Data/layout.json");
        string jsonString = reader.ReadToEnd();
        NetworkManager.JsonLayoutExport savedLayout = JsonConvert.DeserializeObject<NetworkManager.JsonLayoutExport>(jsonString);
        Dictionary<Vector3, GameObject> createdNodeDict = new Dictionary<Vector3, GameObject>();
        Dictionary<Vector3, Vector3> scalePerPosition = new Dictionary<Vector3, Vector3>();  
        for (int i = 0; i < savedLayout.posSequence.Count; i++)
        {
            scalePerPosition[savedLayout.posSequence[i]] = savedLayout.scaleSequence[i];
        }
        for (int i = 0; i < savedLayout.edgeStartPosition.Count; i++)
        {
            Vector3 startPosNode = savedLayout.edgeStartPosition[i];
            Vector3 endPosNode = savedLayout.edgeEndPosition[i];
            GameObject endNode;
            GameObject startNode;
            if (createdNodeDict.ContainsKey(startPosNode))
            {
                startNode = createdNodeDict[startPosNode];
            }
            else
            {
                startNode = Instantiate(node, startPosNode, Quaternion.identity);
                startNode.transform.localScale = scalePerPosition[startPosNode];
                createdNodeDict[startPosNode] = startNode;
                counter++;
            }
            
            if (createdNodeDict.ContainsKey(endPosNode))
            {
                endNode = createdNodeDict[endPosNode];
            }
            else
            {
                endNode = Instantiate(node, endPosNode, Quaternion.identity);
                endNode.transform.localScale = scalePerPosition[endPosNode];
                createdNodeDict[endPosNode] = endNode;
                counter++;
            }
            GameObject edgeObject = Instantiate(edge);
            
            EdgeBehaviour edgeScript = edgeObject.GetComponent<EdgeBehaviour>();
            edgeScript.leftNode = startNode;
            edgeScript.rightNode = endNode;

        }
        foreach (Vector3 lockedPos in savedLayout.LockedLocations)
        {
            LockedNodesList.Add(createdNodeDict[lockedPos]);
        }
    }
}
