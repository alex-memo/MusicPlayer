using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchPageScript : MonoBehaviour
{
    public Transform SongsHolder;
    public GameObject SongContentViewPrefab;


    public void Search(string name)
    {
        name = name.ToLower();
        DeletePageSongs();
        int i = 0;
        SongObject[] songs=MainController.Instance.GetAllSongs();
        foreach (SongObject song in songs)
        {
            if (song.SongName.ToLower().Contains(name))
            {
                GameObject a = Instantiate(SongContentViewPrefab, SongsHolder);
                a.GetComponent<SongContentScript>().SetSong(song, null);
                i++;
            }
        }
        SongsHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(SongsHolder.GetComponent<RectTransform>().sizeDelta.x, 100 * i);
       

    }
    private void DeletePageSongs()
    {
        foreach (Transform child in SongsHolder.transform)
        {
            child.GetComponent<SongContentScript>().DestroySongObj();
        }

    }
}
