using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuPanel;
    private bool isPaused = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)){
            if(isPaused){
                ResumeGame();
            }
            else{
                PauseGame();
            }
        }
    }

    public void PauseGame(){
        //Show Pause Menu UI
        pauseMenuPanel.SetActive(true);
        //Freeze game time
        Time.timeScale = 0f;
        //(Optional) Freeze audio
        //Audiolistener.pause = true;
        isPaused = true;
    }

    public void ResumeGame(){
        //Show Pause Menu UI
        pauseMenuPanel.SetActive(false);
        //Unfreeze game time
        Time.timeScale = 1f;
        //(Optional) Unfreeze audio
        //Audiolistener.pause = false;
        isPaused = false;
    }

    public void QuitGame(){
        // If you're in the editor, this won't fully work,
        // but in a built application, this will quit the game.
        Application.Quit();
        // If you have a Main Menu scene, you might do:
        // SceneManager.LoadScene("MainMenu");
    }
}
