using UnityEngine;
using System.Collections;

public class EdgeBehaviour : MonoBehaviour
{

    public GameObject leftNode;
    public GameObject rightNode;

    private Transform leftTrans;
    private Transform rightTrans;

    private LineRenderer lineRenderer;

    private bool isInitiated = false;

    // Use this for initialization
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isInitiated && leftNode != null)
        {
            leftTrans = leftNode.transform;
            rightTrans = rightNode.transform;
            isInitiated = true;
        }

        if (isInitiated)
        {
            lineRenderer.SetPosition(0, leftTrans.position);
            lineRenderer.SetPosition(1, rightTrans.position);
        }

    }
}
