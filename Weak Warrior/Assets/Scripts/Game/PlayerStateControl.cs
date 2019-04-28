using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerStateControl : MonoSingleton<PlayerStateControl>
{
    public enum MovementState
    {
        Idle,
        Slash,
        Dash,
        Die
    }

    public enum ArmorState
    {
        Full,
        Half,
        Naked
    }

    public int health;
    public bool beingDamaged;
    public float beingDamagedDuration;
    protected float beingDamagedDurationTimer;
    public int currentMovementState;
    public int currentArmorState;
    public bool currentDirection;

    public float slashDuration;
    protected float slashDurationTimer;

    public BoxCollider2D hitBox;
    public Player_DamageBox damageBox;
    public Player_DamageBox ultimateDamageBox;
    public bool damageBoxOn;
    public bool ultimateDamageBoxOn;
    public float ultimateSpeed;
    public bool ultimateSide;

    public bool pause;

    public GameObject slashLeftButton;
    public GameObject slashRightButton;

    public Transform ultimateLocationLeft;
    public Transform ultimateLocationRight;
    public Transform ultimateEndLocation;

    public bool isSlashing;
    public bool isDashing;

    public AudioSource audioSource;
    public AudioClip hitSound;
    public AudioClip nakedHitSound;
    public AudioClip dieSound;

    // Use this for initialization
    void Start()
    {
        this.Setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause)
        {
            SlashDurationTiming();
            DashDurationTiming();
            BeingDamagedDurationTiming();
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        OnCollide(col);
    }

    public void Setup()
    {
        currentMovementState = (int)MovementState.Idle;
        currentArmorState = (int)ArmorState.Full;
        health = 3;
        beingDamaged = false;
        pause = false;
        currentDirection = false;
        PlayerAnimationControl.Instance.Flip(currentDirection);
        //currentArmorState = (int)ArmorState.Half;
        //currentArmorState = (int)ArmorState.Naked;
        PlayerAnimationControl.Instance.SetMovementState(currentMovementState);
        PlayerAnimationControl.Instance.SetArmorState(currentArmorState);
        isSlashing = false;
        isDashing = false;
        StopAllCoroutines();
    }

    public void Slash(bool direction)
    {
        if (currentMovementState == (int)MovementState.Idle)
        {
            currentMovementState = (int)MovementState.Slash;
            PlayerAnimationControl.Instance.SetMovementState(currentMovementState);
            PlayerAnimationControl.Instance.Slash(direction);
            currentDirection = direction;
            slashDurationTimer = 0;
            LoadMissSound();
        }
    }

    public void SlashDurationTiming()
    {
        if (currentMovementState == (int)MovementState.Slash)
        {

            slashDurationTimer += Time.deltaTime;
            if (slashDurationTimer >= slashDuration)
            {
                currentMovementState = (int)MovementState.Idle;
                PlayerAnimationControl.Instance.SetMovementState(currentMovementState);
            }

            if (slashDurationTimer >= 0.25f)
            {
                EnableMissedText();
                MissSound();
            }
        }
    }

    public void BeingDamagedDurationTiming()
    {
        if (beingDamaged & currentMovementState != (int)MovementState.Dash)
        {
            beingDamagedDurationTimer += Time.deltaTime;
            if (beingDamagedDurationTimer >= beingDamagedDuration)
            {
                beingDamaged = false;
                EnableHitBox();
                StopAllCoroutines();
                this.gameObject.GetComponent<Image>().enabled = true;
            }
            else
            {
                StartCoroutine(FlashSprite());
            }
        }
        else
        {
            beingDamagedDurationTimer = 0;
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        currentMovementState = (int)MovementState.Idle;
        PlayerAnimationControl.Instance.SetMovementState(currentMovementState);
        beingDamaged = true;
        DisableHitBox();
        if (health <= 0)
        {
            audioSource.clip = dieSound;
            audioSource.Play(0);
            health = 0;
            Popup.Instance.EnableDeadDialog(EnemySpawnManager.Instance.enemyLevelID, EnemySpawnManager.Instance.enemyKilled);            
            EnemySpawnManager.Instance.Pause();
            this.gameObject.SetActive(false);           
        }
        if (health == 3)
        {
            audioSource.clip = hitSound;
            audioSource.Play(0);
            currentArmorState = 0;
            PlayerAnimationControl.Instance.SetArmorState(currentArmorState);
        }
        else if (health == 2)
        {
            audioSource.clip = hitSound;
            audioSource.Play(0);
            currentArmorState = 1;
            PlayerAnimationControl.Instance.SetArmorState(currentArmorState);
        }
        else if (health == 1)
        {
            audioSource.clip = hitSound;
            audioSource.Play(0);
            currentArmorState = 2;
            PlayerAnimationControl.Instance.SetArmorState(currentArmorState);
        }
    }

    public void Dash()
    {
        beingDamaged = false;
        currentMovementState = (int)MovementState.Dash;
        PlayerAnimationControl.Instance.SetMovementState(currentMovementState);
        ultimateSide = false;
    }
    public void DashDurationTiming()
    {
        if (currentMovementState == (int)MovementState.Dash)
        {
            if (!currentDirection)
            {
                if (!ultimateSide)
                {
                    if (transform.position.x < ultimateLocationRight.position.x)
                    {
                        transform.position += new Vector3(ultimateSpeed, 0) * Time.deltaTime;
                    }
                    else
                    {
                        transform.position = new Vector3(ultimateLocationLeft.position.x, transform.position.y, 0);
                        ultimateSide = true;
                    }
                }
                else
                {
                    if (transform.position.x < ultimateEndLocation.position.x)
                    {
                        transform.position += new Vector3(ultimateSpeed, 0) * Time.deltaTime;
                    }
                    else
                    {
                        currentMovementState = (int)MovementState.Idle;
                        PlayerAnimationControl.Instance.SetMovementState(currentMovementState);
                        beingDamaged = true;
                        DisableHitBox();
                        beingDamagedDurationTimer = 0;
                        DisableUltimateDamageBox();
                        if (EnemySpawnManager.Instance.spawnedBosses.Count == 0)
                        {
                            EnemySpawnManager.Instance.UnPause();
                        }
                        EnemySpawnManager.Instance.UltimateUnPause();
                        EnemySpawnManager.Instance.AfterDash();
                        UltimateCooldown.Instance.Setup();
                    }
                }
            }
            else
            {
                if (!ultimateSide)
                {
                    if (transform.position.x > ultimateLocationLeft.position.x)
                    {
                        transform.position -= new Vector3(ultimateSpeed, 0) * Time.deltaTime;
                    }
                    else
                    {
                        transform.position = new Vector3(ultimateLocationRight.position.x, transform.position.y, 0);
                        ultimateSide = true;
                    }
                }
                else
                {
                    if (transform.position.x > ultimateEndLocation.position.x)
                    {
                        transform.position -= new Vector3(ultimateSpeed, 0) * Time.deltaTime;
                    }
                    else
                    {
                        currentMovementState = (int)MovementState.Idle;
                        PlayerAnimationControl.Instance.SetMovementState(currentMovementState);
                        beingDamaged = true;
                        DisableHitBox();
                        beingDamagedDurationTimer = 0;
                        DisableUltimateDamageBox();
                        if (EnemySpawnManager.Instance.spawnedBosses.Count == 0)
                        {
                            EnemySpawnManager.Instance.UnPause();
                        }
                        EnemySpawnManager.Instance.UltimateUnPause();
                        EnemySpawnManager.Instance.AfterDash();
                        UltimateCooldown.Instance.Setup();
                    }
                }
            }
        }
    }

    public void EnableHitBox()
    {
        hitBox.enabled = true;
    }

    public void EnableHitBoxNotDamaged()
    {
        if (!beingDamaged)
        {
            hitBox.enabled = true;
        }
    }

    public void DisableHitBox()
    {
        hitBox.enabled = false;
    }

    public void EnableDamageBox()
    {
        damageBox.EnableDamageBox();
        isSlashing = true;
    }

    public void EnableUltimateDamageBox()
    {
        ultimateDamageBox.EnableDamageBox();
        isDashing = true;
    }

    public void DisableDamageBox()
    {
        //damageBox.DisableDamageBox();
        isSlashing = false;
    }

    public void DisableUltimateDamageBox()
    {
        //ultimateDamageBox.DisableDamageBox();
        isDashing = false;
    }

    public void EnableMissedText()
    {
        UI_Text.Instance.EnableMissedText();
    }

    public void DisableMissedText()
    {
        UI_Text.Instance.DisableMissedText();
    }

    public void LoadMissSound()
    {
        UI_Text.Instance.LoadMissSound();
    }

    public void MissSound()
    {
        UI_Text.Instance.MissSound();
    }

    public void SetIdleState()
    {
        if (currentMovementState != (int)MovementState.Idle)
        {
            currentMovementState = (int)MovementState.Idle;
            PlayerAnimationControl.Instance.SetMovementState(currentMovementState);
        }
    }

    public void OnCollide(Collider2D col)
    {

    }

    public void Pause()
    {
        this.pause = true;
        DisableDamageBox();
        DisableHitBox();
    }

    public void UnPause()
    {
        this.pause = false;
        PlayerAnimationControl.Instance.SetMovementState(currentMovementState);
    }

    public void Revive()
    {
        currentArmorState = (int)ArmorState.Naked;
        currentMovementState = (int)MovementState.Idle;
        PlayerAnimationControl.Instance.SetMovementState(currentMovementState);
        PlayerAnimationControl.Instance.SetArmorState(currentArmorState);
        health = 1;
        StopAllCoroutines();
    }

    IEnumerator FlashSprite()
    {
        while (true)
        {
            this.gameObject.GetComponent<Image>().enabled = false;
            yield return new WaitForSeconds(.05f);
            this.gameObject.GetComponent<Image>().enabled = true;
            yield return new WaitForSeconds(.05f);
        }
    }
}
