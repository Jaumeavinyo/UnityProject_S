using UnityEngine;

public class AnimEventManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    public FSM_CharMov character;

    void Start()
    {
        attackColliderDeactivationEvent();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void attackColliderActivationEvent()
    {
        character.attack.ActivateAttackCollider();//already knows wich attack should be called, no more info needed
    }
    public void attackColliderDeactivationEvent()
    {
        character.attack.DeactivateAttackCollider();//already knows wich attack should be called, no more info needed
    }
}
