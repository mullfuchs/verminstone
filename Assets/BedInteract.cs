using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedInteract : MonoBehaviour
{

    public GameObject bedOccupiedObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "WorkerNPC" || other.tag == "dialog_npc")
        {
            bedOccupiedObject.SetActive(true);
            other.GetComponent<SleepTimeController>().setCharacterModelVisibility(false);
            other.GetComponent<SleepTimeController>().setDialogBoxActive(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "WorkerNPC" || other.tag == "dialog_npc")
        {
            bedOccupiedObject.SetActive(false);
            other.GetComponent<SleepTimeController>().setCharacterModelVisibility(true);
            other.GetComponent<SleepTimeController>().setDialogBoxActive(true);
        }
    }
}
