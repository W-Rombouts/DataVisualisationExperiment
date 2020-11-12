using UnityEngine;
using System.Collections;

public class SoundGen : MonoBehaviour
{
    public int position = 0;
    public int samplerate = 44100;
    public float frequency = 262f;
    public SoundPattern soundPattern;
    public GameObject VisualCue;
    public AudioClip defaultClip;

    public AudioClip MakeAudioClip(float tone = 262f)
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