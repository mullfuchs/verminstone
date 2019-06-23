using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitTutorialLevel : MonoBehaviour {

	public string levelToLoad;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter( Collider Other ){
		if (Other.gameObject.name == "Player") {
			destroyPlayerAndNPCS ();
			SceneManager.LoadScene (levelToLoad);
		}
	}

	void destroyPlayerAndNPCS(){
		GameObject[] npcs = GameObject.FindGameObjectsWithTag ("WorkerNPC");
		for (int i = 0; i < npcs.Length; i++) {
			Destroy (npcs [i]);
		}
		Destroy(GameObject.Find("Player"));
	}
}
