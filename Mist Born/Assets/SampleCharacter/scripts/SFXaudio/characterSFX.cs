using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
public class characterSFX : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip footStepsStone;
    public AudioClip swordHitMetal;
    public AudioClip swordSlash1;
    public AudioClip swordSlash2;
    public AudioClip swordSlash3;
    public AudioClip dash1;
    public AudioClip jump;
    public AudioClip errorAttack;

    public FSM_CharMov my_sm;
    private bool playingAudioClip;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playingAudioClip = false;
        //audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {

        if (my_sm.getCurrState() == my_sm.run && !playingAudioClip)
        {
            audioSource.Play();
            audioSource.loop = true;
            playingAudioClip = true;
        }
        else if (my_sm.getCurrState() != my_sm.run)
        {
            audioSource.loop = false;
            playingAudioClip = false;
        }
    }

   public void playSound(AudioClip audio,bool loop = false)
    {
        audioSource.PlayOneShot(audio);

    }

}
