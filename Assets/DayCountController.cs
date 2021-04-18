using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DayCountController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        Text counterText = gameObject.GetComponent<Text>();
        CampEventController campNarrativeController = GameObject.Find("CampEventController").GetComponent<CampEventController>();
        counterText.text = "Day: " + campNarrativeController.day;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
