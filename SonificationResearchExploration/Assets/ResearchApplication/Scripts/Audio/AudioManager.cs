using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public int position = 0;
    public int samplerate = 44100;
    public float frequency = 262f;
    public SoundPattern soundPattern;
    public GameObject VisualCue;
    public AudioClip defaultClip;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
        defaultClip = MakeAudioClip(262f);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void SetAudioOnObject(GameObject GO, float duration = 1f, float volume = 0.5f)
    {
        AudioSource aud = GO.AddComponent<AudioSource>();
        aud.loop = true;
        aud.spatialize = true;
        aud.spatialBlend = 1f;
        aud.clip = defaultClip;
        aud.volume = volume;
        aud.Play();
        AudioStopTimer AST = GO.AddComponent<AudioStopTimer>();
        AST.AS = aud;
        AST.duration = duration;
    }
    public void SetAudioOnObject(GameObject GO, AudioClip myClip, float duration = 1f, float volume = 0.5f)
    {
        AudioSource aud = GO.AddComponent<AudioSource>();
        aud.loop = true;
        aud.spatialBlend = 1f;
        aud.spatialBlend = 1f;
        aud.clip = myClip;
        aud.volume = volume;
        aud.Play();
        AudioStopTimer AST = GO.AddComponent<AudioStopTimer>();
        AST.AS = aud;
        AST.duration = duration;
    }


    public void SetAudioOnLocation(Vector3 location, AudioClip myClip, float duration = 100f, float volume = 0.5f, bool isVisual = false)
    {
        GameObject GO;
        if (isVisual)
        {
            GO = Instantiate(VisualCue, location, Quaternion.identity);
        }
        else
        {
            GO = new GameObject();
        }

        GO.transform.position = location;
        AudioSource aud = GO.AddComponent<AudioSource>();
        aud.loop = true;
        aud.spatialBlend = 1f;
        aud.spatialBlend = 1f;
        aud.clip = myClip;
        aud.volume = volume;
        aud.Play();

        AudioStopTimer AST = GO.AddComponent<AudioStopTimer>();
        AST.AS = aud;
        AST.duration = duration;
        AST.isLocation = true;
    }
    public void SetAudioOnLocation(Vector3 location, float duration = 1f, float volume = 0.5f, bool isVisual = false)
    {
        GameObject GO;
        if (isVisual)
        {
            GO = Instantiate(VisualCue, location, Quaternion.identity);
        }
        else
        {
            GO = new GameObject();
        }

        GO.transform.position = location;
        AudioSource aud = GO.AddComponent<AudioSource>();
        aud.loop = true;
        aud.spatialBlend = 1f;
        aud.spatialBlend = 1f;
        aud.clip = defaultClip;
        aud.volume = volume;
        aud.Play();

        AudioStopTimer AST = GO.AddComponent<AudioStopTimer>();
        AST.AS = aud;
        AST.duration = duration;
        AST.isLocation = true;
    }

    public void SetAudioOnAngle(float angle, float radius, float duration = 1f, float volume = 0.5f, bool isVisual = false)
    {
        Vector3 postition = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
        GameObject GO;
        if (isVisual)
        {
            GO = Instantiate(VisualCue, postition, Quaternion.identity);
        }
        else
        {
            GO = new GameObject();
            GO.transform.position = postition;
        }


        AudioSource aud = GO.AddComponent<AudioSource>();
        aud.loop = true;
        aud.spatialBlend = 1f;
        aud.spatialBlend = 1f;
        aud.clip = defaultClip;
        aud.volume = volume;
        aud.Play();

        AudioStopTimer AST = GO.AddComponent<AudioStopTimer>();
        AST.AS = aud;
        AST.duration = duration;
        AST.isLocation = true;
    }
    public void SetAudioOnAngle(float angle, float radius, AudioClip myClip, float duration = 1f, float volume = 0.5f, bool isVisual = false)
    {
        Vector3 postition = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
        GameObject GO;
        if (isVisual)
        {
            GO = Instantiate(VisualCue, postition, Quaternion.identity);
        }
        else
        {
            GO = new GameObject();
            GO.transform.position = postition;
        }


        AudioSource aud = GO.AddComponent<AudioSource>();
        aud.loop = true;
        aud.spatialBlend = 1f;
        aud.spatialBlend = 1f;
        aud.clip = myClip;
        aud.volume = volume;
        aud.Play();

        AudioStopTimer AST = GO.AddComponent<AudioStopTimer>();
        AST.AS = aud;
        AST.duration = duration;
        AST.isLocation = true;
    }

    public AudioClip MakeAudioClip(float tone)
    {
        frequency = tone;
        AudioClip myClip = AudioClip.Create("MySinusoid", samplerate * 2, 1, samplerate, true, OnAudioRead, OnAudioSetPosition);
        return myClip;
    }


    void OnAudioRead(float[] data)
    {
        int count = 0;
        while (count < data.Length)
        {
            switch (soundPattern)
            {
                case SoundPattern.Sine:
                    data[count] = Mathf.Sin(2 * Mathf.PI * frequency * position / samplerate);
                    break;
                    //Sounpatterns Below have clicking artifacts
                    /*
                    case SoundPattern.Rechtangle:
                        data[count] = (Mathf.Repeat(count * frequency / samplerate, 1) > 0.5f) ? 1f : -1f;
                        break;
                    case SoundPattern.Sawtooth:
                        data[count] = Mathf.Repeat(count * frequency / samplerate, 1) * 2f - 1f;
                        break;
                    case SoundPattern.Triangle:
                        data[count] = Mathf.PingPong(count * 2f * frequency / samplerate, 1) * 2f - 1f;
                        break;
                    */
            }
            position++;
            count++;
        }
    }

    void OnAudioSetPosition(int newPosition)
    {
        position = newPosition;
    }

    public enum SoundPattern
    {
        Sine
        /*
        Artifacting in other types
        Rechtangle,
        Sawtooth,
        Triangle
        */
    }

    public class AudioStopTimer : MonoBehaviour
    {
        public float duration = 0f;
        public AudioSource AS;
        public bool isLocation = false;
        float counter = 0f;
        private void Update()
        {
            counter += Time.deltaTime;
            if (counter >= duration)
            {
                if (isLocation)
                {
                    Destroy(gameObject);
                }
                else
                {
                    Destroy(AS);
                    Destroy(this);
                }
            }

        }
    }



}
