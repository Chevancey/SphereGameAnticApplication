using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public static PauseManager Instance { get; private set; }

    public bool isPaused { get; private set; }

    [SerializeField] private GameObject pausePanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        pausePanel.SetActive(false);
    }



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void RestartGame() 
    {
        SceneManager.LoadScene(1);
    }

    void PauseGame()
    {
        pausePanel.SetActive(true);
    }

    void ResumeGame()
    {
        pausePanel.SetActive(false);
    }

    

}
