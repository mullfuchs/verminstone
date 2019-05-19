using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VStoneEconomyObject : MonoBehaviour {

    private float vStoneDailyAmount;
    private float vStoneTotalCollected;
	private float DailyQuota = 300;


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

	public void resetDailyTotal(){
		vStoneDailyAmount = 0;
	}

    public float getDailyTotal()
    {
        return vStoneDailyAmount;
    }

	public float getTotalCollected()
	{
		return vStoneTotalCollected;
	}

	public float getDailyQuota()
	{
		return DailyQuota;
	}

	public void IncreaseDailyQuota(int day){
		DailyQuota = DailyQuota * day;
	}

	public bool meetsDailyQuota(float vStoneAmount){
		if (vStoneAmount >= DailyQuota) {
			return true;
		}
		return false;
	}

	/*
	 * need to add...uhhh...some shit
	 * so we need to set the daily quota, and then we need to test wether the daily amout meets that
	 * 
	 */
}
