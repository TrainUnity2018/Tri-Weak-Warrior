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
    // Use this for initialization
    void Start()
    {
        Setup();
    }

    void Setup()
    {
        inCooldown = true;
        cooldownDurationTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        CooldownDurationTiming();
    }

    void CooldownDurationTiming()
    {
        cooldownDurationTimer += Time.deltaTime;
        float fillAmount = 1 - (cooldownDurationTimer / cooldownDuration);
        cooldownImage.fillAmount = fillAmount;
        if (cooldownDurationTimer >= cooldownDuration)
        {
            inCooldown = false;
            ultimateCooldownReady.color = new Color(ultimateCooldownReady.color.r, ultimateCooldownReady.color.r, ultimateCooldownReady.color.r, 1f);
			cooldownImage.gameObject.SetActive(false);
        }
    }
}
