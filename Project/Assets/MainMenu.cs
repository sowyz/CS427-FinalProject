using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    string newGameScene = "SampleScene";
    public TMP_Text highScoreText;

    public AudioClip backgroundMusic;
    public AudioSource audioSource;

    private void Start()
    {
        // Unlock cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        audioSource.clip = backgroundMusic;
        audioSource.Play();
        highScoreText.text = "Highest Wave Survived: " + SaveLoadManager.Instance.LoadHighScore();
    }

    public void StartNewGame()
    {
        audioSource.Stop();
        UnityEngine.SceneManagement.SceneManager.LoadScene(newGameScene);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
