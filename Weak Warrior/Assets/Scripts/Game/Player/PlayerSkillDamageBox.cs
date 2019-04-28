using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkillDamageBox : MonoBehaviour {

    public List<Goblin> monsterList;
    //public List<DarkTree_Arm> bossList;   

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
		Skill();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            if (col.gameObject.GetComponent<Goblin>() != null)
            {
                Goblin monster = col.gameObject.GetComponent<Goblin>();
                monsterList.Add(monster);
            }

            // if (col.gameObject.GetComponent<DarkTree_Arm>() != null)
            // {
            //     DarkTree_Arm boss = col.gameObject.GetComponent<DarkTree_Arm>();
            //     bossList.Add(boss);
            // }
        }
    }

    public void Skill()
    {
        for (int i = 0; i < monsterList.Count; i++)
        {
            monsterList[i].TakeDamage(PlayerStateController.Instance.currentMovementState);
        }

        monsterList.RemoveAll(Goblin => Goblin != null);
        monsterList.RemoveAll(Goblin => Goblin == null);
    }
}
