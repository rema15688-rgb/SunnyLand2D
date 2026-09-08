using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseUI;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        bool isPaused = Time.timeScale == 0f;
        Time.timeScale = isPaused ? 1f : 0f;
        if (pauseUI) pauseUI.SetActive(!isPaused);
    }

    public void Resume() => TogglePause();
    public void QuitToMenu() { Time.timeScale = 1f; UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu"); }
}
