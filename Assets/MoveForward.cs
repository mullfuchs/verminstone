using UnityEngine;
using System.Collections;

public class MoveForward : MonoBehaviour {
	public float speed = 10.0f;
	public float maxTime = 5f;
    public float damage = 10.0f;
	public string[] affectedTags;

	private bool isMoving = true;

	public GameObject originObject;

	public GameObject hitEffect;

	void Start(){
		//gameObject.GetComponent<Rigidbody> ().AddForce (Vector3.forward * speed);
	}

	// Update is called once per frame
	void Update () {
		maxTime -= 1.0f * Time.deltaTime;
		if (maxTime <= 0.0f) {
			GameObject.Destroy(gameObject);
		}
		if (isMoving) {
			transform.Translate(Vector3.forward * speed * Time.deltaTime);
		}

	}

    void OnCollisionEnter(Collision other)
    {
		for (int i = 0; i < affectedTags.Length; i++) {
			if (other.gameObject.tag == affectedTags [i]) {
				other.gameObject.GetComponent<health>().AddDamage(damage);
			}
		}
		//keeping this in for redundancy
       // if(other.gameObject.tag == "Bug")
        {
		//	other.gameObject.GetComponent<health>().AddDamage(damage);
        }

		if (other.gameObject != originObject) {
			isMoving = false;
			if (hitEffect != null) {
				gameObject.GetComponent<Collider> ().enabled = false;
				Instantiate(hitEffect, gameObject.transform.position, Quaternion.identity);
			}
		}
        
    }
		
}
