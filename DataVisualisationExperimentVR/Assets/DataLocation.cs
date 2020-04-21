using UnityEngine;

public class DataLocation : MonoBehaviour
{
    public bool SkipSelect;
    // Start is called before the first frame update
    void Start()
    {
        if (SkipSelect)
        {
            DataContainer.instance.GetByYear();
            gameObject.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
