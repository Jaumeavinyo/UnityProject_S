using UnityEngine;

public class PlayerApplyDamageCollider : MonoBehaviour
{
    public FSM_CharMov player;
    public EnemyBoss Enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Collided with: " + collision.name);
            if (Enemy.animator.GetCurrentAnimatorStateInfo(0).IsName("Boss_LegSlam"))
            {
                Enemy.healthSlider.UpdateSliderValue(8);
                StartCoroutine(player.FreezeAnimation(0.2f));
                StartCoroutine(Enemy.FreezeAnimation(0.2f));
                
            }
            else
            {
                Enemy.healthSlider.UpdateSliderValue(3);
                StartCoroutine(player.FreezeAnimation(0.2f));
                StartCoroutine(Enemy.FreezeAnimation(0.2f));
                
            }


            StartCoroutine(Enemy.SpriteWhiteFlash(0.2f));

            if (player.attack.currCombo1Attack == 2)//last attack of the 3 attack combo (typeheavy)
            {
                Enemy.changeBossState(BossState.JUMP);
            }

        }
    }
}
