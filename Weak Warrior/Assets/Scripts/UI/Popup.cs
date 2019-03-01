using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Popup : MonoSingleton<Popup>
{
    public Animator lastEnemyAnimator;
    public Text killCount;

    // Use this for initialization
    void Start()
    {
        Disable();
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

    public void LastEnemyShow(int id)
    {
        lastEnemyAnimator.SetInteger("ID", id);
    }

    public void KillCountShow(int killCount) {
        this.killCount.text = killCount.ToString();
    }
}
