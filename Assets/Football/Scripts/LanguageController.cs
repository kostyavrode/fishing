using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LanguageController : MonoBehaviour
{
    public GameObject chooseLanguageObject;
    public TMP_Text[] texts;
    public string[] portugals;
    public string[] english;
    private void Awake()
    {
        if (PlayerPrefs.HasKey("Language"))
        {
            chooseLanguageObject.SetActive(false);
        }
        if (PlayerPrefs.GetInt("Language")==1)
        {
            ChangeLanguage(1);
        }
    }
    public void ChangeLanguage(int languageType)
    {
        if (languageType == 0)
        {
            //for (int i = 0; i < texts.Length; i++)
            //{
            //   texts[i].text = english[i];
            //}
            PlayerPrefs.SetInt("Language", 0);
        }
        else
        {
            for (int i = 0; i < texts.Length; i++)
            {
                texts[i].text = portugals[i];
                PlayerPrefs.SetInt("Language", 1);
                PlayerPrefs.Save();
            }
        }
    }
}
