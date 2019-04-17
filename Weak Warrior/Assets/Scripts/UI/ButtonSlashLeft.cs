using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSlashLeft : Touch {

	public AudioSource audioSource;
    public AudioClip buttonSlashPressed;
    
    public override void OnPointerClick(PointerEventData eventData)
    {
        PlayerInput.Instance.SlashLeft();
        audioSource.clip = buttonSlashPressed;
        audioSource.Play(0);
    }
}
