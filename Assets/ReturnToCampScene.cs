 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToCampScene : MonoBehaviour {

    private CameraFade cameraFade;

    // Use this for initialization
    void Start () {
        cameraFade = GameObject.Find("Main Camera").GetComponent<CameraFade>();
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
        StartCoroutine("goToCamp");

        /* -- keeping this around in case this does not work
		print ("returning to camp?");
        Destroy(GameObject.Find("CampEventController")); //gonna try destroying the camp event controller then having the whole scene reload
        //DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("Camp");
        print("restoring camp stuff");
      //  GameObject.Find("CampEventController").GetComponent<CampNarrativeController>().SetPlayerAndNPCsActive(true);
        GameObject.Find ("CampEventController").GetComponent<CampEventController> ().StartDay ();
        Destroy(this.gameObject);
        */
    }

    [Yarn.Unity.YarnCommand("goToScene")]
    public void goToScene(string sceneName)
    {
        // DontDestroyOnLoad(this.gameObject);
        IEnumerator corutine = goToSceneWithFade(sceneName);
        StartCoroutine(corutine);
        /*
        SceneManager.LoadScene(sceneName);
        Destroy(this.gameObject);
        */
    }

    IEnumerator goToCamp()
    {
        cameraFade.StartFade(Color.black, 2.0f);
        yield return new WaitForSeconds(2.0f);

        cleanUpScene();
    }

    private void cleanUpScene()
    {
        print("returning to camp?");
        Destroy(GameObject.Find("CampEventController")); //gonna try destroying the camp event controller then having the whole scene reload
        //DontDestroyOnLoad(this.gameObject);
        SceneManager.LoadScene("Camp");
        print("restoring camp stuff");
        //  GameObject.Find("CampEventController").GetComponent<CampNarrativeController>().SetPlayerAndNPCsActive(true);
        GameObject.Find("CampEventController").GetComponent<CampEventController>().StartDay();
        Destroy(this.gameObject);
    }

    IEnumerator goToSceneWithFade(string sceneName)
    {
        cameraFade.StartFade(Color.black, 2.0f);
        yield return new WaitForSeconds(2.0f);

        SceneManager.LoadScene(sceneName);
        Destroy(this.gameObject);
    }



}
