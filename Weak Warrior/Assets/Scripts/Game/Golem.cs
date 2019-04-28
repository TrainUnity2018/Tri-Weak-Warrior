using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golem : Orge {

    // Use this for initialization
    void Start()
    {
        EnableHitBox();
        currentMovementState = (int)MovementState.Walk;
        animator.SetInteger("State", currentMovementState);
        pause = false;
        initHealth = health;
        currentHealth = initHealth;
        animationDelayTimer = animationDelayDuration;
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause)
        {
            Walk();
            SlashDelayTiming();
            DelayAnimationOnTakeDamage();
        }
        if (!Popup.Instance.pauseDialog.activeSelf && !Popup.Instance.deadDiaglog.activeSelf)
        {
            OnDead();
        }
    }

    public override void DelayAnimationOnTakeDamage()
    {
        if (currentMovementState == (int)MovementState.Slash || currentMovementState == (int)MovementState.Walk)
        {
            animationDelayTimer += Time.deltaTime;
            if (animationDelayTimer >= animationDelayDuration)
            {
                animator.speed = 1f;
            }
            else
            {
                animator.speed = animationDelayRate;
            }
        }
    }

    IEnumerator DieEffect()
    {
        while (true)
        {
            yield return new WaitForSeconds(.2f);
            dieEffect.GetComponent<SpriteRenderer>().enabled = false;
            StopCoroutine(DieEffect());
        }
    }

    IEnumerator HitEffect()
    {
        for (int i = 0; i < 5; i++)
        {
            Color color = new Color((float)(255 / 255), (float)(61 / 255), (float)(61 / 255), (float)(255 / 255));
            this.gameObject.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(.05f);
            color = new Color((float)(255 / 255), (float)(255 / 255), (float)(255 / 255), (float)(255 / 255));
            this.gameObject.GetComponent<SpriteRenderer>().color = color;
            yield return new WaitForSeconds(.05f);
        }
        StopCoroutine(HitEffect());
    }
}
