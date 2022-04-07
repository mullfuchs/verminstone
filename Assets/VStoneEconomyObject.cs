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
    public int ExtraPortionsForDay = 0;

    public float ExtraVStoneForTwoPortion = 0.1f;
    public float ExtraVStoneForThreePortion = 0.25f;
    public float ExtraVstoneForFourPortion = 0.4f;
    public float ExtraVstoneForFivePortion = 0.55f;

    // Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int getExtraPortionsBasedOnVstone(){
		//float diff = vStoneDailyAmount - YesterdaysQuota;
		//return (int)(diff / VStoneNeededForExtraFood);
		return 4 + ExtraPortionsForDay;
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
