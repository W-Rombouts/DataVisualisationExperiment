using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LogThisObject : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ResearchDataCollector.Instance.trackedGameObject.Add(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
