using UnityEngine;
using System.Collections;

public class PowerObject : MonoBehaviour {

	private float powerAmount = 0;

	// Update is called once per frame
	void Update () {
		
	}

	public void AddPowerAmount(float amount){
		powerAmount += amount;
	}

	public void RemovePowerAmount(float amount){
		powerAmount -= amount;	
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "VerminStone") {
			AddPowerAmount(other.GetComponent<VStoneObject>().energy);
			this.GetComponent<NPCTeamHandler>().AddStoneToBeMined(other.gameObject);
		}
	}

	void OnGUI(){
		GUILayout.Label ("Power " + powerAmount);
	}


}
