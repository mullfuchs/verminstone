using System.Collections;
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

        metersOfTunnel = EscapeObjectInstance.metersOfTunnelDug;
    }

    private void Awake()
    {
        EscapeObjectInstance = GameObject.Find("CampAreaSecretEscape").GetComponent<SetNPCEscapeFlags>();
    }

    public void CreateAndDisplayNPCcards()
    {

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
        if(metersOfTunnel >= 10)
        {
            print("Successful escape");
        }
        else
        {
            print("ya fucked up");
        }
    }
        // Update is called once per frame
    void Update()
    {
        
    }
}
