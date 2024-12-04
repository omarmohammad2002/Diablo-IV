using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionsMainManagement : MinionsDemonsMainManagement
{
    // Start is called before the first frame update
    new void Start()
    {
        enemyType = "Minion";
        maxHealth = 20;
        attackPower = 5;
        xpReward = 10;
        moveSpeed = 2f;
        currentState = EnemyState.Idle;
        base.Start();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
