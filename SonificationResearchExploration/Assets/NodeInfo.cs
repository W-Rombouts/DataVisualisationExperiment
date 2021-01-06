using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeInfo : MonoBehaviour
{
    public List<GameObject> connectedNodes = new List<GameObject>();
    public IEnumerable<GameObject> discNodes;
    public Vector3 position;
    public Vector3 velocity;
    public bool localDisableApplyForce = false;
    public bool isDebugRun = false;
    //public bool isBias = true;
    private NetworkManager networkManager;
    private bool isListChecked= false;
    private MainScript mainScript;
    public float radius = 20f;
  

    float counter = 0f;
    // Start is called before the first frame update
    void Start()
    {
        mainScript = MainScript.Instance;
        networkManager = NetworkManager.Instance;
        if (networkManager != null)
        {
            isDebugRun = networkManager.isDebugRun;
        }
        isDebugRun = networkManager.isDebugRun;
        if (mainScript.phase == 6 || isDebugRun)
        {
            networkManager.AddNodeToRanking(gameObject, connectedNodes.Count);

        }

    }

    // Update is called once per frame
    void Update()
    {
        if (networkManager != null)
        {
            isDebugRun = networkManager.isDebugRun;
        }
        else
        {
            networkManager = NetworkManager.Instance;
        }
        
        if (mainScript.phase == 6 || isDebugRun)
        {
            if (!isListChecked && networkManager.sortedList.Contains(gameObject))
            {
                SetLocalDisable(true);

                float i = networkManager.sortedList.IndexOf(gameObject);
                if (i == 0)
                {
                    transform.position = new Vector3(0f, radius, 0f);
                }
                else
                {
                    float degree = (i / (networkManager.sortedList.Count - 1) * 360) * (3.14f / 180);

                    transform.position = new Vector3(Mathf.Cos(degree) * radius, 0f, Mathf.Sin(degree) * radius);
                    position = gameObject.transform.position;
                    //Debug.Log("degree:" + degree + " position" + position.x + "," + position.y + "," + position.z);
                }

                isListChecked = true;
            }
            else
            {

                isListChecked = true;
            }
            if (networkManager.isApplyForce && !localDisableApplyForce)
            {
                velocity = velocity * 0.85f;
                position += velocity * Time.deltaTime;
                gameObject.transform.position = position;

            }
        }

    }
    public void SetLocalDisable(bool setToState)
    {
        localDisableApplyForce = setToState;
    }
}
