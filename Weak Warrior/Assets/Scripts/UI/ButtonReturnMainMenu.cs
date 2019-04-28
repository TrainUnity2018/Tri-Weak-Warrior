using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonReturnMainMenu : Touch
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        Popup.Instance.EnableMainMenu();
        Popup.Instance.ButtonPressedSound();
    }
}
