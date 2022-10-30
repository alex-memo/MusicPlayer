using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongDiscoverScript : MonoBehaviour
{
    SongObject song;
    public void OpenSong()
    {
        MainController.Instance.playSong(song,null);
    }
    public void SetSong(SongObject NewSong)
    {
        song = NewSong;
        GetComponentInChildren<Image>().sprite = song.SongCover;  
        GetComponentInChildren<Text>().text = song.SongName;
    }
}
