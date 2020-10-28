using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class NetworkManager : MonoBehaviour
{
    public bool isCone = false;
    public bool NetworkOnStart = true;
    public List<Graph> CurrentGraphs;
    public Dictionary<Node,int> NodeReference = new Dictionary<Node, int>();
    public NetworkObjects NetworkObjectContainer;
    public GameObject EdgeObject;
    public GameObject NodeObject;
    public string dataPath = "Assets/ResearchApplication/Data/klmnetwork.xml";

    float highestConnecitonCount = 132; //TODO: make variable
    float MinimumNodeSize = 0.25f;

    private static NetworkManager _instance;
    public  bool destroy;
    public bool loadAlternateNodeLayout;
    public string AlternateNodePath= "NetworksJson/";

    public static NetworkManager Instance { get { return _instance; } }
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        CurrentGraphs = loadData(dataPath);
        SetReference(CurrentGraphs);
        if (loadAlternateNodeLayout)
        {
            LoadAlternateLayout(AlternateNodePath);
        }
        if (NetworkOnStart)
        {
            NetworkObjectContainer = InstantiateNetwork(CurrentGraphs);
        }
        if (destroy)
        {
            DestroyNetwork();
        }
      
    }

    private void LoadAlternateLayout(string alternateNodePath)
    {
        string alternateNodePathJsonString = File.ReadAllText(alternateNodePath);
        int[] alternateLayout = JsonConvert.DeserializeObject<int[]>(alternateNodePathJsonString);
        List<Node> alternateLayoutNodes = new List<Node>();
        foreach (int nodeId in alternateLayout)
        {
            alternateLayoutNodes.Add(NodeReference.First(x => x.Value == nodeId).Key);
        }
        CurrentGraphs[0].nodes = alternateLayoutNodes;
    }

    private void SetReference(List<Graph> currentGraphs)
    {
        int counter = 0;
        foreach (Node node in currentGraphs[0].nodes)
        {
            NodeReference[node] = counter;
            counter++;
        }
    }

    public NetworkObjects InstantiateNetwork(List<Graph> graphs)
    {
        float lenghtCount = 0f;
        Vector3 postition = Vector3.one;
        Dictionary<Node, GameObject> nodeObjects = new Dictionary<Node, GameObject>();
        List<GameObject> networkObjects = new List<GameObject>();
        float counter = 0f;
        int connectionCounter = 0;
        float numberOfNodes = graphs[0].nodes.Count;
        foreach (Node node in graphs[0].nodes)
        {
            if (node.incomingEdges.Count + node.outgoingEdges.Count > connectionCounter)
            {
                connectionCounter = node.incomingEdges.Count + node.outgoingEdges.Count;
            }


            int numberConnections = node.incomingEdges.Count + node.outgoingEdges.Count;

            if (isCone)
            {
                postition = Vector3.Lerp(new Vector3(Mathf.Cos(counter / numberOfNodes * 360) * 30, (numberConnections / highestConnecitonCount) * 30, Mathf.Sin(counter / numberOfNodes * 360) * 30), new Vector3(0f, (numberConnections / highestConnecitonCount) * 30, 0f), numberConnections / highestConnecitonCount);
            }
            else
            {
                if (numberConnections / highestConnecitonCount == 1)
                {
                    postition = new Vector3(0f, (numberConnections / highestConnecitonCount) * 30, 0f);
                }
                else
                {
                    postition = new Vector3(Mathf.Cos(counter / numberOfNodes * 360) * 30, (numberConnections / highestConnecitonCount) * 30, Mathf.Sin(counter / numberOfNodes * 360) * 30);
                }
            }

            GameObject nodeObject = Instantiate(NodeObject, postition, Quaternion.identity);//node.configuration.position
            networkObjects.Add(nodeObject);
            nodeObject.gameObject.transform.localScale = nodeObject.gameObject.transform.localScale * ((numberConnections / highestConnecitonCount) + MinimumNodeSize);
            nodeObjects.Add(node, nodeObject);
            
            counter++;

        }
        //Debug.Log("number of connections" + connectionCounter);
        foreach (Edge edge in graphs[0].edges)
        {
            GameObject edgeObject = Instantiate(EdgeObject);
            networkObjects.Add(edgeObject);
            EdgeBehaviour edgeScript = edgeObject.GetComponent<EdgeBehaviour>();
            edgeScript.leftNode = nodeObjects[edge.startNode];
            edgeScript.rightNode = nodeObjects[edge.endNode];
            lenghtCount += Vector3.Distance(edgeScript.leftNode.transform.position, edgeScript.rightNode.transform.position);
        }
        NetworkObjects networkObjectContainer = new NetworkObjects();
        networkObjectContainer.networkObjects = networkObjects;
        networkObjectContainer.lenghtCount = lenghtCount;
        NetworkObjectContainer = networkObjectContainer;
        return networkObjectContainer;
    }

    public struct NetworkObjects
    {
        public List<GameObject> networkObjects;
        public float lenghtCount;
    }

    public void DestroyNetwork()
    {
        foreach (GameObject go in NetworkObjectContainer.networkObjects)
        {
            Destroy(go);
        }
        
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Graph> loadData(string path)
    {
        XmlDocument xmlDoc = new XmlDocument();
        List<Graph> graphList = new List<Graph>();

       StreamReader stream = new StreamReader(path);
        //TextAsset textAsset = ;
        //stream.Close();



        //TextAsset textAsset = (TextAsset)Resources.Load("", typeof(TextAsset));
        xmlDoc.LoadXml(stream.ReadToEnd());

        // parse data
        XmlElement root = xmlDoc.FirstChild as XmlElement;

        foreach (XmlElement xmlGraph in root.ChildNodes)
        {
            Dictionary<string, Node> nodeMap = new Dictionary<string, Node>();

            List<Node> nodes = new List<Node>();
            List<Edge> edges = new List<Edge>();

            // we make two passes here to ensure that all the nodes
            // have been parsed before any of the edges get parsed
            // because edges rely on node references.

            // first pass -> parse all nodes
            int index = 0;
            foreach (XmlElement xmlNode in xmlGraph.ChildNodes)
            {
                //create nodes
                if (xmlNode.Name == "node")
                {
                    var id = xmlNode.GetAttribute("Id");
                    var label = xmlNode.GetAttribute("Label");
                    var description = "Node " + index++;
                    var url = xmlNode.GetAttribute("link");
                    var category = xmlNode.GetAttribute("category");
                    int likeCount = int.Parse(xmlNode.GetAttribute("like_count"));
                    int talkingAboutCount = int.Parse(xmlNode.GetAttribute("talking_about_count"));
                    var usersCanPost = xmlNode.GetAttribute("users_can_post").ToLower().Equals("yes");

                    // NOTE: we totally ignore "In-Degree", "Out-Degree" and "Degree" here,
                    // because this values can be calculated automatically from the existing edges !

                    var data = new NodeData
                    {
                        id = id,
                        label = label,
                        description = description,
                        linkUrl = url,
                        category = category,
                        numLikes = likeCount,
                        talkingAboutCount = talkingAboutCount,
                        usersCanPost = usersCanPost,
                    };

                    // NOTE: we don't use the total degree here because total degree = in degree + out degree

                    var config = new NodeConfiguration
                    {
                        // TODO: change the configuration based on the node type
                        // we choose some random positions here
                        position = new Vector3(UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10), UnityEngine.Random.Range(-10, 10)),
                        force = new Vector3()
                    };

                    var node = new Node(data, config);
                    nodes.Add(node);
                    nodeMap.Add(id, node);
                }
            }

            // second pass -> parse all edges
            foreach (XmlElement xmlNode in xmlGraph.ChildNodes)
            {
                // create edges
                if (xmlNode.Name == "edge")
                {
                    var id = xmlNode.GetAttribute("Id");
                    var sourceId = xmlNode.GetAttribute("Source");
                    var targetId = xmlNode.GetAttribute("Target");

                    var source = nodeMap[sourceId];
                    var target = nodeMap[targetId];

                    if (source != null && target != null)
                    {
                        var edge = new Edge(source, target, id);
                        edges.Add(edge);

                        source.outgoingEdges.Add(edge);
                        target.incomingEdges.Add(edge);
                    }
                    else
                    {
                        Debug.Log("Error in xml structure! Source id: " + sourceId + ", Target id: " + targetId);
                    }
                }
            }

            // ref List<Node> nodes, ref List<Edge> edges
            Graph graph = new Graph(nodes, edges);

            graphList.Add(graph);
        }
        return graphList;
    }

    public class Graph
    {
        public Graph() { }
        
        private Dictionary<GameObject, Node> goNodeMap = new Dictionary<GameObject, Node>();

        public Graph(List<Node> nodes, List<Edge> edges)
        {
            this.nodes = nodes;
            this.edges = edges;
        }

        public List<Node> nodes { get; set; }

        public List<Edge> edges { get; }

        // TODO: we should do this somehow automatically
        public void buildGameobjectNodeMap()
        {
            foreach (var node in nodes)
                goNodeMap.Add(node.target, node);
        }

        public Node getNodeForGameObject(GameObject gameObject)
        {
            return goNodeMap.ContainsKey(gameObject) ? goNodeMap[gameObject] : null;
        }
    }

    public class Node
    {
        public Node() { }
        public Node(NodeData data, NodeConfiguration configuration)
        {
            this.data = data;
            this.configuration = configuration;
        }

        public GameObject target { get; set; }

        public NodeData data { get; }

        public NodeConfiguration configuration { get; }

        public List<Edge> incomingEdges { get; set; } = new List<Edge>();

        public List<Edge> outgoingEdges { get; } = new List<Edge>();

        public GameObject highlightTarget { get; set; }

        public bool highlighted
        {
            get { return highlightTarget != null && highlightTarget.activeInHierarchy; }
            set
            {
                if (highlightTarget != null) highlightTarget.SetActive(value);
            }
        }

        public int degree
        {
            get { return incomingEdges.Count + outgoingEdges.Count; }
        }
    }

    public class Edge
    {
        public Edge() { }
        private GameObject _target;

        public Edge(Node startNode, Node endNode, string id)
        {
            this.id = id;
            this.startNode = startNode;
            this.endNode = endNode;
        }

        public LineRenderer lineRenderer { get; private set; }

        public GameObject target
        {
            get { return _target; }
            set
            {
                _target = value;
                if (_target != null) lineRenderer = _target.GetComponent<LineRenderer>();
            }
        }

        public Node startNode { get; }

        public Node endNode { get; }

        public string id { get; }

        public override bool Equals(System.Object obj)
        {
            // If parameter is null return false.
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            Edge p = obj as Edge;
            if ((System.Object)p == null)
            {
                return false;
            }

            // Return true if the fields match:
            return Equals(p);
        }

        // TODO: remove
        public bool Equals(Edge other)
        {
            // If parameter is null return false:
            if ((object)other == null)
            {
                return false;
            }

            // Return true if the fields match:
            return (startNode == other.endNode) && (endNode == other.startNode)
                   || (startNode == other.startNode) && (endNode == other.endNode);
        }

        public override int GetHashCode()
        {
            return startNode.GetHashCode() | endNode.GetHashCode();
        }
    }

    public class NodeData
    {
        public NodeData() { }
        public string id;
        public string imageUrl;
        public string label;
        public string linkUrl;
        public string description; // not used atm - can be used for debugging
        public int numLikes;
        public string category; // we should change that to category id
        public bool usersCanPost;
        public int talkingAboutCount;
    }

    public class NodeConfiguration
    {
        private Color _color;
        private Vector3 _position;
        private Vector3 _force;

        public NodeConfiguration()
        {
            _force = new Vector3();
        }

        public bool isSelected { get; set; }

        public Color color
        {
            get { return _color; }
            set { _color = value; }
        }

        public float size { get; set; }

        public float mass { get; set; }

        public Vector3 position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vector3 force
        {
            get { return _force; }
            set { _force = value; }
        }
    }
}
