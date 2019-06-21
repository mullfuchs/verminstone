using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTutorialController : MonoBehaviour {

    bool startedTutorial = false;
    bool endedTutorial = false;

    public string TutorialStartText;
    public string TutorialEndText;    

	bool m_Started;
	public LayerMask m_LayerMask;

	public GameObject[] ObjectsToActivateOnStart;
	public GameObject[] ObjectsToDeactivateOnEnd;

	// Use this for initialization
	void Start () {
		m_Started = true;
	}

	void FixedUpdate(){
		MyCollisions();
	}

	void MyCollisions (){
		Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, m_LayerMask);
		//Check when there is a new collider coming into contact with the box
		if (hitColliders.Length <= 0 && endedTutorial == false) {
            endedTutorial = true;
            DoExitStuff ();
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.name == "Player" && startedTutorial == false) {
			startedTutorial = true;
			DoIntroStuff ();
		}
	}

	void DoIntroStuff(){
        GameObject.Find("Dialogue").GetComponent<Yarn.Unity.DialogueRunner>().StartDialogue(TutorialStartText);
		for (int i = 0; i < ObjectsToActivateOnStart.Length; i++) {
			ObjectsToActivateOnStart [i].SetActive (true);
		}
		print ("Entering Mining Tutoral");
	}

	void DoExitStuff(){
        GameObject.Find("Dialogue").GetComponent<Yarn.Unity.DialogueRunner>().StartDialogue(TutorialEndText);
		for (int i = 0; i < ObjectsToDeactivateOnEnd.Length; i++) {
			ObjectsToDeactivateOnEnd [i].SetActive (false);
		}
        print ("finished mining tutorial");
	}

	void OnDrawGizmos()
	{
		Gizmos.color = Color.red;
		//Check that it is being run in Play Mode, so it doesn't try to draw this in Editor mode
		if (m_Started)
			//Draw a cube where the OverlapBox is (positioned where your GameObject is as well as a size)
			Gizmos.DrawWireCube(transform.position, transform.localScale);
	}


}
