using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageToObjects : MonoBehaviour {

    public string AffectedTag;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == AffectedTag)
        {
            other.GetComponent<health>().AddDamage(5);
            //gameObject.SetActive(false);
        }
    }
}
