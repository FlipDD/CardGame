using System.Collections;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI subtitleText;
    public TextMeshProUGUI startText;
    public GameObject quitMenu;

    void Start() => StartCoroutine(DisplayTextChars());

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            LoadGameplayScene();
        else if (Input.GetKeyDown(KeyCode.Escape))
            ToggleMenu();
    }

    void LoadGameplayScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1); 
    }

    public void ToggleMenu()
    {
        bool isActive = quitMenu.activeSelf;
        int scale = isActive ? 0 : 1;
        Time.timeScale = scale;
        quitMenu.SetActive(!isActive);
    }

    public void QuitGame() => Application.Quit();

    IEnumerator DisplayTextChars()
    {
        enabled = false;
        var sb = new StringBuilder();
        var text = titleText.text;
        titleText.gameObject.SetActive(true);
        titleText.SetText("");

        for (int i = 0; i < text.Length; i++)
        {
            sb.Append(text[i]);
            titleText.text = sb.ToString();
            yield return new WaitForSeconds(.15f);
        }

        yield return new WaitForSeconds(.5f);
        subtitleText.gameObject.SetActive(true);

        yield return new WaitForSeconds(2);
        startText.gameObject.SetActive(true);
        enabled = true;
    }
}
