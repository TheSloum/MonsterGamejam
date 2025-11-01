using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public TextMeshProUGUI scoreText; // Assigne ici ton TMP (UI)

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[GameManager] Instance créée");
        }
        else
        {
            Debug.Log("[GameManager] Une autre instance existe déjà, destruction du nouveau GameObject");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        TMP_Text[] allTexts = FindObjectsOfType<TMP_Text>();
        foreach (var t in allTexts)
        {
            if (t != scoreText) t.gameObject.SetActive(false);
        }

        UpdateScoreText();
    }

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("[GameManager] AddScore: +" + amount + " => total = " + score);
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score : " + score.ToString();
            Debug.Log("[GameManager] scoreText mis à jour : " + scoreText.text);
        }
        else
        {
            Debug.LogWarning("[GameManager] scoreText NON assigné dans l'Inspector !");
        }
    }

    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }

    
}
