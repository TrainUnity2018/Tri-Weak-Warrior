﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonExit : Button
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void onClick()
    {
        Application.Quit();
    }
}
