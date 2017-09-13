using UnityEngine;
using System.Collections;

public class VStoneObject : MonoBehaviour {

	public bool HasBeenTouched = false;
	public bool HasBeenMined = false;
	public int energy = 10;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player" && !HasBeenTouched) {
			HasBeenTouched = true;
			this.GetComponent<ParticleSystem>().Stop();
		}

		if (other.tag == "Miner" && HasBeenTouched) {
			HasBeenMined = true;
		}

		if (other.tag == "Carrier" && HasBeenMined) {
			this.GetComponent<SphereCollider>().enabled = false;
		}

	}
	void OnTriggerExit(Collider other){
		if (other.tag == "Player") {
			energy = 0;
		}
	}
}
