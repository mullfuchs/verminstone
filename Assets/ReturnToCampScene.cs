﻿ using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToCampScene : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //press x to skip
		if(Input.GetKeyDown(KeyCode.X))
        {
            //loadCamp();
        }
	}

	[Yarn.Unity.YarnCommand("returnToCampScene")]
	public void loadCamp(){
		print ("returning to camp?");
        Destroy(GameObject.Find("CampEventController")); //gonna try destroying the camp event controller then having the whole scene reload
        //DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("Camp");
        print("restoring camp stuff");
      //  GameObject.Find("CampEventController").GetComponent<CampNarrativeController>().SetPlayerAndNPCsActive(true);
        GameObject.Find ("CampEventController").GetComponent<CampEventController> ().StartDay ();
        Destroy(this.gameObject);

    }

    [Yarn.Unity.YarnCommand("goToScene")]
    public void goToScene(string sceneName)
    {
       // DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene(sceneName);
        Destroy(this.gameObject);
    }


}
