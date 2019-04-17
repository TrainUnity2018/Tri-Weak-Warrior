using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonPause : Touch {

	public override void OnPointerClick(PointerEventData eventData)
    {
		Popup.Instance.EnablePauseDialog();
	}
}
