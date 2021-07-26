﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeUIController : MonoBehaviour
{
    public GameObject CardPrefab;

    public GameObject cardParent;

    public GameObject TunnelDisplay;

    private GameObject[] NPCs;

    private List<GameObject> NPCCards;

    private SetNPCEscapeFlags EscapeObjectInstance;

    private int metersOfTunnel;
    // Start is called before the first frame update
    void Start()
    {
        NPCCards = new List<GameObject>();
        NPCs = GameObject.FindGameObjectsWithTag("WorkerNPC");
        if (EscapeObjectInstance != null)
        {
            metersOfTunnel = EscapeObjectInstance.metersOfTunnelDug;
        }
        else
        {
            metersOfTunnel = 0;
        }
    }

    private void Awake()
    {
        //EscapeObjectInstance = GameObject.Find("CampAreaSecretEscape").GetComponent<SetNPCEscapeFlags>();
    }

    private void SetUp()
    {

        EscapeObjectInstance = GameObject.Find("CampAreaSecretEscape").GetComponent<SetNPCEscapeFlags>();
        NPCCards = new List<GameObject>();
        NPCs = GameObject.FindGameObjectsWithTag("WorkerNPC");

        metersOfTunnel = EscapeObjectInstance.metersOfTunnelDug;
    }

    public void CreateAndDisplayNPCcards()
    {
        SetUp();
        cardParent.SetActive(true);
        //get npcs that are signed up to escape
        foreach(string g in EscapeObjectInstance.npcsThatCanEscape)
        {
            GameObject n = GameObject.Find(g);
            GameObject npcCard = Instantiate(CardPrefab, cardParent.transform);
            npcCard.SetActive(true);
            npcCard.GetComponent<NPCEscapeCardController>().assignNPCtoCard(n);
            npcCard.GetComponent<NPCEscapeCardController>().parentEscapeUIController = this;
            NPCCards.Add(npcCard);
        }
        TunnelDisplay.GetComponent<Text>().text = "Tunnel Dug: " + metersOfTunnel;
        //make cards for em
        //add a player card too
        GameObject playerObj = GameObject.Find("Player");
        GameObject playerCard = Instantiate(CardPrefab, cardParent.transform);
        playerCard.SetActive(true);
        playerCard.GetComponent<NPCEscapeCardController>().assignNPCtoCard(playerObj);
        playerCard.GetComponent<NPCEscapeCardController>().parentEscapeUIController = this;
        NPCCards.Add(playerCard);
        NPCCards[0].GetComponentInChildren<UnityEngine.UI.Button>().Select();
        //display em
        //display how much digging's been done
    }

    public void CleanUpEscapeUI()
    {
        EscapeObjectInstance.metersOfTunnelDug = metersOfTunnel;
        //remove everything
        foreach(GameObject g in NPCCards)
        {
            Destroy(g);
        }
        cardParent.SetActive(false);
    }

    public void updateEscapeStatsAndUI(int metersDug)
    {
        //update the amount of tunnel dug
        //update the UI
        metersOfTunnel += metersDug;
        TunnelDisplay.GetComponent<Text>().text = "Tunnel Dug: " + metersOfTunnel;
    }

    public void AttemptEscape()
    {
        if(metersOfTunnel >= EscapeObjectInstance.metersNeededToEscape)
        {
            print("Successful escape");
            //
            EscapeObjectInstance.LoadEscapeScene();
        }
        else
        {
            print("ya fucked up");
        }
    }

    public void EndDigAttempt()
    {
        EscapeObjectInstance.WarpNPCsToBed();
    }
        // Update is called once per frame
    void Update()
    {
        
    }
}
