 using System.Collections;
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
            loadCamp();
        }
	}

	[Yarn.Unity.YarnCommand("returnToCampScene")]
	public void loadCamp(){
		print ("returning to camp?");
        DontDestroyOnLoad(transform.gameObject);
        SceneManager.LoadScene("Camp");
        print("restoring camp stuff");
        GameObject.Find("CampEventController").GetComponent<CampNarrativeController>().SetPlayerAndNPCsActive(true);
        GameObject.Find ("CampEventController").GetComponent<CampEventController> ().StartDay ();
        Destroy(transform.gameObject);

    }

    [Yarn.Unity.YarnCommand("goToScene")]
    public void goToScene(string sceneName)
    {
        DontDestroyOnLoad(transform.gameObject);
        SceneManager.LoadScene(sceneName);
        Destroy(transform.gameObject);
    }


}
