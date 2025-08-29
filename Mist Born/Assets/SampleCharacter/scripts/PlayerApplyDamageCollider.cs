using UnityEngine;

public class PlayerApplyDamageCollider : MonoBehaviour
{
    public FSM_CharMov player;
    public EnemyBoss Enemy;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Enemy.healthSlider.UpdateSliderValue(30);

            //here effect of hit


        }
    }
}
