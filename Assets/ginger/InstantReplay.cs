using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantReplay : MonoBehaviour
{
    public struct Action
    {
        public float time;
        public int player;
        public string action;
        public bool counter;
        public bool combo;        
    }

    float timeStart;
    List<Action> actions;
    int playbackIndex;
    public PlayerController p1, p2;
    float playbackTime;
    bool isPlaying;
    
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void FixedUpdate ()
    {
		
	}



    public void StartRecording()
    {
        timeStart = Time.time;
        actions.Clear();
    }

    public void RecordAction(int player, string action, bool iscounter, bool iscombo)
    {
        Action t = new Action();

        t.player = player;
        t.action = action;
        t.counter = iscounter;
        t.combo = iscombo;
        t.time = Time.time - timeStart;

        actions.Add(t);
    }


    public void PlayRecording(float matchTime = 0)
    {
        playbackTime = Time.time - matchTime;
        playbackIndex = 0;
        isPlaying = true;
    }

    void Update()
    {
        if (!isPlaying) return;

        float relTime = Time.time - playbackTime;
        
        while(relTime >= actions[playbackIndex].time)
        {

            var t = actions[playbackIndex];
            //if (t.player == 1)
                //p1.DoPlaybackPunch(t.action, t.counter, t.combo);
            //else
               // p2.DoPlaybackPunch(t.action, t.counter, t.combo);


            playbackIndex++;
        }
        if (playbackIndex >= actions.Count)
            isPlaying = false;
    }



    ////////////////////////
    // Add to PlayerController.cs

    //public InstantReplay IRSytem;

    //public void DoRecordPunch(string stateName, bool isCounter = false, bool isCombo = false)
    //{
    //    DoPunch(stateName, isCounter, isCombo);
    //    IRSytem.RecordAction(playerLayer, stateName, isCounter, isCombo);
    //}

    //public void DoPlaybackPunch(string stateName, bool isCounter = false, bool isCombo = false)
    //{
    //    DoPunch(stateName, isCounter, isCombo);
    //}
}
