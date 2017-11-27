using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public GameObject HealthBarObject;
    public GameObject PowerBarObject;
    public GameObject XPBarObject;

    public GameObject VStoneAmountText;
    public GameObject GameStatusText;

    private FillableBarController HealthBar;
    private FillableBarController PowerBar;
    private FillableBarController XPBar;

    // Use this for initialization
    void Start () {
        assignBarObject(HealthBar, HealthBarObject);
        assignBarObject(PowerBar, PowerBarObject);
        assignBarObject(XPBar, XPBarObject); }

    void assignBarObject(FillableBarController bar, GameObject barObject)
    {
        if(barObject != null)
        {
            bar = barObject.GetComponent<FillableBarController>();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void updateBar(GameObject barToUpdate, float amount)
    {
        print("updating bar with" + amount);
        if(barToUpdate != null)
        {
            barToUpdate.GetComponent<FillableBarController>().UpdateCurrentValue(amount);
        }
    }

    public void updateBarMaxValue(GameObject barToUpdate, float amount)
    {
        print("updating bar max value with" + amount);
        if(barToUpdate != null)
        {
            barToUpdate.GetComponent<FillableBarController>().SetMaxValue(amount);
        }
    }

    public void updateText(GameObject textToUpdate, string value)
    {
        if(textToUpdate != null)
        {
            textToUpdate.GetComponent<Text>().text = value;
        }
    }

}
