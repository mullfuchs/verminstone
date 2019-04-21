using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yarn.Unity.Example{
	public class DialogTrigger : MonoBehaviour {
		public float interactionRadius = 3.0f;

		GameObject mainCam;

		// Use this for initialization
		void Start () {
			mainCam = GameObject.Find ("MultipurposeCameraRig");
		}

		// Update is called once per frame
		void Update () {
            if(FindObjectOfType<DialogueRunner>() != null)
            {
                if (FindObjectOfType<DialogueRunner>().isDialogueRunning == true)
                {
                    return;
                }
            }


			if (Input.GetKeyDown(KeyCode.Space)) {
				CheckForNearbyNPC ();
				//CheckForClosestNPC();
			}
		}

		void CheckForNearbyNPC(){
			var allParticipants = new List<NPC> (FindObjectsOfType<NPC> ());
			var target = allParticipants.Find (delegate (NPC p) {
				return string.IsNullOrEmpty (p.talkToNode) == false && // has a conversation node?
					(p.transform.position - this.transform.position)// is in range?
						.magnitude <= interactionRadius;
			});
			if (target != null) {
				// Kick off the dialogue at this node.
				if (target.GetComponentInParent<NPCstats> ().hasBeenTalkedToToday == false) {
					target.GetComponentInParent<NPCstats> ().hasBeenTalkedToToday = true;
				}
		
				FindObjectOfType<DialogueRunner> ().StartDialogue (target.talkToNode);
                FindObjectOfType<DialogPortraitController>().populateDialogPortraits(target.GetComponent<NPCstats>().DialogPortraits, gameObject.GetComponent<NPCstats>().DialogPortraits);
            }
		}

		void CheckForClosestNPC(){
			GameObject closest = FindClosestNPC ();
			FindObjectOfType<DialogueRunner> ().StartDialogue (closest.GetComponent<NPC> ().talkToNode);
			mainCam.GetComponent<ZoomNFocus> ().focusOnNPC (closest.transform);
			//mainCam.transform.SetPositionAndRotation(closest.GetComponent<NPC> ().CameraPosition.position, closest.GetComponent<NPC> ().CameraPosition.rotation);
		}

		public GameObject FindClosestNPC()
		{
			GameObject[] gos;
			gos = GameObject.FindGameObjectsWithTag("dialog_npc");
			GameObject closest = null;
			float distance = Mathf.Infinity;
			Vector3 position = transform.position;
			foreach (GameObject go in gos)
			{
				Vector3 diff = go.transform.position - position;
				float curDistance = diff.sqrMagnitude;
				if (curDistance < distance)
				{
					closest = go;
					distance = curDistance;
				}
			}
			return closest;
		}


	}

}
