using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField]
    private GameObject endGamePanel;
    [SerializeField] 
    private TextMeshProUGUI scoreText;
    [SerializeField] 
    private TextMeshProUGUI timeText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Time.timeScale = 1;
        endGamePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowScore(int score, float time)
    {
        endGamePanel.SetActive(true);
        scoreText.text = $"Score: {score}";
        timeText.text = $"Time: {time:F1} seconds";
        Time.timeScale = 0; //pauses the game
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
