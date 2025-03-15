using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public GameObject gameOverPanel;
    // Start is called before the first frame update
    void Start()
    {
        gameOverPanel.SetActive(true);
        if(GameManager.Instance != null){
            int minutes = Mathf.FloorToInt(GameManager.Instance.timer / 60);
            int seconds = Mathf.FloorToInt(GameManager.Instance.timer % 60);
        
            scoreText.text = $"Time: {minutes:00}:{seconds:00}\nScore: {GameManager.Instance.totalScore}";
        }
        else
            {
                Debug.LogError("GameManager is not instantiated.");
            }
    }

    public void RestartGame()
    {
        if(GameManager.Instance != null){
            GameManager.Instance.RestartGame();
        }else
            {
                Debug.LogError("GameManager is not instantiated.");
            }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
