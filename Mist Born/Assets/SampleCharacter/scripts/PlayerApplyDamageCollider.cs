using UnityEngine;

public class PlayerApplyDamageCollider : MonoBehaviour
{
    public FSM_CharMov player;
    public EnemyBoss Enemy;
    public SmartCamera SmartCam;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            //Debug.Log("Collided with: " + collision.name);
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

            if(player.directionInput != 0)//input exists now
            {
                if(player.directionInput == 1)//right dir facing attack
                {
                    player.audioSFX.playSound(player.audioSFX.swordHitMetal);
                    SmartCam.DirectionalShake(0.1f, 15, 0.0f, 0.5f, 0.2f, 0.0f);
                }else if (player.directionInput == -1)//left dir facing attack
                {
                    player.audioSFX.playSound(player.audioSFX.swordHitMetal);
                    SmartCam.DirectionalShake(0.1f, 15, 0.5f, 0.0f, 0.2f, 0.0f);
                }
            }else if(player.directionInput == 0)//no input now
            {
                if (player.lastDirectionInput == 1)//right dir facing attack
                {
                    player.audioSFX.playSound(player.audioSFX.swordHitMetal);
                    SmartCam.DirectionalShake(0.1f, 15, 0.0f, 0.5f, 0.2f, 0.0f);
                }
                else if (player.lastDirectionInput == -1)//left dir facing attack
                {
                    player.audioSFX.playSound(player.audioSFX.swordHitMetal);
                    SmartCam.DirectionalShake(0.1f, 15, 0.5f, 0.0f, 0.2f, 0.0f);
                }
            }
            

            if (player.attack.currCombo1Attack == 2)//last attack of the 3 attack combo (typeheavy)
            {
                Enemy.changeBossState(BossState.JUMP);
            }

        }
    }
}
