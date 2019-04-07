using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoSingleton<Popup>
{
    public Animator lastEnemyAnimator;
    public Text killCount;

    public GameObject deadDiaglog;
    public GameObject pauseDialog;
    public GameObject gamePlayButtons;
    public GameObject pauseButton;
    public GameObject mainMenu;
    public GameObject startMenu;
    public GameObject settingMenu;
    public GameObject player;


    // Use this for initialization
    void Start()
    {
        EnableMainMenu();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Disable()
    {
        this.gameObject.SetActive(false);
    }

    public void Enable()
    {
        this.gameObject.SetActive(true);
    }

    public void EnablePauseDialog()
    {
        this.pauseDialog.SetActive(true);
        if (PlayerStateControl.Instance.currentMovementState != (int)PlayerStateControl.MovementState.Dash)
            EnemySpawnManager.Instance.Pause();
        PlayerStateControl.Instance.Pause();
        gamePlayButtons.SetActive(false);
    }

    public void DisablePauseDialog()
    {
        this.pauseDialog.SetActive(false);
        if (PlayerStateControl.Instance.currentMovementState != (int)PlayerStateControl.MovementState.Dash)
            EnemySpawnManager.Instance.UnPause();
        PlayerStateControl.Instance.UnPause();
        gamePlayButtons.SetActive(true);
    }

    public void EnableDeadDialog(int id, int killCount)
    {
        this.deadDiaglog.SetActive(true);
        lastEnemyAnimator.SetInteger("ID", id);
        this.killCount.text = killCount.ToString();
        this.mainMenu.SetActive(false);
        this.pauseDialog.SetActive(false);
        this.gamePlayButtons.SetActive(false);
        this.pauseButton.SetActive(false);
        this.startMenu.SetActive(false);
        this.settingMenu.SetActive(false);
    }

    public void DisableDeadDialog()
    {
        this.deadDiaglog.SetActive(false);
        this.player.SetActive(true);
        PlayerStateControl.Instance.Revive();
        this.mainMenu.SetActive(false);
        this.pauseDialog.SetActive(false);
        this.gamePlayButtons.SetActive(true);
        this.pauseButton.SetActive(false);
        this.startMenu.SetActive(false);
        this.settingMenu.SetActive(false);
    }

    public void EnableMainMenu() {
        this.mainMenu.SetActive(true);
        this.deadDiaglog.SetActive(false);
        this.pauseDialog.SetActive(false);
        this.gamePlayButtons.SetActive(false);
        this.pauseButton.SetActive(false);
        this.startMenu.SetActive(false);
        this.player.SetActive(false);
        this.settingMenu.SetActive(false);
        EnemySpawnManager.Instance.Disable();
        EnemySpawnManager.Instance.Pause();
        EnemySpawnManager.Instance.Setup();
    }

    public void DisableMainMenu() {
        this.mainMenu.SetActive(false);
        this.deadDiaglog.SetActive(false);
        this.pauseDialog.SetActive(false);
        this.gamePlayButtons.SetActive(false);
        this.pauseButton.SetActive(false);
        this.startMenu.SetActive(true);
        this.player.SetActive(true);
        this.settingMenu.SetActive(false);
        EnemySpawnManager.Instance.Pause();
        EnemySpawnManager.Instance.Disable();
    }

    public void DisableStartMenu() {
        this.mainMenu.SetActive(false);
        this.deadDiaglog.SetActive(false);
        this.pauseDialog.SetActive(false);
        this.gamePlayButtons.SetActive(true);
        this.pauseButton.SetActive(true);
        this.startMenu.SetActive(false);
        this.player.SetActive(true);
        this.settingMenu.SetActive(false);
        PlayerStateControl.Instance.Setup();
        EnemySpawnManager.Instance.Enable();
    }

    public void EnableSettingMenu() {
        this.mainMenu.SetActive(true);
        this.deadDiaglog.SetActive(false);
        this.pauseDialog.SetActive(false);
        this.gamePlayButtons.SetActive(false);
        this.pauseButton.SetActive(false);
        this.startMenu.SetActive(false);
        this.player.SetActive(false);
        this.settingMenu.SetActive(true);
    }

    public void DisableSettingMenu() {
        this.mainMenu.SetActive(true);
        this.deadDiaglog.SetActive(false);
        this.pauseDialog.SetActive(false);
        this.gamePlayButtons.SetActive(false);
        this.pauseButton.SetActive(false);
        this.startMenu.SetActive(false);
        this.player.SetActive(false);
        this.settingMenu.SetActive(false);
    }
}
