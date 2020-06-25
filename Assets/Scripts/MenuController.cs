using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public TextMeshProUGUI titleText;
    public TextMeshProUGUI subtitleText;
    public TextMeshProUGUI startText;

    void Start()
    {
        StartCoroutine(DisplayTextChar());
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
            SceneManager.LoadScene(1); 
    }

    IEnumerator DisplayTextChar()
    {
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
    }
}
