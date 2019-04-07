using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UltimateCooldown : MonoSingleton<UltimateCooldown>
{

    public Image cooldownImage;
    public Image ultimateCooldownReady;
    public bool inCooldown;
    public float cooldownDuration;
    protected float cooldownDurationTimer;
    public Animator animator;
    // Use this for initialization
    void Start()
    {
        Setup();
    }

    public void Setup()
    {
        inCooldown = true;
        cooldownDurationTimer = 0;
        cooldownImage.fillAmount = 1;
        cooldownImage.gameObject.SetActive(true);
        //ultimateCooldownReady.color = new Color(ultimateCooldownReady.color.r, ultimateCooldownReady.color.g, ultimateCooldownReady.color.b, 0f);
        animator.SetBool("Ready", false);
    }

    // Update is called once per frame
    void Update()
    {
        CooldownDurationTiming();
    }

    public void CooldownDurationTiming()
    {
        cooldownDurationTimer += Time.deltaTime;
        float fillAmount = 1 - (cooldownDurationTimer / cooldownDuration);
        cooldownImage.fillAmount = fillAmount;
        if (cooldownDurationTimer >= cooldownDuration)
        {
            inCooldown = false;
            //ultimateCooldownReady.color = new Color(ultimateCooldownReady.color.r, ultimateCooldownReady.color.g, ultimateCooldownReady.color.b, 1f);
            animator.SetBool("Ready", true);
            cooldownImage.gameObject.SetActive(false);
        }
    }
}
