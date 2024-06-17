using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private GameObject GameOverScreen;

    //Turn System
    [SerializeField] private int NumberOfTurns;
    public int currentNumberOfTurns { get; private set; }
    public int currentNumberOfTurnsLeft { get; private set; }
    [SerializeField] TextMeshProUGUI TurnDisplay;

    //Score
    public int score;
    [SerializeField] TextMeshProUGUI ScoreDisplay;

    //GameOver
    public bool isGameOver { get; private set; }
    [SerializeField] TextMeshProUGUI FinalScoreDisplay;


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

        GameOverScreen.SetActive(false);

        currentNumberOfTurnsLeft = NumberOfTurns;

        TurnDisplay.text = currentNumberOfTurnsLeft.ToString();
        ScoreDisplay.text = score.ToString();
    }

    public void TurnCompleted() 
    {
        currentNumberOfTurns++;
        currentNumberOfTurnsLeft = NumberOfTurns - currentNumberOfTurns;
        TurnDisplay.text = currentNumberOfTurnsLeft.ToString();

        score = PlayerController.Instance.totalPoints;
        ScoreDisplay.text = score.ToString();

        if (currentNumberOfTurns == NumberOfTurns) 
        {
            isGameOver = true;
            GameOverScreen.SetActive(true);
            FinalScoreDisplay.text = "Turns: " + NumberOfTurns.ToString() + " | Points: " + score.ToString();
        }
    }
}
