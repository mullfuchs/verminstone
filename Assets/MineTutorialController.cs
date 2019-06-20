using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineTutorialController : MonoBehaviour {

	public GameObject[] stonesToMine;

	public bool allStonesGone;

	bool m_Started;
	public LayerMask m_LayerMask;

	// Use this for initialization
	void Start () {
		allStonesGone = false;
		m_Started = true;
	}

	void FixedUpdate(){
		MyCollisions();
	}

	void MyCollisions (){
		Collider[] hitColliders = Physics.OverlapBox(gameObject.transform.position, transform.localScale / 2, Quaternion.identity, m_LayerMask);
		int i = 0;
		//Check when there is a new collider coming into contact with the box
		if (hitColliders.Length <= 0) {
			DoExitStuff ();
		}
	}

	void OnTriggerEnter(Collider other){
		if (other.gameObject.name == "Player") {
			DoIntroStuff ();
		}
	}

	void DoIntroStuff(){
		print ("Entering Mining Tutoral");
	}

	void DoExitStuff(){
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
