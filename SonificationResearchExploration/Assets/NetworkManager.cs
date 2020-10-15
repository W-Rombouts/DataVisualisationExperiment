using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public class NetworkManager : MonoBehaviour
{
    public List<Graph> graphs;
    public Dictionary<Node,GameObject> NodeObjects;
    public GameObject EdgeObject;
    public GameObject NodeObject;
    public string path = "Assets/ResearchApplication/Data/klmnetwork.xml";


    float highestlikecount = 38964105; //TODO: make variable
    float MinimumNodeSize = 0.25f;
    // Start is called before the first frame update
    void Start()
    {

        NodeObjects = new Dictionary<Node, GameObject>();
        graphs = loadData();
        foreach (Node node in graphs[0].nodes)
        {
            
            GameObject nodeObject = Instantiate(NodeObject, node.configuration.position, Quaternion.identity);
            Debug.Log(node.data.numLikes / highestlikecount);
            nodeObject.gameObject.transform.localScale = nodeObject.gameObject.transform.localScale * ((node.data.numLikes / highestlikecount)+MinimumNodeSize);
            NodeObjects.Add(node,nodeObject);
        }

        foreach (Edge edge in graphs[0].edges)
        {
            GameObject edgeObject = Instantiate(EdgeObject);
            EdgeBehaviour edgeScript = edgeObject.GetComponent<EdgeBehaviour>();
            edgeScript.leftNode = NodeObjects[edge.startNode];
            edgeScript.rightNode = NodeObjects[edge.endNode];
        }



    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public List<Graph> loadData()
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
                        position = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), Random.Range(-10, 10)),
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
        private Dictionary<GameObject, Node> goNodeMap = new Dictionary<GameObject, Node>();

        public Graph(List<Node> nodes, List<Edge> edges)
        {
            this.nodes = nodes;
            this.edges = edges;
        }

        public List<Node> nodes { get; }

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
