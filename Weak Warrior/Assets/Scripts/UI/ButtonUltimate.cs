using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonUltimate : Touch
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (UltimateCooldown.Instance.inCooldown)
        {
            return;
        }
        else
        {
            PlayerStateControl.Instance.SetIdleState();
            PlayerStateControl.Instance.Dash();
            EnemySpawnManager.Instance.Pause();
        }
    }
}
