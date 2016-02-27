using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour {

    public AudioMixerSnapshot defaultSnapshot;
    public AudioMixerSnapshot musicOffSnapshot;
    public AudioMixerSnapshot pausedSnapshot;

    public float transitionTime = 0.1f;

    private bool musicState = true;

    void Start ()
    {
        defaultSnapshot.TransitionTo(0f);
    }

    public void AudioUnpause()
    {
        if (musicState) { defaultSnapshot.TransitionTo(transitionTime * Time.timeScale); }
        else { musicOffSnapshot.TransitionTo(transitionTime * Time.timeScale); }
    }

    public void AudioPause()
    {
        if (musicState) { pausedSnapshot.TransitionTo(transitionTime * Time.timeScale); }
    }

    public void TurnOffAudio()
    {
        if (musicState)
        {
            musicOffSnapshot.TransitionTo(transitionTime);
            musicState = false;
        }
        else
        {
            defaultSnapshot.TransitionTo(transitionTime);
            musicState = true;
        }
    }

    public void ChangeImageColor(GameObject obj)
    {
        if (!musicState) // if on, then turn off
        {
            obj.GetComponent<Image>().color = new Color(200f / 255, 200f / 255, 200f / 255, 128f / 255);
        }
        else // else turn on
        {
            obj.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
