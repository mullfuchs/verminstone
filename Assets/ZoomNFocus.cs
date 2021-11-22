using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomNFocus : MonoBehaviour {

	//Transform target;
	private Transform origin;
	private Quaternion originRot;
    public float rotationAmount = 11.0f;
	public float transitionDuration = 2.5f;
    private bool isRotated = false;

	// Use this for initialization
	void Awake () {
		//origin = this.transform;
		originRot = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void focusOnPointBelowPlayer()
    {
        //find player transform
        GameObject player = GameObject.Find("Player");
        //make a transform that is N units below player
        GameObject cameraLookAtObject = new GameObject();
        cameraLookAtObject.transform.position = player.transform.position;
        cameraLookAtObject.transform.Translate(new Vector3(0, 0, 10), player.transform);
        //then IDK, run focusOnTransform
        focusOnTransform(cameraLookAtObject.transform);

    }

	public void focusOnTransform(Transform _target){
        if(!isRotated)
        {
            origin = GameObject.Find("MainCamera").transform;
            origin.Rotate(rotationAmount, 0, 0);
            isRotated = !isRotated;
        }
        //StartCoroutine (Transition (origin, _target));
    }

	public void resetCameraRotation(){
        if(isRotated)
        {
            origin = GameObject.Find("MainCamera").transform;
            origin.Rotate(-rotationAmount, 0, 0);
            isRotated = !isRotated;
        }

       // Transform currentTransform = transform;
		//StartCoroutine(ResetPositon(currentTransform));
	}

	IEnumerator Transition(Transform start, Transform end)
	{
        GameObject camera = GameObject.Find("MainCamera");
        Quaternion targetRotation = Quaternion.Euler(70, 0, 0); // Quaternion.LookRotation (end.position - start.position);
		float t = 0.0f;
		while (t < 1.0f)
		{
			t += Time.deltaTime * (Time.timeScale/transitionDuration);
            //transform.LookAt(Vector3.Lerp(start, end, t));
            camera.transform.Rotate(70, 0, 0);
           // camera.transform.rotation = Quaternion.Slerp(start.rotation, targetRotation, t);
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
