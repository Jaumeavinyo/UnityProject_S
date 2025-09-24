using UnityEngine;

public class BossAnimEventManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public EnemyBoss boss;

    void Start()
    {
        
    }

   


    public void GroundSlamColliderActivation()
    {
        boss.groundSlamCollider.enabled=true;
    }
    public void GroundSlamColliderDeactivation()
    {
        boss.groundSlamCollider.enabled = false;
    }

    public void GroundSlamGetDamageColliderPositionChange(float offsetX)
    {
        boss.SetDamageColliderOffsetChange(offsetX);
    }

    public void FireThrowerColliderActivation()
    {
        boss.fireThrowerCollider.enabled = true;
    }
    public void FireThrowerColliderDeactivation()
    {
        boss.fireThrowerCollider.enabled = false;
    }

    public void JumpSlamColliderActivation()
    {
        boss.jumpSlamCollider.enabled = true;
    }
    public void JumpSlamColliderDeactivation()
    {
        boss.jumpSlamCollider.enabled = false;
    }

    public void BossLegSlamSFX()
    {
        MusicManager.Instance.PlaySFX(MusicManager.Instance.MonsterSlam);
    }
    public void BossFireThrower()
    {
        MusicManager.Instance.PlaySFX(MusicManager.Instance.MonsterFireThrower);
    }

    public void BossDeath()
    {
        
        MusicManager.Instance.PlaySFX(MusicManager.Instance.MonsterDeath);
    }

}
