using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Text : MonoSingleton<UI_Text>
{
    public AudioSource audioSource;
    public AudioClip missSound;
    public bool missSoundPlayed;

    public GameObject missedText;

    // Use this for initialization
    void Start()
    {
        missedText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void EnableMissedText()
    {
        missedText.SetActive(true);
    }

    public void DisableMissedText()
    {
        missedText.SetActive(false);
    }

    public void LoadMissSound()
    {
        missSoundPlayed = false;
        audioSource.clip = missSound;
        Debug.Log(missSoundPlayed);
    }

    public void MissSound()
    {
        if (missSoundPlayed == false)
        {
            audioSource.Play(0);
            missSoundPlayed = true;
        }
    }
}
