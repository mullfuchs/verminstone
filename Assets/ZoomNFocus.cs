using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomNFocus : MonoBehaviour {

	//Transform target;
	private Transform origin;
	private Quaternion originRot;
	public float transitionDuration = 2.5f;

	// Use this for initialization
	void Awake () {
		//origin = this.transform;
		originRot = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void focusOnNPC(Transform _target){
		origin = transform;
		StartCoroutine (Transition (origin, _target));
	}

	public void reset(){
		Transform currentTransform = transform;
		StartCoroutine(ResetPositon(currentTransform));
	}

	IEnumerator Transition(Transform start, Transform end)
	{
		Quaternion targetRotation = Quaternion.LookRotation (end.position - start.position);
		float t = 0.0f;
		while (t < 1.0f)
		{
			t += Time.deltaTime * (Time.timeScale/transitionDuration);
			//transform.LookAt(Vector3.Lerp(start, end, t));
			transform.rotation = Quaternion.Slerp(start.rotation, targetRotation, t);
			yield return 0;
		}

	}

	IEnumerator ResetPositon(Transform _current){
		float t = 0.0f;
		while (t < 1.0f) {
			t += Time.deltaTime * (Time.timeScale / transitionDuration);
			transform.rotation = Quaternion.Slerp (_current.rotation, originRot, t);
			yield return 0;
		}
	}

}
