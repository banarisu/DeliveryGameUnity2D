using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField]
    private GameObject buttons, helps, prompt;

    // Start is called before the first frame update
    void Start()
    {
        buttons.SetActive(true);
        helps.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame()
    {
        SceneManager.LoadScene("MainScene");
        Time.timeScale = 1;
    }

    public void promptQuit()
    {
        buttons.SetActive(false);
        prompt.SetActive(true);
    }

    public void cancelPrompt()
    {
        prompt.SetActive(false);
        buttons.SetActive(true);
    }

    public void callHelpUI()
    {
        buttons.SetActive(false);
        helps.SetActive(true);
    }

    public void quitHelpUI()
    {
        helps.SetActive(false);
        buttons.SetActive(true);
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
