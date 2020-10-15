using UnityEngine;
using System.Collections;

public class SoundGen : MonoBehaviour
{
    public int position = 0;
    public int samplerate = 44100;
    public float frequency = 220;
    public GameObject audioSource;
    public SoundPattern soundPattern;

    void Start()
    {
        AudioClip myClip = AudioClip.Create("MySinusoid", samplerate * 2, 1, samplerate, true, OnAudioRead, OnAudioSetPosition);
        AudioSource aud = audioSource.gameObject.AddComponent<AudioSource>();
        aud.loop = true;
        aud.spatialBlend = 1f;
        aud.clip = myClip;
        aud.Play();
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
                case SoundPattern.Rechtangle:
                    data[count] = (Mathf.Repeat(count * frequency / samplerate, 1) > 0.5f) ? 1f : -1f;
                    break;
                case SoundPattern.Sawtooth:
                    data[count] = Mathf.Repeat(count * frequency / samplerate, 1) * 2f - 1f;
                    break;
                case SoundPattern.Triangle:
                    data[count] = Mathf.PingPong(count * 2f * frequency / samplerate, 1) * 2f - 1f;
                    break;

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
        Sine,
        Rechtangle,
        Sawtooth,
        Triangle
    }
}