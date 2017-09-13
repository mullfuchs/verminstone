using UnityEngine;
using System.Collections;

public class LookAtTargetAndWalkForward : MonoBehaviour {
	public float walkSpeed;
	public GameObject target;	
	// Update is called once per frame
	void Start (){
		if (Random.Range (0, 2) <= 0 && target == null) {
			target = GameObject.FindGameObjectWithTag ("Player");
		} 
		else {
			target = GameObject.FindGameObjectWithTag ("Carrier");
		}

		this.gameObject.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl> ().target = target.transform;
	}

	void Update () {
//		if (target != null) {
//			//transform.LookAt (target.transform.position);
//		} else {
//			//target = GameObject.FindGameObjectWithTag ("Player");
//		}
//		transform.Translate(Vector3.forward * walkSpeed * Time.deltaTime);
	}
}
