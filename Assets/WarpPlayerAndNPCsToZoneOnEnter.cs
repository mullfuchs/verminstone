using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpPlayerAndNPCsToZoneOnEnter : MonoBehaviour {

	public GameObject TargetZone;
	public Transform point;

	public bool ForceNPCsToFollowOnExit;
    public bool warpOnlyOne = false;

    public bool canTalkAfterWarping = true;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		if (other.tag == "Player" && TargetZone != null) {
            if (!warpOnlyOne)
            {
                StartCoroutine("warpToZOne");
            }
            else
            {
                StartCoroutine("warpPlayerToZone");
            }
		}
        if(other.tag == "WorkerNPC" && TargetZone != null && warpOnlyOne)
        {
            WarpNPCToZone(other.gameObject);
        }
	}

	IEnumerator warpToZOne(){
		//get objects
		GameObject player = GameObject.Find("Player");
		GameObject[] npcs = GameObject.FindGameObjectsWithTag ("WorkerNPC");
        player.GetComponent<Yarn.Unity.Example.DialogTrigger>().canTalkToNPCs = canTalkAfterWarping;
        if (ForceNPCsToFollowOnExit) {
			player.GetComponent<NPCTeamHandler> ().resetNPCTargets ();
		}

		//fade camera out
		GameObject.Find("MultipurposeCameraRig").GetComponent<CameraFade>().StartFade(Color.black, 2.0f);
		//move em
		yield return new WaitForSeconds(2.0f);
		PlayerAndNPCSpawner destinationObject = TargetZone.GetComponent<PlayerAndNPCSpawner>();
		destinationObject.setPoint (TargetZone.GetComponent<WarpPlayerAndNPCsToZoneOnEnter>().point.position);
		destinationObject.addPlayerAndNPCs (player, npcs);
		destinationObject.placePlayerAndNPCs ();

		GameObject.Find("MultipurposeCameraRig").GetComponent<CameraFade>().StartFade(Color.clear, 2.0f);
		//fade camera in

	}

    IEnumerator warpPlayerToZone()
    {
        GameObject.Find("MultipurposeCameraRig").GetComponent<CameraFade>().StartFade(Color.black, 2.0f);
        GameObject player = GameObject.Find("Player");
        yield return new WaitForSeconds(2.0f);
        player.transform.position = TargetZone.GetComponent<WarpPlayerAndNPCsToZoneOnEnter>().point.position;
        GameObject.Find("MultipurposeCameraRig").GetComponent<CameraFade>().StartFade(Color.clear, 2.0f);
       
    }

    public void WarpNPCToZone(GameObject g)
    {
       g.GetComponent<UnityStandardAssets.Characters.ThirdPerson.AICharacterControl>().agent.Warp(TargetZone.GetComponent<WarpPlayerAndNPCsToZoneOnEnter>().point.position);
    }
}
