using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using gamespace;


public class MusicManager : MonoBehaviour
{
    public AudioClip[] songs;
    private AudioClip good_song;
    private AudioClip bad_song;

    private AudioSource audioSource;

    private int good_song_timer;
    private int bad_song_timer;

    private string active_song;

    void OnEnable()
    {
        WorldController.OnWorldChanged += UpdateWorld;
    }
    void OnDisable()
    {
        WorldController.OnWorldChanged -= UpdateWorld;
    }
    
    void UpdateWorld(WorldType type){
        if(type == WorldType.GoodWorld){
            playWorld("good");
        }
        if(type == WorldType.BadWorld){
            playWorld("bad");
        }
    }

    void Start()
    {

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;

        good_song = songs[0];
        bad_song = songs[1];

        good_song_timer = 0;
        bad_song_timer = 0;

        audioSource.clip = good_song;
        audioSource.Play();
        active_song = "good";

        InvokeRepeating("counter", 1.0f, 1.0f);
    }

    public void playWorld(string world_type){
        if(world_type == "good"){
            active_song = "good";
            audioSource.clip = good_song;
            if((float)good_song_timer < good_song.length - 10.0f){
                audioSource.time = good_song_timer;
            }else{
                good_song_timer = 0;
            }
            audioSource.Play();
        }
        if(world_type == "bad"){
            active_song = "bad";
            audioSource.clip = bad_song;
            if((float)bad_song_timer < bad_song.length - 10.0f){
                audioSource.time = bad_song_timer;
            }
            else{
                bad_song_timer = 0;
            }
            audioSource.Play();
        }
    }

    void counter()
    {   
        if(active_song == "good"){
            good_song_timer += 1;
            if(good_song_timer > good_song.length -1){
                good_song_timer = 0;
            }
        }
        if(active_song == "bad"){
            bad_song_timer += 1;
            if(bad_song_timer > bad_song.length -1){
                bad_song_timer = 0;
            }
        }
    }
}
