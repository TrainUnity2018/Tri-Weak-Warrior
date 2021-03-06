﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoSingleton<PlayerInput>
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        InputHandle();
    }

    public void InputHandle()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PlayerStateControl.Instance.Slash(true);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            PlayerStateControl.Instance.Slash(false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {

        }
    }

    public void SlashRight()
    {
        PlayerStateControl.Instance.Slash(false);
    }

    public void SlashLeft()
    {
        PlayerStateControl.Instance.Slash(true);
    }
}
