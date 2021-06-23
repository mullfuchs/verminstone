using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yarn.Unity.Example{
	public class DialogTrigger : MonoBehaviour {
		public float interactionRadius = 3.0f;

        public bool canTalkToNPCs = false;

		GameObject mainCam;

        GameObject CurrentNPC;

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


			if (Input.GetKeyDown(KeyCode.Q) || Input.GetButtonDown("Action") ){
				//CheckForNearbyNPC ();
				//CheckForClosestNPC();
			}
		}

        private void OnTriggerStay(Collider other)
        {
            if(other.tag == "NPCDialogTrigger")
            {
                //do a UI thing that shows who you're talking to
                //probably on the Player character, idk
                GameObject npc = other.gameObject;

                if(Input.GetButtonDown("Action") && canTalkToNPCs)
                {
                    canTalkToNPCs = false;
                    // Kick off the dialogue at this node.
                    if (npc.GetComponentInParent<NPCstats>().hasBeenTalkedToToday == false)
                    {
                        npc.GetComponentInParent<NPCstats>().hasBeenTalkedToToday = true;
                    }
                    CurrentNPC = npc.gameObject.transform.parent.gameObject;
                    if(CurrentNPC.GetComponent<NPCOverworldController>() != null)
                    {
                        CurrentNPC.GetComponent<NPCOverworldController>().idling = false;
                    }

                    FindObjectOfType<DialogueRunner>().StartDialogue(CurrentNPC.GetComponent<NPC>().talkToNode);
                    FindObjectOfType<DialogPortraitController>().populateDialogPortraits(CurrentNPC.GetComponent<NPCstats>().DialogPortraits, gameObject.GetComponent<NPCstats>().DialogPortraits);
                }
            }
        }

        void CheckForNearbyNPC(){
			var allParticipants = new List<NPC> (FindObjectsOfType<NPC> ());


			var target = allParticipants.Find (delegate (NPC p) {
				return string.IsNullOrEmpty (p.talkToNode) == false && // has a conversation node?
					(p.transform.position - this.transform.position)// is in range?
						.magnitude <= interactionRadius;
			});


			if (target != null && canTalkToNPCs) {
                canTalkToNPCs = false;
				// Kick off the dialogue at this node.
				if (target.GetComponentInParent<NPCstats> ().hasBeenTalkedToToday == false) {
					target.GetComponentInParent<NPCstats> ().hasBeenTalkedToToday = true;
				}
                CurrentNPC = target.gameObject;
                CurrentNPC.GetComponent<NPCOverworldController>().idling = false;

				FindObjectOfType<DialogueRunner> ().StartDialogue (target.talkToNode);
                FindObjectOfType<DialogPortraitController>().populateDialogPortraits(target.GetComponent<NPCstats>().DialogPortraits, gameObject.GetComponent<NPCstats>().DialogPortraits);
            }
		}

		void CheckForClosestNPC(){
			GameObject closest = FindClosestNPC ();
			if (closest != null) {
				FindObjectOfType<DialogueRunner> ().StartDialogue (closest.GetComponent<NPC> ().talkToNode);
				//mainCam.GetComponent<ZoomNFocus> ().focusOnNPC (closest.transform);
			}
			//mainCam.transform.SetPositionAndRotation(closest.GetComponent<NPC> ().CameraPosition.position, closest.GetComponent<NPC> ().CameraPosition.rotation);
		}

        public void ReleaseNPCPlayerIsTalkingTo()
        {
            canTalkToNPCs = true;
            if(CurrentNPC != null && CurrentNPC.GetComponent<NPCOverworldController>() != null)
            {
                CurrentNPC.GetComponent<NPCOverworldController>().idling = true;
            }
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
