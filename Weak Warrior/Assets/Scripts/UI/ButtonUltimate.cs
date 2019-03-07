using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonUltimate : Button
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
        if (UltimateCooldown.Instance.inCooldown)
        {
            return;
        }
        else
        {
			EnemySpawnManager.Instance.Pause();
        }
    }
}
