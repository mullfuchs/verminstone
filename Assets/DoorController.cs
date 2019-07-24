using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour {

	//Door object exists as a prefab consisting of a door and a positional game object

	public GameObject DoorObject;
	float moveSpeed = 1.0f;

	public Transform OpenPosition;
	public Transform ClosePosition;

	public bool OpenAutomatically;
	public bool OpenOnPrompt;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.U)){

			OpenDoor ();
		}
	}

	public void OpenDoor(){
		StartCoroutine ("OpenDoorCoroutine");
	}

	public void CloseDoor(){
		StartCoroutine ("CloseDoorCoroutine");
	}

	IEnumerator OpenDoorCoroutine(){
		print ("Opening door?");
		float elapsedTime = 0;
		float waitTime = 3f;
		Vector3 currentPos;
		Vector3 targetPos = OpenPosition.position;
		currentPos = transform.position;

		while (elapsedTime < waitTime)
		{
			DoorObject.transform.position = Vector3.Lerp(currentPos, targetPos, (elapsedTime / waitTime));
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		DoorObject.transform.position = targetPos;
		yield return null;
		//transform.rotation = Quaternion.Lerp(from.rotation, to.rotation, Time.time * speed);	
	}

	IEnumerator CloseDoorCoroutine(){
		float elapsedTime = 0;
		float waitTime = 3f;
		Vector3 currentPos;
		Vector3 targetPos = ClosePosition.position;
		currentPos = transform.position;

		while (elapsedTime < waitTime)
		{
			DoorObject.transform.position = Vector3.Lerp(currentPos, targetPos, (elapsedTime / waitTime));
			elapsedTime += Time.deltaTime;
			yield return null;
		}

		DoorObject.transform.position = targetPos;
		yield return null;
	}
}
