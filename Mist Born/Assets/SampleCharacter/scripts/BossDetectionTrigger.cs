using UnityEngine;

public class BossDetectionTrigger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public EnemyBoss boss;

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("Player"))
        {
            boss.playerDetected = true;
            Debug.Log("DETECTED THE PLAYERR");
            //MusicManager.Instance.PlayMusic(MusicManager.Instance.battleTheme);
            gameObject.SetActive(false);
        }

    }
    
}
