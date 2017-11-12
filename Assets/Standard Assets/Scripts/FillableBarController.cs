using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FillableBarController : MonoBehaviour {

    private float MaxValue;
    private float CurrentValue;

    private Image bar;

	// Use this for initialization
	void Start () {
        bar = gameObject.GetComponent<Image>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SetMaxValue(float amount)
    {
        print("Setting max value of bar with " + amount);
        MaxValue = amount;
    }

    public void UpdateCurrentValue(float amount)
    {
        print("Updating current value of bar with " + amount);
        if(amount > 0 && bar != null)
        {
			bar.fillAmount = (amount / MaxValue);
        }
        else
        {
           // bar.fillAmount = 0;
        }
    }
}
