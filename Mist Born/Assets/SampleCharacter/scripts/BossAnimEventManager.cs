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

    public void FireThrowerColliderActivation()
    {

    }
    public void FireThrowerColliderDeactivation()
    {

    }

    public void JumpSlamColliderActivation()
    {

    }
    public void JumpSlamColliderDeactivation()
    {

    }

}
