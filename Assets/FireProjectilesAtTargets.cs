using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireProjectilesAtTargets : MonoBehaviour {

	public GameObject Projectile;

	private GameObject currentTarget = null;

	public float shootDelay = 2.0f;
	public float damage = 0.5f;
	public float projectileSpeed = 20.0f;
	public float fireSpread = 0.9f; //TODO, implement
	public float cost = 0.8f;
	public float secondsAlive = 5.0f;

	private bool canShoot = true;

	public float attackRange = 5.0f;

	private Transform shootTransform;

	void ResetShot(){
		canShoot = true;
	}

	// Use this for initialization
	void Start () {
		shootTransform = transform;
		shootTransform.position = transform.position;
		shootTransform.rotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		if (canShoot) {
			lookForANewTarget ();
		}
	}

	void lookForANewTarget(){
		Collider[] allOverlappingColliders = Physics.OverlapSphere(transform.position, attackRange);

		List<GameObject> NPCsToTarget = new List<GameObject> ();
		for (int i = 0; i < allOverlappingColliders.Length; i++) {
			if (allOverlappingColliders [i].gameObject.tag == "WorkerNPC" || allOverlappingColliders [i].tag == "Player") {
				NPCsToTarget.Add (allOverlappingColliders [i].gameObject);
			}
		}
		//print ("why isn't this working, found " + NPCsToTarget.Count + "targets");
		GameObject target = null;
		float distance = 100.0f;
	
		foreach (GameObject item in NPCsToTarget) {
			float tempdist = Vector3.Distance (transform.position, item.transform.position);
			if (tempdist < distance) {
				target = item;
				distance = tempdist;
			}
		}
			
		ShootProjectile(target);
	}

	void ShootProjectile(GameObject target)
	{
		if (target != null) {
			shootTransform.LookAt (target.transform);
			GameObject projectile = Instantiate(Projectile, shootTransform.position, shootTransform.rotation);
			Physics.IgnoreCollision (projectile.GetComponent<Collider> (), gameObject.GetComponent<Collider> (), true);
			projectile.GetComponent<MoveForward> ().originObject = gameObject;
		}
		canShoot = false;
		Invoke("ResetShot", shootDelay);

	}

}
