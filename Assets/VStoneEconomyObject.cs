using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VStoneEconomyObject : MonoBehaviour {

    private float vStoneDailyAmount;
    private float vStoneTotalCollected;
	public float DailyQuota = 45;
	public float YesterdaysQuota = 0;
	public float IncrementAmout = 20;
	public float VStoneNeededForExtraFood = 5;

    // Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int getExtraPortionsBasedOnVstone(){
		float diff = vStoneDailyAmount - DailyQuota;
		return (int)(diff / VStoneNeededForExtraFood);
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

	public void SetDailyQuota(){
		GameObject.Find ("Player").GetComponent<NPCTeamHandler> ().setVstoneQuota (DailyQuota);
	}

	public void IncreaseDailyQuota(int day){
		YesterdaysQuota = DailyQuota;
		DailyQuota = DailyQuota + IncrementAmout;
	}

	public bool meetsDailyQuota(float vStoneAmount){
		if (vStoneAmount >= DailyQuota) {
			return true;
		}
		return false;
	}
		
}
