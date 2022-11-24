﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuEnabler : MonoBehaviour
{
    public GameObject pauseMenuObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //add "if start menu pressed close menu" code here
    }

    public void EnablePauseMenu()
    {
        if(pauseMenuObject != null)
        {
            pauseMenuObject.SetActive(true);
        }
    }

    public void DisablePauseMenu()
    {
        if (pauseMenuObject != null)
        {
            pauseMenuObject.SetActive(false);
        }
    }
}
