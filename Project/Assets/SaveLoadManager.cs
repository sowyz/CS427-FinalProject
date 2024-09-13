using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    //Singleton
    public static SaveLoadManager Instance { get; set; }

    #region FIELDS

    #region SAVE
    private string HighScoreKey = "HighScore";
    #endregion

    #endregion

    #region METHODS
    private void Awake()
    {
        //Singleton
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this);
    }
    public void SaveHighScore(int score)
    {
        //Save High Score
        PlayerPrefs.SetInt(HighScoreKey, score);
    }

    public int LoadHighScore()
    {
        if (!PlayerPrefs.HasKey(HighScoreKey))
        {
            return 0;
        }

        //Load High Score
        return PlayerPrefs.GetInt(HighScoreKey);
    }
    #endregion
}
