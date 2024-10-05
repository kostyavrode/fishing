using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster instance;

    public int score1;
    public int score2;
    public int gameTime;
    private float wastedTime;
    public GameObject[] ruchki;
    private bool isWin;
    private void Awake()
    {
        instance=this;
    }
    private void Update()
    {
        if (GameManager.instance.state==GameStates.PLAYING)
        {
            wastedTime += Time.deltaTime;
            UIMaster.instance.ShowTime((gameTime - Mathf.Round(wastedTime)).ToString());

            if (wastedTime>=gameTime)
            {
                UIMaster.instance.EndGame(CheckResult());
            }
        }
    }
    public void CheckBuy()
    {
        int g = PlayerPrefs.GetInt("Buy");
        if (g>0)
        {
            foreach(GameObject go in ruchki)
            {
                go.SetActive(true);
            }
        }
    }
    public void ResetGame()
    {
        wastedTime = 0;
        score1 = 0;
        score2 = 0;
    }
    public void ScoreGoal(int type)
    {
        if (type == 1)
        {
            score1 += 1;
            UIMaster.instance.ShowPlayerScore(score1.ToString());
        }
        else if (type == 2)
        {
            score2 += 1;
            UIMaster.instance.ShowEnemyScore(score2.ToString());
        }
    }
    private bool CheckResult()
    {
        if (score1 < score2)
        {
            return false;
        }
        else if (score2 < score1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
