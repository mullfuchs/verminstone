using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealDamageToObjects : MonoBehaviour {

    public string AffectedTag;

	public float damageAmount;
	public float knockback;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        health OtherHealth = other.GetComponent<health>();
        if (other.tag == AffectedTag && OtherHealth)
        {
			other.attachedRigidbody.AddForce ( Vector3.Normalize( gameObject.transform.position - other.transform.position ) * 2 );
            OtherHealth.AddDamage(5);
            //gameObject.SetActive(false);
        }
    }
}
