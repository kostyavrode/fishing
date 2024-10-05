using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Settings : MonoBehaviour
{
    private AudioSource audioSource;

    public GameObject sourcePrefab;

    public TMP_Text soundText;
    public TMP_Text vibroText;

    private bool isSoundEnabled;
    private bool isVibroEnabled;
    private void OnEnable()
    {
        CheckSource();
        CheckSettings();
    }
    public void SoundButton()
    {
        if (isSoundEnabled)
        {
            ChangeStatusSound(true);
        }
        else
        {
            ChangeStatusSound(false);
        }
    }
    public void VibroButton()
    {
        if (isVibroEnabled)
        {
            ChangeStatusVibro(false);
        }
        else
        {
            ChangeStatusVibro(true);
        }
    }
    private void CheckSettings()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            ChangeStatusSound(true);
        }
        else
        {
            ChangeStatusSound(false);
        }

        if (PlayerPrefs.GetInt("Vibro")==1)
        {
ChangeStatusVibro(true);
        }
        else
        {
ChangeStatusVibro(false);
        }
    }
    private void CheckSource()
    {
        if (audioSource == null)
        {
            try
            {
                audioSource = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioSource>();
                DontDestroyOnLoad(audioSource);
            }
            catch
            {
                audioSource = Instantiate(sourcePrefab).GetComponent<AudioSource>();
            }
        }
    }
    private void ChangeStatusSound(bool f)
    {
        if (f)
        {
            audioSource.volume = 0;
            soundText.text = "Off";
            isSoundEnabled = false;
            if (PlayerPrefs.GetInt("Language") == 1)
            {
                soundText.text = "Não";
            }
            PlayerPrefs.SetInt("Sound", 1);
        }
        else
        {
            audioSource.volume = 100;
            soundText.text = "On";
            if (PlayerPrefs.GetInt("Language") == 1)
            {
                soundText.text = "Sim";
            }
            isSoundEnabled = true;
            PlayerPrefs.SetInt("Sound", 0);
        }
    }
    private void ChangeStatusVibro(bool f)
    {
        if (f)
        {
            isVibroEnabled = true;
            vibroText.text = "On";
            if (PlayerPrefs.GetInt("Language") == 1)
            {
                vibroText.text = "Sim";
            }
                PlayerPrefs.SetInt("Vibro", 1);
        }
        else
        {
            isVibroEnabled = false;
            vibroText.text = "Off";
            if (PlayerPrefs.GetInt("Language") == 1)
            {
                vibroText.text = "Não";
            }
            PlayerPrefs.SetInt("Vibro", 0);
        }
    }
    public void ExitButton()
    {
        Application.Quit();
    }
}
