using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioClip windup;
    public AudioClip flinch;
    AudioClip[] clips;

    private AudioSource source;
    
    int ran = Random.Range(0, 3);
 

    void Awake ()
    {
        source = GetComponent<AudioSource>();
        clips = new AudioClip[] {(AudioClip)Resources.Load("Audio/Effects/NFF-metal-hit.wav"),
                                (AudioClip)Resources.Load("Audio/Effects/NFF-bionic-claw.wav"),
                                (AudioClip)Resources.Load("Audio/Effects/NFF-thud.wav"),
                                (AudioClip)Resources.Load("Audio/Effects/NFF-cyborg-move.wav")};
    }
	
	public void PlayWindUp ()
    {       
        
        source.PlayOneShot(windup, 1f); 
        
	}

    public void punch()
    {
        source.clip = clips[ran];
        source.Play();
    }

    public void Flinch()
    {
        source.clip = flinch;
        source.Play();
    }
}
