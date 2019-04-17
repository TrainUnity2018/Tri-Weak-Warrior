using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SettingMenu_ExitButton : Touch
{

    public override void OnPointerClick(PointerEventData eventData)
    {
        Popup.Instance.DisableSettingMenu();
        Popup.Instance.ButtonPressedSound();
    }
}
