using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSlashRight : Touch {

	public override void OnPointerClick(PointerEventData eventData)
    {
        PlayerInput.Instance.SlashRight();
    }
}
