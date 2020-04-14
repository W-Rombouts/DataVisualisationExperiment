using UnityEngine;

public class DataStats : MonoBehaviour
{
    public static DataStats instance;
    private void Awake()
    {
        // if the singleton hasn't been initialized yet
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;//Avoid doing anything else
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }




    //In te future calculate this dynamicly
    public readonly int tempMin = -31;
    public readonly int tempMax = 396;
    public readonly float tempAverage = 127f;
    public readonly int rainMin = 0;
    public readonly int rainMax = 10;
    public readonly float rainAverage = 0.64f;
    public readonly int fosfaatMin = 0;
    public readonly int fosfaatMax = 19;
    public readonly float fosfaatAverage = 1.41f;
}
