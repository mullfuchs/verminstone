using UnityEngine;
using System.Collections;

public class health : MonoBehaviour {
	public float healthPoints = 2;
	public bool isFreindlyFireOn = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter(Collision obj){
		if (obj.gameObject.tag == "projectile" && isFreindlyFireOn) {

			healthPoints -= obj.gameObject.GetComponent<MoveForward>().damage;
			Destroy (obj.gameObject);
			if (healthPoints <= 0) {
				Destroy (this.gameObject);
			}
		}

		if (obj.gameObject.tag == "Bug" && gameObject.tag != "Bug") {
			healthPoints -= 1;
			if (healthPoints <= 0) {
				Destroy (this.gameObject);
			}
		}
        

	}

    public void AddDamage(int damage)
    {
        healthPoints -= damage;
        if(healthPoints <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
