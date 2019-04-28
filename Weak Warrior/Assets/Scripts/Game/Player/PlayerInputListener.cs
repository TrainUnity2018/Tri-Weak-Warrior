using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputListener : MonoSingleton<PlayerInputListener>
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        InputHandle();
    }

    public void Attack(bool direction) //true for left, false for right
    {
        PlayerStateController.Instance.Attack(direction);
    }

    public void Skill()
    {
        PlayerStateController.Instance.Skill();
    }

    public void InputHandle()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Attack(true);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Attack(false);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Skill();
        }
    }
}
