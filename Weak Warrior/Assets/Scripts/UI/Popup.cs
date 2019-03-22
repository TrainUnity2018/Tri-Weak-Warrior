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
    public GameObject slashLeftButton;
    public GameObject slashRightButton;


    // Use this for initialization
    void Start()
    {

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
        slashLeftButton.SetActive(false);
        slashRightButton.SetActive(false);
    }

    public void DisablePauseDialog()
    {
        this.pauseDialog.SetActive(false);
        if (PlayerStateControl.Instance.currentMovementState != (int)PlayerStateControl.MovementState.Dash)
            EnemySpawnManager.Instance.UnPause();
        PlayerStateControl.Instance.UnPause();
        slashLeftButton.SetActive(true);
        slashRightButton.SetActive(true);
    }

    public void EnableDeadDialog(int id, int killCount)
    {
        this.deadDiaglog.SetActive(true);
        lastEnemyAnimator.SetInteger("ID", id);
        this.killCount.text = killCount.ToString();
    }

    public void DisableDeadDialog()
    {
        this.deadDiaglog.SetActive(false);
    }
}
