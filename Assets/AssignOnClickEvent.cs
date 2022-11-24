using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignOnClickEvent : MonoBehaviour
{
    // Start is called before the first frame update
    

    void Start()
    {
        
    }

    private void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(Button_OnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Button_OnClick()
    {
        GameObject.Find("CampEventController").GetComponent<CampEventController>().EnterCaveSequence();

    }

}
