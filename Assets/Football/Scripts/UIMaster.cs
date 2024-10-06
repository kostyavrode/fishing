using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIMaster : MonoBehaviour
{
    public static UIMaster instance;
    public TMP_Text playerScore;
    public TMP_Text enemyScore;
    public TMP_Text timeBar;
    public TMP_Text resultText;
    public TMP_Text enemyNameBar;
    public TMP_Text finishScore;
    public TMP_Text finishMoney;
    public TMP_Text finishCups;

    public Image backToMenuButton;
    public Sprite restartButtonPrefab;

    public TMP_Text cupsText;
    public TMP_Text[] moneyBars;

    public TMP_Text taskText;

    public GameObject shopButton;
    public GameObject sellFishButton;

    public GameObject[] panels;

    //public Joystick joy;
    //public Joystick joy2;

    private void Awake()
    {
        instance = this;
        UpdateMoney();
        ShowCups();
    }
    public void UpdateMoney()
    {
        foreach(TMP_Text money in moneyBars)
        {
            money.text = PlayerPrefs.GetInt("Money").ToString();
        }
    }
    public void ShoTaskComplete(string i)
    {
        taskText.text = "catch 10 fish (" + i + "/10)";
    }
    public void ShowShopButton(bool state)
    {
        shopButton.SetActive(state);
    }
    public void ShowCups()
    {
        cupsText.text = PlayerPrefs.GetInt("Cups").ToString();
    }
    public void ShowTime(string t)
    {
        timeBar.text = string.Format("{0:d4}",t);
    }
    public void ShowEmenyName(string name)
    {
        enemyNameBar.text = name;
    }
    public void ShowPlayerScore(string s)
    {
        playerScore.text = s;
    }
    public void ShowEnemyScore(string s)
    {  
        enemyScore.text = s;
    }
    public void StartGame(int level)
    {
       // GameManager.instance.StartPlaying(level);
    }
    public void RestartGame()
    {
        GameManager.instance.Restart();
    }

    public void AddFish()
    {
        
        PlayerPrefs.SetInt("Cups", PlayerPrefs.GetInt("Cups") + 1);
        PlayerPrefs.Save();
        ShoTaskComplete(PlayerPrefs.GetInt("Cups").ToString());
        ShowCups();
    }

    public void ShowSellFishButton(bool state)
    {
        sellFishButton.SetActive(state);
    }
    public void SellFishButton()
    {
        if (PlayerPrefs.GetInt("Cups")>0)
        {
            PlayerPrefs.SetInt("Cups", PlayerPrefs.GetInt("Cups") - 1);
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money")+10);
            PlayerPrefs.Save();
            UpdateMoney();
            ShowCups();
        }
    }
    public void EndGame(bool isWin)
    {
        panels[1].SetActive(false);
        panels[2].SetActive(true);
        if (isWin)
        {
            resultText.text = "You Win!";
            if (PlayerPrefs.GetInt("Language")==1)
            {
                resultText.text = "você ganha";
            }
            finishMoney.text = "100";
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 100);
            finishCups.text = "1";
            PlayerPrefs.SetInt("Cups", PlayerPrefs.GetInt("Cups") + 1);
            
        }
        else
        {
            resultText.text = "You Lose!";
            if (PlayerPrefs.GetInt("Language") == 1)
            {
                resultText.text = "você perde";
            }
            finishMoney.text = "10";
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + 10);
            finishCups.text = "0";
            backToMenuButton.sprite = restartButtonPrefab;
        }
        finishScore.text = enemyScore.text + ":" + playerScore.text;
        GameManager.instance.EndGame();
    }
}
