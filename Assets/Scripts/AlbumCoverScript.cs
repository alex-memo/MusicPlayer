using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/**
 * @memo 2022
 * Script to handle album covers
 */
public class AlbumCoverScript : MonoBehaviour
{
    private AlbumObject Album;
    /**
     * @memo 2022
     * Open ablum method, called from clicking this album
     */
    public void OpenAlbum()
    {
        MainController.Instance.changetoAlbumView(Album);
    }
    /**
 * @memo 2022
 * sets the current album
 */
    public void SetAlbum(AlbumObject NewAlbum)
    {
        Album = NewAlbum;
        GetComponent<Image>().sprite = Album.AlbumCover;
    }
}
