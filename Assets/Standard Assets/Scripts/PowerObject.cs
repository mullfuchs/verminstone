using UnityEngine;
using System.Collections;

public class PowerObject : MonoBehaviour {

    public GameObject UIObject;

	private float powerAmount = 0;
    private float maxPowerAmount = 420;

    private int powerLevel = 1;
    private int xp = 0;
    private int powerThreshold = 5;

    private ShootOnAxisInput shootingObject;
    private UIController uiController;


    void Start()
    {
        uiController = UIObject.GetComponent<UIController>();
        shootingObject = transform.GetChild(0).GetComponent<ShootOnAxisInput>();

        uiController.updateBarMaxValue(uiController.XPBarObject, powerThreshold);
        uiController.updateBar(uiController.XPBarObject, xp);

        uiController.updateBarMaxValue(uiController.PowerBarObject, maxPowerAmount);
        uiController.updateBar(uiController.PowerBarObject, powerAmount);
    }


	// Update is called once per frame
	void Update () {
		
	}

	public void AddPowerAmount(float amount){
		powerAmount += amount;
        uiController.updateBar(uiController.PowerBarObject, powerLevel);
    }

	public void RemovePowerAmount(float amount){
		powerAmount -= amount;
        uiController.updateBar(uiController.PowerBarObject, powerLevel);
    }

    void levelUp()
    {
        if(powerAmount >= powerThreshold)
        {
            powerThreshold += 2;
            shootingObject.shootDelay = reduceValueUntilFloor(shootingObject.shootDelay, 0.01f, 0.01f);
            //        public float shootDelay = 0.3f;
            shootingObject.damage = increaseValueUntilCeiling(shootingObject.damage, 10.0f, 0.5f);
            // public float damage = 0.5f;
            shootingObject.projectileSpeed = increaseValueUntilCeiling(shootingObject.projectileSpeed, 70.0f, 5.0f);
            powerLevel++;

            uiController.updateBarMaxValue(uiController.XPBarObject, powerThreshold);
            uiController.updateBar(uiController.XPBarObject, powerLevel);
            // public float projectileSpeed = 0.5f;

            // public float fireSpread = 0.9f; //TODO, implement

            //  public float cost = 0.1f;
            //  public float secondsAlive = 5.0f;
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

	void OnTriggerEnter(Collider other){
		if (other.tag == "VerminStone") {
            print("tagged verminstone");
			AddPowerAmount(other.GetComponent<VStoneObject>().energy);

            xp += 1;

            if(xp > powerThreshold)
            {
                levelUp();
            }

			this.GetComponent<NPCTeamHandler>().AddStoneToBeMined(other.gameObject);
		}
	}

	void OnGUI(){
		GUILayout.Label ("Power " + powerAmount + " Level " + powerLevel);
	}


}
