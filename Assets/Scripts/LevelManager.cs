using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else Destroy(gameObject);
    }

    public void UnlockLevel(int levelIndex)
    {
        PlayerPrefs.SetInt("level_unlocked_" + levelIndex, 1);
        PlayerPrefs.Save();
    }

    public bool IsLevelUnlocked(int levelIndex)
    {
        return PlayerPrefs.GetInt("level_unlocked_" + levelIndex, levelIndex==1?1:0) == 1;
    }

    public void LoadLevel(int buildIndex)
    {
        if (IsLevelUnlocked(buildIndex) || buildIndex==1)
            SceneManager.LoadScene(buildIndex);
    }

    public void ResetProgress()
    {
        PlayerPrefs.DeleteAll();
    }
}
