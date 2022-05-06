using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCEscapeCardController : MonoBehaviour
{
    public EscapeUIController parentEscapeUIController;

    private GameObject associatedNPC;
    private health npcHealth;

    public GameObject nameText;
    public GameObject moodText;
    public GameObject healthBar;
    public GameObject staminaBar;

    public bool hasDugTunnel;

    public Button digButton;

    public Image portrait;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void UpdateEscapeCardUI()
    {
        NPCstats stats = associatedNPC.GetComponent<NPCstats>();
        print("stats name" + stats.name);
        healthBar.GetComponent<FillableBarController>().SetMaxValue(stats.maxHealth);
        healthBar.GetComponent<FillableBarController>().UpdateCurrentValue(stats.health);

        staminaBar.GetComponent<FillableBarController>().SetMaxValue(stats.maxStamina);
        staminaBar.GetComponent<FillableBarController>().UpdateCurrentValue(stats.stamina);


        healthBar.GetComponent<Image>().fillAmount = (npcHealth.healthPoints / npcHealth.maxHealth);
        staminaBar.GetComponent<Image>().fillAmount = (stats.stamina / stats.maxStamina);

        nameText.GetComponent<Text>().text = stats.name;
        moodText.GetComponent<Text>().text = stats.mood;

        portrait.sprite = associatedNPC.GetComponent<NPCstats>().DialogPortraits[0];
    }

    public void assignNPCtoCard(GameObject npc)
    {
        associatedNPC = npc;
        npcHealth = npc.GetComponent<health>();
        UpdateEscapeCardUI();
    }

    public void digPortionOfTunnel()
    {
        //check remaining amount
        if (!hasDugTunnel)
        {
            NPCstats stats = associatedNPC.GetComponent<NPCstats>();
            if(stats.health > 26)
            {
                stats.health -= 25;
                npcHealth.healthPoints = stats.health;

                UpdateEscapeCardUI();
                parentEscapeUIController.updateEscapeStatsAndUI(stats.attack);
            }

        }


    }

}
