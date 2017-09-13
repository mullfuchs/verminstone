using UnityEngine;
using System.Collections;

public class ShootOnAxisInput : MonoBehaviour {
	public GameObject bullet;

	public string horizontalAxis = "Horizontal";
	public string verticalAxis = "Vertical";

	public Transform spawnOffset;

	public float shootDelay = 0.1f;


	private bool canShoot = true;

	void ResetShot(){
		canShoot = true;
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
//		float xAxisInput = Input.GetAxis (horizontalAxis);
//		float yAxisInput = Input.GetAxis (verticalAxis);
//
//		float shootAngle = Mathf.Atan2(xAxisInput, yAxisInput) * Mathf.Rad2Deg;
//
//		if(xAxisInput != 
//		transform.rotation = Quaternion.Euler (0.0f, 0.0f, shootAngle);
//


		Vector3 shootDirection = Vector3.right * Input.GetAxis (horizontalAxis) + Vector3.forward * Input.GetAxis (verticalAxis);
		if (shootDirection.sqrMagnitude > 0.0f) {
			transform.rotation = Quaternion.LookRotation (shootDirection, Vector3.up);
			this.transform.parent.GetComponent<IKControl>().ikActive = true;
		} else {
			transform.rotation = this.transform.parent.rotation;
			this.transform.parent.GetComponent<IKControl>().ikActive = false;
		}

		if (canShoot && shootDirection.sqrMagnitude > 0.1f) {
			Instantiate(bullet, spawnOffset.position, transform.rotation);
			//Physics.IgnoreCollision(bullet.GetComponent<Collider>(), this.transform.parent.GetComponent<Collider>());
			canShoot = false;
			gameObject.GetComponentInParent<PowerObject> ().RemovePowerAmount (0.1f);
			Invoke("ResetShot",shootDelay);
		}
	}
	
}
