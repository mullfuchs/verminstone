using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTutorialLevel : MonoBehaviour {

	public string levelToLoad;
    private CameraFade cameraFade;

	// Use this for initialization
	void Start () {
        cameraFade = GameObject.Find("MultipurposeCameraRig").GetComponent<CameraFade>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter( Collider Other ){
		if (Other.gameObject.name == "Player") {
            StartCoroutine("changeLevels");
		}
	}

	void destroyPlayerAndNPCS(){
		GameObject[] npcs = GameObject.FindGameObjectsWithTag ("WorkerNPC");
		for (int i = 0; i < npcs.Length; i++) {
			Destroy (npcs [i]);
		}
		Destroy(GameObject.Find("Player"));
	}

    IEnumerator changeLevels()
    {
        cameraFade.StartFade(Color.black, 2.0f);
        yield return new WaitForSeconds(2.0f);
        destroyPlayerAndNPCS();
        SceneManager.LoadScene(levelToLoad);
    }
}
