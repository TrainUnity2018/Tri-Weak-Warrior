using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu_SettingButton : Touch
{

    public override void OnPointerClick(PointerEventData eventData)
    {
        Popup.Instance.EnableSettingMenu();
    }

}
