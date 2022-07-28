using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EscapeUIController : MonoBehaviour
{
    public GameObject CardPrefab;

    public GameObject cardParent;

    public GameObject TunnelDisplay;

    public GameObject EscapeButton;
    public GameObject ExitButton;

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
        TunnelDisplay.GetComponent<Text>().text = "Meters left to dig: " + (EscapeObjectInstance.metersNeededToEscape - metersOfTunnel);
        //make cards for em
        //add a player card too
        GameObject playerObj = GameObject.Find("Player");
        GameObject playerCard = Instantiate(CardPrefab, cardParent.transform);
        playerCard.SetActive(true);
        playerCard.GetComponent<NPCEscapeCardController>().assignNPCtoCard(playerObj);
        playerCard.GetComponent<NPCEscapeCardController>().parentEscapeUIController = this;
        NPCCards.Add(playerCard);
        NPCCards[0].GetComponentInChildren<UnityEngine.UI.Button>().Select();
        SetUpControllerNavigation();
        //display em
        //display how much digging's been done
    }

    public void CleanUpEscapeUI()
    {
        FindObjectOfType<PlayerEventController>().SetPlayerMovement(true);
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
        TunnelDisplay.GetComponent<Text>().text = "Distance left: " + (EscapeObjectInstance.metersNeededToEscape - metersOfTunnel);
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

    public void SetUpControllerNavigation()
    {
        Navigation escapeButtonNav = EscapeButton.GetComponent<Button>().navigation;
        escapeButtonNav.selectOnDown = ExitButton.GetComponent<Button>();
        escapeButtonNav.selectOnUp = NPCCards[NPCCards.Count - 1].GetComponent<NPCEscapeCardController>().digButton;
        EscapeButton.GetComponent<Button>().navigation = escapeButtonNav;

        Navigation exitButtonNav = ExitButton.GetComponent<Button>().navigation;
        exitButtonNav.selectOnDown = NPCCards[0].GetComponent<NPCEscapeCardController>().digButton;
        exitButtonNav.selectOnUp = EscapeButton.GetComponent<Button>();
        ExitButton.GetComponent<Button>().navigation = exitButtonNav;

        for (int cardIndex = 0; cardIndex < NPCCards.Count; cardIndex++)
        {
            Navigation buttonNav = NPCCards[cardIndex].GetComponent<NPCEscapeCardController>().digButton.navigation;
            buttonNav.selectOnUp = getSelectOnUp(NPCCards, cardIndex, ExitButton);
            buttonNav.selectOnDown = getSelectOnDown(NPCCards, cardIndex, EscapeButton);
            NPCCards[cardIndex].GetComponent<NPCEscapeCardController>().digButton.navigation = buttonNav;
        }
    }

    Button getSelectOnUp(List<GameObject> buttonStack, int currentIndex, GameObject DefaultButton)
    {
        //given the index, give the button above it in order, if it's the first, set it to the default button
        if (currentIndex == 0)
        {
            if (DefaultButton.GetComponent<Button>() != null)
            {
                return DefaultButton.GetComponent<Button>();
            }
        }
        else
        {
            return buttonStack[currentIndex - 1].GetComponentInChildren<Button>();
        }
        return null;
    }

    Button getSelectOnDown(List<GameObject> buttonStack, int currentIndex, GameObject DefaultButton)
    {
        //given index, give button below it in order, if it's the last, set it to default button;
        if (currentIndex == (buttonStack.Count - 1) && DefaultButton.GetComponent<Button>() != null)
        {
            return DefaultButton.GetComponent<Button>();
        }
        else
        {
            return buttonStack[currentIndex + 1].GetComponentInChildren<Button>();
        }
        return null;
    }
}
