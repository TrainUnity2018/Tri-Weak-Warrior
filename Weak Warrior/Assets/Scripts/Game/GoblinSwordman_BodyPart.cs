using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinSwordman_BodyPart : MonoBehaviour
{

    public bool boundTouched;


    // Use this for initialization
    void Start()
    {
        boundTouched = false;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D col)
    {
        OnCollide(col);
    }

    void OnCollide(Collider2D col)
    {
		if (col.gameObject.tag == "Bound")
			boundTouched = true;
    }
}
