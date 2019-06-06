using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollController : MonoBehaviour {

	public GameObject NPCCopy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//ughhhhh
		//so the parent object just goes fuck all and the child object is like...does nothing. So 
		//i need to attach a trigger to the actual skeleton of the object to get this to work
		//use collider.transform.root to get the top parent, which has a reference to the bullshit i need
		//will have to res on the spot of the gameobject, in worldspace
	}
}
