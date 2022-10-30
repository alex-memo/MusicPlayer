using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerViewScript : MonoBehaviour
{
    public Text songName;
    public Text artist;
    public Image cover;
    public SongObject song;

    public Image PlayPauseButton;
    public Sprite PlayButton;
    public Sprite PauseButton;
    public void setCurrentPlaying(SongObject song)
    {
        //this.album = album;
        this.song = song;
        songName.text = song.name;
        artist.text = song.Artist;
        cover.sprite = song.SongCover;
        //this.gameObject.SetActive(true);

    }
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
