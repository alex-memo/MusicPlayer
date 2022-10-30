using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongContentScript : MonoBehaviour
{
    public Text SongNameText;
    public Text SongArtistText;
    public Image SongCover;
    public SongObject song;
    public AlbumObject thisAlbum;
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
    public void PlaySong()
    {
        MainController.Instance.playSong(song,thisAlbum);
    }
    public void DestroySongObj()
    {
        Destroy(this.gameObject);
    }
}
