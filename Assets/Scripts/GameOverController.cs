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
        if(GameManager.Instance){
            scoreText.text = "Score: " + GameManager.Instance.totalScore.ToString();
        }
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
