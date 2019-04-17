using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonUltimate : Touch
{
    public AudioSource audioSource;
    public AudioClip buttonSlashPressed;
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (UltimateCooldown.Instance.inCooldown)
        {
            return;
        }
        else
        {
            audioSource.clip = buttonSlashPressed;
            audioSource.Play(0);
            PlayerStateControl.Instance.SetIdleState();
            PlayerStateControl.Instance.Dash();
            EnemySpawnManager.Instance.Pause();
        }
    }
}
