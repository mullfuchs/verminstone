using UnityEngine;
using System.Collections;

public class health : MonoBehaviour {
	public float healthPoints = 2;
	public float stamina = 100;
    private float maxHealth;
	public bool isFreindlyFireOn = false;
	public bool TrackOnTheUI = false;

	private UIController controller = null;

	// Use this for initialization
	void Start () {
        if(gameObject.tag == "WorkerNPC")
        {
            healthPoints = gameObject.GetComponent<NPCstats>().health;
        }
		if (TrackOnTheUI) {
			controller = GameObject.Find ("Canvas").GetComponent<UIController> ();
			controller.updateBarMaxValue (controller.HealthBarObject, healthPoints);
			controller.updateBar (controller.HealthBarObject, healthPoints);
		}

        maxHealth = healthPoints;
    }
	
	// Update is called once per frame
	void Update () {

	}

	void OnCollisionEnter(Collision obj){
		if (obj.gameObject.tag == "projectile" && isFreindlyFireOn) {
			//print ("took projectile damage");
			healthPoints -= obj.gameObject.GetComponent<MoveForward>().damage;
			Destroy (obj.gameObject);
			if (healthPoints <= 0) {
				Destroy (this.gameObject);
			}
		}

		/*
		if (obj.gameObject.tag == "Bug" && gameObject.tag != "Bug") {
			healthPoints -= 1;
			gameObject.GetComponent<NPCstats>().health = healthPoints;
			if (healthPoints <= 0) {
				Destroy (this.gameObject);
			}
		}
		*/

		if (TrackOnTheUI) {
			controller = GameObject.Find ("Canvas").GetComponent<UIController> ();
			controller.updateBarMaxValue (controller.HealthBarObject, healthPoints);
			controller.updateBar (controller.HealthBarObject, healthPoints);
		}
        

	}

    public void AddDamage(float damage)
    {
        healthPoints -= damage;

		if (gameObject.GetComponent<NPCstats> () != null) {
			gameObject.GetComponent<NPCstats> ().health = healthPoints;
		}

		if (TrackOnTheUI) {
			controller.updateBar (controller.HealthBarObject, healthPoints);
		}

        if(healthPoints <= 0)
        {
			if(gameObject.GetComponent<NPCstats> ().ragDollObject != null){
				Instantiate (gameObject.GetComponent<NPCstats> ().ragDollObject, gameObject.transform.position, gameObject.transform.rotation); 
			}
			Destroy(this.gameObject);
        }
    }


    public bool AddHealth(float health)
    {
        float tempHealth = healthPoints + health;
        if(tempHealth < maxHealth)
        {
            health = tempHealth;
            return true;
        }
        else
        {
            return false;
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
