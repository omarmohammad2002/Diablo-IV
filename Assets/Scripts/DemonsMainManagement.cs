using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonsMainManagement : MinionsDemonsMainManagement
{
    public int explosivePower;
   new void Start()
    {
        enemyType = "Demon";
        maxHealth = 40;
        attackPower = 10;
        explosivePower = 15;
        xpReward = 30;
        moveSpeed = 2.5f;
        currentState = EnemyState.Patrolling;
        base.Start();
        
    }
}
