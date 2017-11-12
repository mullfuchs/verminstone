using UnityEngine;
using System.Collections;

public class MoveForward : MonoBehaviour {
	public float speed = 1.0f;
	public float maxTime = 5f;
    public float damage = 5.0f;

	// Update is called once per frame
	void Update () {
		maxTime -= 1.0f * Time.deltaTime;
		if (maxTime <= 0.0f) {
			GameObject.Destroy(gameObject);
		}

		transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}
}
