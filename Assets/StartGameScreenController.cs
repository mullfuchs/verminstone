using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.UI;

public class StartGameScreenController : MonoBehaviour
{
    public GameObject startGameButton;
    public GameObject startGameWithDialogButton;
    public GameObject buttonGroup;
    public GameObject loadGameButton;
    public GameObject startNewGameDialog;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Awake()
    {
        if(!File.Exists(Application.persistentDataPath + "/gamesave.save"))
        {
            startGameWithDialogButton.SetActive(false);
            loadGameButton.SetActive(false);
        }
        else
        {
            startGameButton.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setButtonGroupActive(bool isActive)
    {
        buttonGroup.SetActive(isActive);
    }
}
