using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu_ButtonPlay : Touch
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        Popup.Instance.DisableMainMenu();
        Popup.Instance.ButtonPressedSound();
    }
}
