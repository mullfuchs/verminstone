using UnityEngine;
using System.Collections;

public class health : MonoBehaviour {
	public float healthPoints = 2;
	public float stamina = 100;
    public float maxHealth;
	public bool isFreindlyFireOn = false;
	public bool TrackOnTheUI = false;


    private int defensePoints = 0;
	private UIController controller = null;

	private FillableBarController healthBar;

	public GameObject DeathEffectObject;

	// Use this for initialization
	void Start () {
		if(gameObject.tag == "WorkerNPC" || gameObject.tag == "Player")
        {
            healthPoints = gameObject.GetComponent<NPCstats>().health;
            defensePoints = gameObject.GetComponent<NPCstats>().defense;
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
            //healthPoints -= obj.gameObject.GetComponent<MoveForward>().damage;
            ///AddDamage(obj.gameObject.GetComponent<MoveForward>().damage);
           // Destroy (obj.gameObject);
			//if (healthPoints <= 0) {
			//	killgameObject ();
				//Destroy (this.gameObject);
			//}
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
			controller.updateBarMaxValue (controller.HealthBarObject, maxHealth);
			controller.updateBar (controller.HealthBarObject, healthPoints);
		}
        

	}

    public void AddDamage(float damage)
    {
		float tempDamage = (damage - defensePoints);
		if (tempDamage <= 0) {
			healthPoints -= 1;
		} else {
			healthPoints -= tempDamage;
		}

		if (healthBar != null) {
			healthBar.UpdateCurrentValue (healthPoints);
		}

		if (gameObject.GetComponent<NPCstats> () != null) {
			gameObject.GetComponent<NPCstats> ().health = healthPoints;
		}

		if (TrackOnTheUI) {
			controller.updateBar (controller.HealthBarObject, healthPoints);
		}

        if(healthPoints <= 0)
        {
			killgameObject ();
			//Destroy(this.gameObject);
        }
    }


    public bool AddHealth(float health)
    {
        float tempHealth = healthPoints + health;
        if(tempHealth < maxHealth)
        {
			healthPoints = tempHealth;

			if (healthBar != null) {
				healthBar.UpdateCurrentValue (healthPoints);
			}

            return true;
        }
        else
        {
			healthPoints = maxHealth;

			if (healthBar != null) {
				healthBar.UpdateCurrentValue (healthPoints);
			}

            return false;
        }
			
    }

    void killgameObject()
    {
		if (DeathEffectObject != null) {
			print ("creating death effect");
			Instantiate (DeathEffectObject, gameObject.transform.position, Quaternion.identity);	
		}

        if(gameObject.tag == "WorkerNPC")
        {
			if(gameObject.GetComponent<NPCstats> ().ragDollObject != null){
                GameObject.FindGameObjectWithTag("Player").GetComponent<NPCTeamHandler>().handleNPCDeath(gameObject);
				GameObject ragdoll = Instantiate (gameObject.GetComponent<NPCstats> ().ragDollObject, gameObject.transform.position, gameObject.transform.rotation);
                ragdoll.GetComponent<RagdollController>().NPCCopy = this.gameObject;
                this.gameObject.transform.parent = ragdoll.transform;
                this.gameObject.SetActive(false);
			}
			/*
			if (gameObject.GetComponent<NPCInventory> () != null) {
				NPCInventory inventory = gameObject.GetComponent<NPCInventory> ();
				if (inventory.ObjectOnBack != null) {
					inventory.DropBackItem ();
				}
				if (inventory.ObjectHeldInHands != null) {
					inventory.DropHandItem ();
				}
			}
			*/
			gameObject.GetComponent<AIStateMachine>().handleDeath();
            return;
        }
			
        Destroy(this.gameObject);
    }

	public void SetTrackingUIElement(FillableBarController _healthBar){
		healthBar = _healthBar;
	}
}
