using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    void Start()
    {
        if (scoreText == null)
        {
            scoreText = GetComponent<TextMeshProUGUI>(); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance != null){
            scoreText.text = "Score: " + GameManager.Instance.currentLevelScore.ToString();
        }
    }
}
