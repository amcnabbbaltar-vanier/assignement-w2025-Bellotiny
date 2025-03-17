using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject pauseMenuPanel;
    private bool isPaused = false;
    //public TextMeshProUGUI currentScoreText;
    public int totalScore = 0;
    public int currentLevelScore = 0;
    private bool gameRunning = false;
    public float timer = 0.0f;
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("Game Manager is Created!!");
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Game Manager is Destroyed!!");
        }
    }
    void Start()
    {
        //Debug.Log("PauseMenuPanel State: " + (pauseMenuPanel != null ? "Exists" : "Null"));
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);  // Hide at the start
        }
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if a new PauseMenuPanel exists in the new scene
        GameObject newPanel = GameObject.Find("PauseMenuPanel");

        if (newPanel != null && newPanel != pauseMenuPanel)
        {
            //Debug.Log("New PauseMenuPanel found in the scene! Replacing the old one.");
            
            // Destroy the old persistent panel
            Destroy(pauseMenuPanel);

            // Assign the new panel and ensure it is hidden
            pauseMenuPanel = newPanel;
            pauseMenuPanel.SetActive(false);

            RectTransform rectTransform = pauseMenuPanel.GetComponent<RectTransform>();

            // Set the position to the center of the screen
            rectTransform.anchorMin = new Vector2(0.5f, 0.5f);  // Set anchors to center
            rectTransform.anchorMax = new Vector2(0.5f, 0.5f);  // Set anchors to center
            rectTransform.pivot = new Vector2(0.5f, 0.5f);      // Set pivot to center

            // Set the position of the panel to be in the middle of the screen
            rectTransform.anchoredPosition = Vector2.zero; // This ensures it's at the center

            // Optionally, ensure the scale is default
            rectTransform.localScale = Vector3.one;

            // Assign button listeners for the new panel
            AssignButtonListeners();
        }
        else if (pauseMenuPanel != null)
        {
            // Hide the old panel when loading a new scene
            pauseMenuPanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("PauseMenuPanel not found in the new scene!");
        }
    }
    void AssignButtonListeners()
    {
        // Make sure PauseMenuPanel exists before assigning buttons
        if (pauseMenuPanel != null)
        {
            Button resumeButton = pauseMenuPanel.transform.Find("Resume")?.GetComponent<Button>();
            Button restartButton = pauseMenuPanel.transform.Find("Restart")?.GetComponent<Button>();
            Button quitButton = pauseMenuPanel.transform.Find("Quit")?.GetComponent<Button>();

            // Assign the button functions to the respective methods
            if(resumeButton != null && restartButton != null && quitButton != null){
                resumeButton.onClick.AddListener(ResumeGame);
                restartButton.onClick.AddListener(RestartLevel);
                quitButton.onClick.AddListener(QuitToMenu);
            }else{
                Debug.Log("No buttons were found");
            }
            
        }
    }

    public void IncrementScore(int pickUpPoints)
    {
        currentLevelScore += pickUpPoints;
        Debug.Log("Score: " + currentLevelScore);
    }

    public void IncrementTotal()
    {
        totalScore += currentLevelScore;
        Debug.Log("Total Score: " + totalScore);
    }

    public void LoadNextScene()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); 
    }

    public void RestartGame()
    {
        totalScore = 0;
        currentLevelScore = 0;
        timer = 0.0f;
        gameRunning = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(2);
    }

    public void RestartLevel()
    {
        currentLevelScore = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Update()
    {
        if (pauseMenuPanel == null)
            {
                pauseMenuPanel = GameObject.Find("PauseMenuPanel");  // Find and assign it
                if (pauseMenuPanel != null)
                {
                    DontDestroyOnLoad(pauseMenuPanel); // Keep PauseMenuPanel persistent
                    AssignButtonListeners();
                }
                else
                {
                    //Debug.LogWarning("PauseMenuPanel not found in the scene!");
                }
            }
        if (!gameRunning && SceneManager.GetActiveScene().buildIndex >= 2 && SceneManager.GetActiveScene().buildIndex < SceneManager.sceneCountInBuildSettings - 1) 
        {
            gameRunning = true;
        } 
        else if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings - 1) 
        {
            gameRunning = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        if(gameRunning && !isPaused){
            timer += Time.deltaTime;
        }

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
        if (pauseMenuPanel != null)  // Check if PauseMenuPanel exists
        {
            //Debug.Log("PauseMenuPanel found. Activating pause menu.");
            pauseMenuPanel.SetActive(true);  // Show Pause Menu
        }
        else
        {
            Debug.LogWarning("PauseMenuPanel is missing! Check your scene and references.");
        }
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

    public void QuitToMenu(){
        totalScore = 0;
        currentLevelScore = 0;
        timer = 0.0f;
        gameRunning = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("MainScene");
    }

    public void QuitGame(){
        // If you're in the editor, this won't fully work,
        // but in a built application, this will quit the game.
        Application.Quit();
    }
}
