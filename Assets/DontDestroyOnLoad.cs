using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoad : MonoBehaviour {

    public bool disableScript = false;

	void Awake() {
        if (!disableScript)
        {
            DontDestroyOnLoad(transform.gameObject);
        }
        
	}
}
