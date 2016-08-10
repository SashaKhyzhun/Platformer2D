using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public Sprite soundOnGfx;
    public Sprite soundOffGfx;

    public AudioMixerSnapshot defaultSnapshot;
    public AudioMixerSnapshot musicOffSnapshot;
    public AudioMixerSnapshot pausedSnapshot;

    public float transitionTime = 0.1f;

    private bool musicState = true;
    private bool paused = false;

    void Start()
    {
        defaultSnapshot.TransitionTo(0f);

        if (musicState)
        {
            defaultSnapshot.TransitionTo(0f);
        }
        else
        {
            musicOffSnapshot.TransitionTo(0f);
        }
    }

    public void AudioUnpause()
    {
        if (musicState)
        {
            defaultSnapshot.TransitionTo(transitionTime * Time.timeScale);
        }
        else
        {
            musicOffSnapshot.TransitionTo(transitionTime * Time.timeScale);
        }
        paused = false;
    }

    public void AudioPause()
    {
        if (musicState)
        {
            pausedSnapshot.TransitionTo(transitionTime * Time.timeScale);
        }
        paused = true;
    }

    public void TurnOffAudio()
    {
        if (musicState)
        {
            musicOffSnapshot.TransitionTo(transitionTime * Time.timeScale);
            musicState = false;
        }
        else
        {
            if (paused)
            {
                pausedSnapshot.TransitionTo(transitionTime * Time.timeScale);
            }
            else
            {
                defaultSnapshot.TransitionTo(transitionTime * Time.timeScale);
            }
            musicState = true;
        }
    }

    public void ChangeImageColor(SoundButtonsHolder buttons)
    {
        if (!musicState) // if on, then turn off
        {
            foreach (GameObject b in buttons.soundButtons)
            {
                b.GetComponent<Image>().sprite = soundOffGfx;
            }
//            obj.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(200f / 255, 200f / 255, 200f / 255, 128f / 255);
//            obj.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color(200f / 255, 200f / 255, 200f / 255, 128f / 255);
        }
        else // else turn on
        {
            foreach (GameObject b in buttons.soundButtons)
            {
                b.GetComponent<Image>().sprite = soundOnGfx;
            }
//            obj.transform.GetChild(0).gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
//            obj.transform.GetChild(1).gameObject.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
        }
    }
}
