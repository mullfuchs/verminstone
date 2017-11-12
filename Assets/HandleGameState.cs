﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandleGameState : MonoBehaviour {

    private UIController UIcontroller;

    private bool clearToRestart = false;

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


		if(Input.GetButtonDown("Fire1") && clearToRestart)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
	}

    IEnumerator GameOver()
    {
        //GameObject.Find ("MultipurposeCameraRig").GetComponent<CameraFade> ().SetScreenOverlayColor (Color.black);
        GameObject.Find("MultipurposeCameraRig").GetComponent<ScreenFade>().FadeOutScene(2.0f);
        yield return new WaitForSeconds(2.0f);
        UIcontroller.updateText(UIcontroller.GameStatusText, "GAME \n OVER");
        yield return new WaitForSeconds(2.0f);
        UIcontroller.updateText(UIcontroller.GameStatusText, "Press the A button to restart");
        clearToRestart = true;

    }
}
