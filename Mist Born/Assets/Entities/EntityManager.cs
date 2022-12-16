using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{

    public GameObject enemytest;

    public int enemiesNearByNum;
    public int enemiesActiveNum;

    public List<Entity> enemiesActive_List;
    // Start is called before the first frame update
    void Start()
    {
        enemytest = GetComponent<GameObject>();
        enemiesNearByNum = 0;
        enemiesActiveNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int enemiesNearBy()
    {
        return 1;
    }

    public void requestStateChange(State newState, Enemy_0 enemy)
    {
        switch(newState){
            case State.wandering:
                {
                    enemy.previousState = enemy.currentState;
                    enemy.currentState = newState;
                    break;
                }
            case State.chasing:
                {
                    if(enemiesActiveNum <= 3)
                    {
                        enemy.previousState = enemy.currentState;
                        enemy.currentState = newState;
                    }
                    else
                    {
                        enemy.previousState = enemy.currentState;
                        enemy.currentState = State.waitManagerOrders;
                    }
                    break;
                }
            case State.attacking:
                {
                    if (enemiesActiveNum < 3)
                    {
                        enemy.previousState = enemy.currentState;
                        enemy.currentState = newState;
                    }
                    else
                    {
                        enemy.previousState = enemy.currentState;
                        enemy.currentState = State.waitManagerOrders;
                    }
                    break;
                }
            case State.die:
                {
                    enemy.previousState = enemy.currentState;
                    enemy.currentState = newState;
                    break;
                }
            case State.waitManagerOrders:
                {
                    enemy.previousState = enemy.currentState;
                    enemy.currentState = newState;
                    break;
                }
        }
        

        
    }

}
