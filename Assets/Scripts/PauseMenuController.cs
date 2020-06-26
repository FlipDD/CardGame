using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseScreen;
    public GameObject gridLayout;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            TogglePause();
    }

    public void TogglePause()
    {
        bool isActive = pauseScreen.activeSelf;
        int scale = isActive ? 1 : 0;
        Time.timeScale = scale;
        pauseScreen.SetActive(!isActive);
        gridLayout.SetActive(isActive);
    }

    public void LoadMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    } 

    public void QuitGame() => Application.Quit();
}
