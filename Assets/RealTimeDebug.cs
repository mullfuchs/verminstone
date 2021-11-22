using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RealTimeDebug : MonoBehaviour
{
    /// <summary>
    /// YESSS
    /// KILLLLLL
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            KillNPCWithBag();
        }

        if(Input.GetKeyDown(KeyCode.L))
        {
            ChangeCameraFocus();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ResetCameraFocus();
        }
    }

    void KillNPCWithBag()
    {
        //getNPCs
        Debug.Log("Killing NPC with Bag");
        List<GameObject> gameObjects = new List<GameObject>();
        gameObjects = gameObject.GetComponent<NPCTeamHandler>().GetCurrentCarriers();
        int randIndex = Random.Range(0, gameObjects.Count);
        GameObject npcToKill = gameObjects[randIndex];
        npcToKill.GetComponent<health>().AddDamage(1000000); //hey
    }

    void ChangeCameraFocus()
    {
        Debug.Log("TestingCameraMovement");
        GameObject cameraRig = GameObject.Find("MultipurposeCameraRig");
        cameraRig.GetComponent<ZoomNFocus>().focusOnPointBelowPlayer();
    }

    void ResetCameraFocus()
    {
        GameObject cameraRig = GameObject.Find("MultipurposeCameraRig");
        cameraRig.GetComponent<ZoomNFocus>().resetCameraRotation();
    }
}
