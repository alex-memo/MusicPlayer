using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/**
* @memo 2022
* handler for the play viewer, current song viewer
*/
public class PlayerViewScript : MonoBehaviour
{
    public Text songName;
    public Text artist;
    public Image cover;
    public SongObject song;

    public Image PlayPauseButton;
    public Sprite PlayButton;
    public Sprite PauseButton;
    /**
* @memo 2022
* sets the currenlt playing song
*/
    public void setCurrentPlaying(SongObject song)
    {
        //this.album = album;
        this.song = song;
        songName.text = song.name;
        artist.text = song.Artist;
        cover.sprite = song.SongCover;
        //this.gameObject.SetActive(true);

    }
    /**
* @memo 2022
* sets play pause
*/
    public void setPlayPause(bool isPlay)
    {
        if (isPlay)
        {
            PlayPauseButton.sprite = PauseButton;
        }
        else
        {
            PlayPauseButton.sprite = PlayButton;
        }
    }
}
