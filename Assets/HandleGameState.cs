using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandleGameState : MonoBehaviour {

    private UIController UIcontroller;

    private bool clearToRestart = false;

    private bool goodEnding = false;

	public bool forceRestart = false;

    private GameObject playerOBJ;

	// Use this for initialization
	void Start () {
        UIcontroller = GameObject.Find("Canvas").GetComponent<UIController>();
        playerOBJ = GameObject.Find("Player");
	}
	
	// Update is called once per frame
	void Update () {

        if(playerOBJ == null)
        {
            StartCoroutine(GameOver());
        }


		if(Input.GetButtonDown("Action") && clearToRestart)
		{
            SceneManager.LoadScene("TitleScreen");
        }

		if(Input.GetKeyDown(KeyCode.Alpha0) && clearToRestart)
        {
            SceneManager.LoadScene("TitleScreen");
        }
	}

    IEnumerator GameOver()
    {
        //GameObject.Find ("MultipurposeCameraRig").GetComponent<CameraFade> ().SetScreenOverlayColor (Color.black);
        GameObject.Find("MultipurposeCameraRig").GetComponent<ScreenFade>().FadeOutScene(2.0f);
        yield return new WaitForSeconds(2.0f);
        UIcontroller.updateText(UIcontroller.GameStatusText, "YOU \n DIED");
        yield return new WaitForSeconds(2.0f);
        UIcontroller.updateText(UIcontroller.GameStatusSubtitle, "Press A to \n return to title screen");
        clearToRestart = true;

    }

    public void AscendAndShowResults()
    {
        if (!goodEnding)
        {
            StartCoroutine(goodGameEnd());
        }
    }

    IEnumerator goodGameEnd()
    {
        goodEnding = true;
        //Destroy(playerOBJ);
        //GameObject.Find ("MultipurposeCameraRig").GetComponent<CameraFade> ().SetScreenOverlayColor (Color.black);
        GameObject.Find("MultipurposeCameraRig").GetComponent<ScreenFade>().FadeOutScene(2.0f);
        yield return new WaitForSeconds(2.0f);
        UIcontroller.updateText(UIcontroller.GameStatusText, "RUN \n OVER");
        yield return new WaitForSeconds(2.0f);
        UIcontroller.updateText(UIcontroller.GameStatusText, "You collected " + playerOBJ.GetComponent<NPCTeamHandler>().getVStoneCollected() + " Kilos of Verminstone");
        yield return new WaitForSeconds(2.0f);
        UIcontroller.updateText(UIcontroller.GameStatusText, "Press the Zero key to restart");
        clearToRestart = true;
    }
}
