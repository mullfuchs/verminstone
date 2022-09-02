using UnityEngine;
using System.Collections;

public class PowerObject : MonoBehaviour {

    public GameObject UIObject;
    public GameObject HealEffect;

	private float powerAmount = 1;
    private float maxPowerAmount = 5;
	private float regenRate = 0.005f;

    public float healingAmount = 10.0f;
    public float healingCost = 0.8f;
    public float healingInterval = 0.3f;

	private bool canRegen = true;
	private bool healing = false;

    private bool canHeal = true;

	public int powerLevel = 1;
	public int xp = 1;
	public int powerThreshold = 5;

    private ShootOnAxisInput shootingObject;
    private UIController uiController;

	public GameObject HealingObject;

	public Light GlowLight;

    void Start()
    {
        UIObject = GameObject.Find("Canvas");
        uiController = UIObject.GetComponent<UIController>();
        shootingObject = transform.GetChild(0).GetComponent<ShootOnAxisInput>();

        uiController.updateBarMaxValue(uiController.XPBarObject, powerThreshold);
        uiController.updateBar(uiController.XPBarObject, 1);

        uiController.updateBarMaxValue(uiController.PowerBarObject, maxPowerAmount);
        uiController.updateBar(uiController.PowerBarObject, powerAmount);

        HealingObject = transform.Find("HealingRadius").gameObject;
        HealingObject.SetActive(false);
    }

	public void resetUIObject(GameObject uiObj){
		UIObject = uiObj;
		uiController = UIObject.GetComponent<UIController>();
        uiController.updateBarMaxValue(uiController.PowerBarObject, maxPowerAmount);
		uiController.updateBarMaxValue(uiController.XPBarObject, powerThreshold);
		uiController.updateText (uiController.XPText, powerLevel.ToString());
    }

	// Update is called once per frame
	void Update () {
		if (powerAmount > maxPowerAmount) {
			//canRegen = false;
		}

		if (powerAmount <= 0) {
			//canRegen = true;
		}

		if (canRegen && !healing) {
			//AddPowerAmount (regenRate);
			//canRegen = false;
			//Invoke ("ResetRegen", 0.2f);
		}
			
        if (Input.GetButton("HealButton") && canHeal)
        {
            healNPCIncremental();
            //healNPCs();
        }
       

		
	}

	public void AddPowerAmount(float amount){
		if ((powerAmount + amount) <= maxPowerAmount) {
			powerAmount += amount;
		} else {
			powerAmount = maxPowerAmount;
			canRegen = false;
		}

		if (uiController != null) {
			uiController.updateBar (uiController.PowerBarObject, powerAmount);
			setLightIntensity (powerAmount);
		}
    }

	public void RemovePowerAmount(float amount){
		if (powerAmount > 0 && uiController != null) {
			powerAmount -= amount;
            if (powerAmount == 0 && uiController.PowerBarObject != null)
            {
                uiController.updateBar(uiController.PowerBarObject, 0.1f);
            }
            else
            {
                uiController.updateBar(uiController.PowerBarObject, powerAmount);
            }
			setLightIntensity (powerAmount);
		}
    }

	void ResetRegen(){
		canRegen = true;
	}

    void levelUp()
    {
        
        if(powerAmount >= powerThreshold)
        {
            powerThreshold += 2;

			powerLevel += 1;
			xp = 0;
		
            uiController.updateBarMaxValue(uiController.XPBarObject, powerThreshold);
			uiController.updateBar(uiController.XPBarObject, xp);
			uiController.updateText (uiController.XPText, powerLevel.ToString());

        }
        
    }

	void levelDown(){
		if (powerLevel >= 2) {
			powerLevel -= 1;
			powerThreshold -= 2;
			xp = 0;
			uiController.updateBarMaxValue(uiController.XPBarObject, powerThreshold);
			uiController.updateBar(uiController.XPBarObject, xp);
			uiController.updateText (uiController.XPText, powerLevel.ToString());
		}
	}

    private float reduceValueUntilFloor(float value, float minValue, float reductionAmount)
    {
        float returnVal;
        if(value - reductionAmount >= minValue)
        {
            returnVal = value - reductionAmount;
        }
        else
        {
            returnVal = minValue;
        }
        return returnVal;
    }

    private float increaseValueUntilCeiling(float value, float maxValue, float additionAmount)
    {
        float returnVal;
        if (value - additionAmount <= maxValue)
        {
            returnVal = value + additionAmount;
        }
        else
        {
            returnVal = maxValue;
        }
        return returnVal;
    }

	void OnCollisionEnter(Collision other){
		if (other.gameObject.tag == "VerminStone") {
            print("tagged verminstone");
			AddPowerAmount(other.gameObject.GetComponent<VStoneObject>().energy);

            xp += 1;
            if(uiController != null)
            {
                uiController.updateBar(uiController.XPBarObject, xp);
            }
			//uiController.updateText (uiController.XPText, "Power Level: " + powerLevel);

            if(xp > powerThreshold)
            {
               // levelUp();
            }

			this.GetComponent<NPCTeamHandler>().AddStoneToBeMined(other.gameObject);
		}
	}

	void OnGUI(){
		//GUILayout.Label ("Power " + powerAmount + " Level " + powerLevel);
	}

	public float getPowerAmount(){
		return powerAmount;
	}

	void setLightIntensity(float powerLevel){
		if (powerLevel >= 5) {
			GlowLight.intensity = 5;
		}
        else if(powerLevel <= 1)
        {
            GlowLight.intensity = 1;
        }
        else {
			GlowLight.intensity = powerLevel;
		}
	}

    public void setGlowLight(bool isOn)
    {
        GlowLight.enabled = isOn;
    }

    void healNPCs()
    {
        //I could probably have a thing that slowly ticks down and takes away a percentage of health, allowing players to slowly heal everyone.
        float powerPercent = powerAmount / maxPowerAmount;

        GameObject[] npcs = GameObject.FindGameObjectsWithTag("WorkerNPC");
        foreach(GameObject npc in npcs)
        {
            health npcHeath = npc.GetComponent<health>();
            float npcMaxHealth = npcHeath.maxHealth;
            npcHeath.AddHealth(npcMaxHealth * powerPercent);
        }

        health playerHealth = gameObject.GetComponent<health>();
        float playerMaxHealth = playerHealth.maxHealth;
        playerHealth.AddHealth(playerMaxHealth * powerPercent);

        RemovePowerAmount(powerAmount);
    }

    public void resetHeal()
    {
        canHeal = true;
    }

    void healNPCIncremental()
    {
        //I guess just heal everyone and then remove a bit of power amount
        GameObject[] npcs = GameObject.FindGameObjectsWithTag("WorkerNPC");
        foreach (GameObject npc in npcs)
        {
            health npcHeath = npc.GetComponent<health>();
            npcHeath.AddHealth(healingAmount);
            GameObject healOrb = Instantiate(HealEffect, gameObject.transform.position, Quaternion.identity);
            healOrb.GetComponent<HealingOrbScript>().target = npc;
        }

        health playerHealth = gameObject.GetComponent<health>();
        playerHealth.AddHealth(healingAmount);

        RemovePowerAmount(healingCost);
        canHeal = false;
        Invoke("resetHeal", healingInterval);
    }
}
