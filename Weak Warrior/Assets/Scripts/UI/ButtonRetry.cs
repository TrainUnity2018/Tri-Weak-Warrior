using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class ButtonRetry : Touch
{

    public override void OnPointerClick(PointerEventData eventData)
    {
        Popup.Instance.DisableDeadDialog();
        Popup.Instance.ButtonPressedSound();
        PlayerStateControl.Instance.Revive();
        EnemySpawnManager.Instance.UnPause();
        EnemySpawnManager.Instance.KillFirstEnemy();
    }
}
