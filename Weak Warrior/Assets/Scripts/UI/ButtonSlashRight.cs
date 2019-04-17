using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSlashRight : Touch {

    public AudioSource audioSource;
    public AudioClip buttonSlashPressed;
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        PlayerInput.Instance.SlashRight();
        audioSource.clip = buttonSlashPressed;
        audioSource.Play(0);
    }
}
