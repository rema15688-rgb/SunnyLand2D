using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public int coins = 0;

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        UIManager.Instance.UpdateCoins(coins);
    }

    public void PlayerDied()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void WinLevel()
    {
        int nextBuild = SceneManager.GetActiveScene().buildIndex + 1;
        LevelManager.Instance.UnlockLevel(nextBuild);
        SceneManager.LoadScene("Win");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
