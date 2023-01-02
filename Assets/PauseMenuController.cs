using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject resumeButton;

    void Start()
    {
        resumeButton.GetComponent<UnityEngine.UI.Button>().Select();
    }

    private void Awake()
    {
       
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ReturnToTitleScreen()
    {
        SceneManager.LoadScene("TitleScreen");
    }

    public void ContinueGame()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerEventController>().dialogOpened = false;
        this.gameObject.SetActive(false);
    }
}
