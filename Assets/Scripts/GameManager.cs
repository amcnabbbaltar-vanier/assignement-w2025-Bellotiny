using UnityEngine;
using UnityEngine.SceneManagement; // Add this namespace

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [SerializeField] private GameObject pauseMenuPanel;
    private bool isPaused = false;
    public int score = 0;
    //public int targetScore = 4; // Score to reach before changing scenes
    public float timer = 0.0f;
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Optional
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void IncrementScore(int pickUpPoints)
    {
        score += pickUpPoints;
        Debug.Log("Score: " + score);

        // if (score >= targetScore)
        // {
        //     LoadNextScene();
        // }
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); // Use your new scene's name
    }

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
