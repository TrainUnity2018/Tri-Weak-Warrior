using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonResume : Touch
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        Popup.Instance.DisablePauseDialog();
    }
}

