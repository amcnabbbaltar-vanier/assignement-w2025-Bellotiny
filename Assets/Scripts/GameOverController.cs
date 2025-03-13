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
        int minutes = Mathf.FloorToInt(GameManager.Instance.timer / 60);
        int seconds = Mathf.FloorToInt(GameManager.Instance.timer % 60);
    
        scoreText.text = $"Time: {minutes:00}:{seconds:00}\nScore: {GameManager.Instance.totalScore}";
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Level1Scene");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
