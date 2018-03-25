using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VStoneEconomyObject : MonoBehaviour {

    private float vStoneDailyAmount;
    private float vStoneTotalCollected;

    // Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void AddVStoneToDailyTotal(float amount)
    {
        vStoneDailyAmount += amount;
        vStoneTotalCollected += amount;
    }

    public float getDailyTotal()
    {
        return vStoneDailyAmount;
    }
}
