using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public enum GameStates
{
    MENU,
    PLAYING,
    PAUSE,
    ENDED,
    RESTART
}
public class GameManager : MonoBehaviour
{
    public GameMaster[] levelPrefabs;
    public static GameManager instance;
    public GameStates state;
    private GameMaster currentLevel;
    private void Awake()
    {
        instance = this;
    }
    public void BackToMenu()
    {
        if (currentLevel!=null)
        {
            Destroy(currentLevel.gameObject);
            currentLevel = null;
        }
        state = GameStates.MENU;
    }
    public void StartPlaying()
    {
        state = GameStates.PLAYING;
    }
    public void Pause()
    {
        state = GameStates.PAUSE;
    }
    public void EndGame()
    {

    state = GameStates.ENDED;
    }
    public void Restart()
    {

    state = GameStates.RESTART;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
