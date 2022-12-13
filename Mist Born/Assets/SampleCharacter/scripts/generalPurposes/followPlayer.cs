using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{

    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        //player = GetComponent<GameObject>();

        if(player == null)
        {
            Debug.Log("FollowPlayer.cs:  player gameObject not found or null");
        }
    }
    private void FixedUpdate()
    {
        Vector2 follow = new Vector2(player.transform.position.x, player.transform.position.y);

        this.transform.position = follow;
    }

   
}
