using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepTimeController : MonoBehaviour
{
    public GameObject characterModelReference;
    public GameObject dialogRadius;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setCharacterModelVisibility(bool isVisible)
    {
        gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = isVisible; 
    }

    public void setDialogBoxActive(bool isEnabled)
    {
        dialogRadius.SetActive(isEnabled);
    }
}
