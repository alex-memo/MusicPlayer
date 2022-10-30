using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/**
* @memo 2022
* Script to handle the content inside of a song object
*/
public class SongContentScript : MonoBehaviour
{
    public Text SongNameText;
    public Text SongArtistText;
    public Image SongCover;
    public SongObject song;
    public AlbumObject thisAlbum;
    /**
* @memo 2022
* sets song in this object
*/
    public void SetSong(SongObject song, AlbumObject album)
    {
        SongNameText.text = song.SongName;
        SongArtistText.text = song.Artist;
        thisAlbum = album;
        if (!song.feauture.Equals(""))
        {
            SongArtistText.text += " ft." + song.feauture;
        }
        if(song.SongCover != null)
        {
            SongCover.sprite = song.SongCover;
        }
        else
        {
            song.SongCover = album.AlbumCover;
            SongCover.sprite = song.SongCover;
        }
        this.song = song;
    }
    /**
* @memo 2022
* plays the song in this object
*/
    public void PlaySong()
    {
        MainController.Instance.playSong(song,thisAlbum);
    }
    /**
* @memo 2022
* destroys this object
*/
    public void DestroySongObj()
    {
        Destroy(this.gameObject);
    }
}
