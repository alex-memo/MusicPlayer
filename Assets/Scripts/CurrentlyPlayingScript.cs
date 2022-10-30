using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentlyPlayingScript : MonoBehaviour
{

    private float fadeAwayTime=5f;
    public Image Panel;
    public Text songName;
    public Text artist;
    public Image cover;
    //public AlbumObject album;
    public SongObject song;
    Color pa;
    Color sn;
    Color ar;
    Color co;
    private void Start()
    {
        pa = Panel.color;
        sn = songName.color;
        ar = artist.color;
        co = cover.color;
    }
    public void setCurrentPlaying(SongObject song, AlbumObject album)
    {
        //this.album = album;
        this.song = song;
        songName.text = song.name;
        artist.text = song.Artist;
        cover.sprite = song.SongCover;
        this.gameObject.SetActive(true);
        
    }
    public void noMusicPlaying()
    {
        this.gameObject.SetActive(false);
        //StartCoroutine(fadeAway());
    }
    private IEnumerator fadeAway()
    {
        
        Color a = Panel.color;
        Color b = songName.color;
        Color c = artist.color;
        Color d = cover.color;
        a.a = 0;
        b.a = 0;
        c.a = 0;
        d.a = 0;
        Panel.color=Color.Lerp(pa,a,fadeAwayTime);
        songName.color = Color.Lerp(sn, b, fadeAwayTime);
        artist.color = Color.Lerp(ar, c, fadeAwayTime);
        cover.color = Color.Lerp(co, d, fadeAwayTime);
        yield return new WaitForSeconds(fadeAwayTime);
        this.gameObject.SetActive(false);
        Panel.color = pa;
        songName.color = sn;
        artist.color = ar;
        cover.color = co;

    }
}
