using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioHandler : MonoBehaviour
{
    //robot
    public AudioClip robotGound, robotAmbient, robotConfirm1, robotConfirm2, robotAlarm;

    //units
    public AudioClip unitReady1, unitReady2, unitConfirm1, unitConfirm2, confirmPing, unitDead,
                        dropOffClop, pickaxeClang;

    //enemies
    public AudioClip enemyAttack1, enemyAttack2, enemyDead1, enemyDead2;

    //shooting and stuff
    public AudioClip smallGun, smallLaz, bigGun, bigLaz, explosion, metalHit, machineGun, railGun, smallEchoBoom;


    public void PlayClip(GameObject source, string clip)
    {
        AudioSource audio;
        if (source.GetComponent<AudioSource>() == null)
            audio = Camera.main.GetComponent<AudioSource>();
        else
            audio = source.GetComponent<AudioSource>();

        if (audio.isPlaying)
        {
            Debug.Log("something is already playing from "+source);
            return;
        }

        AudioClip clipVar = (AudioClip)this.GetType().GetField(clip).GetValue(this);

        if (clipVar == null)
            return;

        audio.PlayOneShot(clipVar);
    }

    public void PlayClipIgnore(GameObject source, string clip)
    {
        AudioSource audio;
        if (source.GetComponent<AudioSource>() == null)
            audio = Camera.main.GetComponent<AudioSource>();
        else
            audio = source.GetComponent<AudioSource>();

        AudioClip clipVar = (AudioClip)this.GetType().GetField(clip).GetValue(this);

        if (clipVar == null)
            return;

        audio.PlayOneShot(clipVar);
    }
}
