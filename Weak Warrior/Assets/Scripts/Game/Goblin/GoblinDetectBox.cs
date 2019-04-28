using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinDetectBox : DetectBox {

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void OnCollide(Collider2D col)
    {
        if (col.gameObject.tag == "Player")
        {
            actor.GetComponent<Goblin>().Attack();
        }
    }
}
