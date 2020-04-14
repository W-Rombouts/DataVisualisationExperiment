using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static SoundsManager instance;
    AudioSource musicPlayer;
    public AudioClip BassLowest;
    public AudioClip Bass2;
    public AudioClip Bass3;
    public AudioClip Bass4;
    public AudioClip Bass5;
    public AudioClip Bass6;


    // Start is called before the first frame update
    void Start()
    {
        musicPlayer = gameObject.GetComponent<AudioSource>();
    }

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

    public void PlayNoteOnFosfaat(double fosfaat)
    {
        if (fosfaat < 0.5)
        {
            musicPlayer.clip = BassLowest;
        }
        else if (fosfaat < 1)
        {
            musicPlayer.clip = Bass2;
        }
        else if (fosfaat < 1.5)
        {
            musicPlayer.clip = Bass3;
        }
        else if (fosfaat < 2)
        {
            musicPlayer.clip = Bass4;
        }
        else if (fosfaat < 2.5)
        {
            musicPlayer.clip = Bass5;
        }
        else
        {
            musicPlayer.clip = Bass6;
        }
        musicPlayer.Play();
    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
