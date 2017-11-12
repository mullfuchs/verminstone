﻿using UnityEngine;
using System.Collections;

public class MoveForward : MonoBehaviour {
	public float speed = 10.0f;
	public float maxTime = 5f;
    public float damage = 10.0f;

	// Update is called once per frame
	void Update () {
		maxTime -= 1.0f * Time.deltaTime;
		if (maxTime <= 0.0f) {
			GameObject.Destroy(gameObject);
		}

		transform.Translate(Vector3.forward * speed * Time.deltaTime);
	}

    void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Bug")
        {
            other.gameObject.GetComponent<health>().AddDamage(damage);
        }
        Destroy(this.gameObject);
    }
}
