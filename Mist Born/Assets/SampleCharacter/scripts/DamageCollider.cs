using UnityEngine;

public class DamageCollider : MonoBehaviour
{

    public FSM_CharMov player;
    public GameObject Enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            player.healthSlider.UpdateSliderValue(30);

            //info for the character knockback
            if (Enemy.transform.position.x - player.gameObject.transform.position.x > 0)//enemy is right to the player
            {
                player.attackingEnemyDir = -1.0f;
            }
            else
            {
                player.attackingEnemyDir = 1.0f;
            }
            player.attackGameObj.SetActive(false);
            player.ChangeState(player.knockback);


        }
    }
}
