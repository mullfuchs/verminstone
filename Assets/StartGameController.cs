using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGameController : MonoBehaviour {

	public bool loadGameFromSave = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartNewGame(){
		SceneManager.LoadScene ("tutorialLevel");
        loadGameFromSave = false;
	}

	public void LoadGameFromSave(){
		loadGameFromSave = true;
		//does the save file exist?
		SceneManager.LoadScene ("Camp");
		//next secene can fail I don't give a shit right now ha
	}

	public void ExitGame(){
		Application.Quit ();
	}
}
