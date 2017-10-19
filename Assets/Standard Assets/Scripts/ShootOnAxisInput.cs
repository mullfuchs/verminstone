using UnityEngine;
using System.Collections;

public class ShootOnAxisInput : MonoBehaviour {
	public GameObject bullet;

	public string horizontalAxis = "Horizontal2";
	public string verticalAxis = "Vertical2";

	public Transform spawnOffset;

	public float shootDelay = 0.3f;
    public float damage = 0.5f;
    public float projectileSpeed = 20.0f;
    public float fireSpread = 0.9f; //TODO, implement
    public float cost = 0.1f;
    public float secondsAlive = 5.0f;

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
            ShootProjectile();
		}
	}

    void ShootProjectile()
    {
        Instantiate(bullet, spawnOffset.position, transform.rotation);
        //bullet.GetComponent<MoveForward>().speed = projectileSpeed;
        //bullet.GetComponent<MoveForward>().maxTime = secondsAlive;
        //Physics.IgnoreCollision(bullet.GetComponent<Collider>(), this.transform.parent.GetComponent<Collider>());
        canShoot = false;
        gameObject.GetComponentInParent<PowerObject>().RemovePowerAmount(cost);
        Invoke("ResetShot", shootDelay);
    }
	
}
