using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/**
* @memo 2022
* script in charge of the dong discover in fyp
*/

public class SongDiscoverScript : MonoBehaviour
{
    SongObject song;
    /**
* @memo 2022
* plays song in this object
*/
    public void OpenSong()
    {
        MainController.Instance.playSong(song,null);
    }
    /**
* @memo 2022
* sets the song in this object
*/
    public void SetSong(SongObject NewSong)
    {
        song = NewSong;
        GetComponentInChildren<Image>().sprite = song.SongCover;  
        GetComponentInChildren<Text>().text = song.SongName;
    }
}
