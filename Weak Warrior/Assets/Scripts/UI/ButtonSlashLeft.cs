using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSlashLeft : Touch {

	public override void OnPointerClick(PointerEventData eventData)
    {
        PlayerInput.Instance.SlashLeft();
    }
}
