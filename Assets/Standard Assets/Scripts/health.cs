using UnityEngine;
using System.Collections;

public class health : MonoBehaviour {
	public float healthPoints = 2;
	public bool isFreindlyFireOn = false;
	public bool TrackOnTheUI = false;

	private UIController controller = null;

	// Use this for initialization
	void Start () {
		if (TrackOnTheUI) {
			controller = GameObject.Find ("Canvas").GetComponent<UIController> ();
			controller.updateBarMaxValue (controller.HealthBarObject, healthPoints);
			controller.updateBar (controller.HealthBarObject, healthPoints);
		}
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

		if (TrackOnTheUI) {
			controller.updateBar (controller.HealthBarObject, healthPoints);
		}
        

	}

    public void AddDamage(int damage)
    {
        healthPoints -= damage;

		if (TrackOnTheUI) {
			controller.updateBar (controller.HealthBarObject, healthPoints);
		}

        if(healthPoints <= 0)
        {
            Destroy(this.gameObject);
        }
    }


    void killgameObject()
    {
        if(gameObject.tag == "WorkerNPC")
        {
            gameObject.GetComponent<AIStateMachine>().handleDeath();
        }

        if(gameObject.tag == "Bug")
        {

        }

        Destroy(this.gameObject);
    }
}
