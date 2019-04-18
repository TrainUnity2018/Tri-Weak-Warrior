using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmorGiver : MonoBehaviour
{

    public Vector3 spawnLocation;
    public Vector3 firstEndMovingLocation;
    public Vector3 middleFirstEndLocation;
    public Vector3 sencondEndMovingLocation;
    public Vector3 middleSecondEndLocation;
    public Vector3 playerLocation;

    public GameObject presentPackage;
    public GameObject present;

    public float presentPackageMovingSpeed;

    public int currentMovementState;
    public float count;

    public bool pause;

    public enum MovementState
    {
        First,
        Second,
        Third,
    }

    public AudioSource audioSource;
    public AudioClip flyingSound;
    public AudioClip equipSound;
    public bool equipSoundPlayed;

    // Use this for initialization
    void Start()
    {
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (!pause)
        {
            FirstMoving();
            SecondMoving();
            ThirdMoving();
        }
    }

    public virtual void Setup()
    {
        currentMovementState = (int)MovementState.First;
        audioSource.clip = flyingSound;
        audioSource.Play(0);
        equipSoundPlayed = false;
        count = 0;
		pause = false;
    }

    public virtual void FirstMoving()
    {
        if (currentMovementState == (int)MovementState.First)
        {
            if (count < 1.0f)
            {
                count += Time.deltaTime;
                Vector3 m1 = Vector3.Lerp(spawnLocation, middleFirstEndLocation, count);
                Vector3 m2 = Vector3.Lerp(middleFirstEndLocation, firstEndMovingLocation, count);
                this.gameObject.transform.position = Vector3.Lerp(m1, m2, count);
            }
            else
            {
                this.GetComponent<SpriteRenderer>().enabled = false;
                presentPackage.GetComponent<SpriteRenderer>().enabled = true;
                count = 0;
                currentMovementState = (int)MovementState.Second;
            }
        }
    }

    public virtual void SecondMoving()
    {
        if (currentMovementState == (int)MovementState.Second)
        {
            if ((Mathf.Abs(transform.localPosition.x - sencondEndMovingLocation.x) > 0.01f) || (Mathf.Abs(transform.localPosition.y - sencondEndMovingLocation.y) > 0.01f))
            {
                Vector3 moveVector = (sencondEndMovingLocation - transform.localPosition).normalized;
                transform.localPosition += moveVector * presentPackageMovingSpeed * Time.deltaTime;
            }
            else
            {
                presentPackage.GetComponent<SpriteRenderer>().enabled = false;
                present.GetComponent<SpriteRenderer>().enabled = true;
                currentMovementState = (int)MovementState.Third;
                count = 0;
            }
        }
    }

    public virtual void ThirdMoving()
    {
        if (currentMovementState == (int)MovementState.Third)
        {
            if (count < 1.0f)
            {
                count += Time.deltaTime;
                Vector3 m1 = Vector3.Lerp(sencondEndMovingLocation, middleSecondEndLocation, count);
                Vector3 m2 = Vector3.Lerp(middleSecondEndLocation, playerLocation, count);
                this.gameObject.transform.position = Vector3.Lerp(m1, m2, count);
            }
            else
            {
                present.GetComponent<SpriteRenderer>().enabled = false;
                PlayerStateControl.Instance.health = 3;
                PlayerStateControl.Instance.currentMovementState = (int)PlayerStateControl.MovementState.Idle;
                PlayerStateControl.Instance.currentArmorState = (int)PlayerStateControl.ArmorState.Full;
                PlayerAnimationControl.Instance.SetMovementState(PlayerStateControl.Instance.currentMovementState);
                PlayerAnimationControl.Instance.SetArmorState(PlayerStateControl.Instance.currentArmorState);
                if (equipSoundPlayed == false)
                {
                    audioSource.clip = equipSound;
                    audioSource.Play(0);
                    equipSoundPlayed = true;
                }
                EnemySpawnManager.Instance.ArmorGiverUnPause();
				if (!audioSource.isPlaying)
                    Destroy(this.gameObject);
            }
        }
    }

    public virtual void Pause()
    {

        pause = true;

    }

    public virtual void UnPause()
    {
        pause = false;
    }
}
