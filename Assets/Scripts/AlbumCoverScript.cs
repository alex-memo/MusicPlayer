using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AlbumCoverScript : MonoBehaviour
{
    private AlbumObject Album;
    public void OpenAlbum()
    {
        MainController.Instance.changetoAlbumView(Album);
    }
    public void SetAlbum(AlbumObject NewAlbum)
    {
        Album = NewAlbum;
        GetComponent<Image>().sprite = Album.AlbumCover;
    }
}
