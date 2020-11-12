using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using UnityEngine;
using System.Xml;
using System.Xml.Serialization;


public class NetworkEvolutionClustering : MonoBehaviour
{
    public bool isXML = true;
    NetworkManager networkManager;
    JsonSerializerSettings jsonSettings;
    List<NetworkManager.Node> randomisedList;
    //List<NetworkManager.Node> BestRandomisedList;
    //NetworkManager.NetworkObjects bestObjects;
    float BestLenghtCount = 0f;
    public bool evolve = false;
    // Start is called before the first frame update
    void Start()
    {
        Directory.CreateDirectory("NetworksJson/");
        Directory.CreateDirectory("NetworksXML/");

        networkManager = NetworkManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if (evolve)
        {
            RandomiseList();
            networkManager.InstantiateNetwork(networkManager.CurrentGraphs);
            CompareNetworkClusting();
            if (Time.realtimeSinceStartup < 3600)
            {
                networkManager.DestroyNetwork();
            }
            else
            {
                SaveNetwork();
                evolve = false;
            }
            
        }
    }

    private void RandomiseList()
    {
        randomisedList = new List<NetworkManager.Node>();
        while (networkManager.CurrentGraphs[0].nodes.Count!=0)
        {
            System.Random rng = new System.Random();
            
            int chosenNumber = rng.Next(1, networkManager.CurrentGraphs[0].nodes.Count)-1;
            //Debug.Log(chosenNumber);
            randomisedList.Add(networkManager.CurrentGraphs[0].nodes[chosenNumber]);
            networkManager.CurrentGraphs[0].nodes.RemoveAt(chosenNumber);
        }
        networkManager.CurrentGraphs[0].nodes = randomisedList;

    }

  

    private void CompareNetworkClusting() 
    {
        if (networkManager.NetworkObjectContainer.lenghtCount  < BestLenghtCount || BestLenghtCount == 0)
        {
            BestLenghtCount = networkManager.NetworkObjectContainer.lenghtCount;
            Debug.Log(BestLenghtCount);
            SaveNetwork();
        }
    }
    
    private void SaveNetwork()
    {
        int counter = 0;
        int[] NodeSequence = new int[randomisedList.Count];
        foreach (NetworkManager.Node node in randomisedList)
        {
            NodeSequence[counter] = networkManager.NodeReference[node];
            counter++;
        }


        if (isXML)
        {
            XmlSerializer serialiser = new XmlSerializer(typeof(int[]));
            FileStream stream = new FileStream("NetworksXML/" + (int)BestLenghtCount + "BestNodeSequence.xml", FileMode.Create);
            serialiser.Serialize(stream, NodeSequence);
            stream.Close();

        }
        else
        {
            string jsonGraphSaveData = JsonConvert.SerializeObject(NodeSequence);
            StreamWriter streamWriter = File.CreateText("NetworksJson/" + (int)BestLenghtCount + "BestNodeSequence.json");
            streamWriter.Write(jsonGraphSaveData);
            streamWriter.Close();
        }
    }



}
