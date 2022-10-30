using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Album", menuName = "Create Album")]
/**
 * @memo 2022
 * Scriptable objact for albums
 */
public class AlbumObject : ScriptableObject
{
    public string AlbumName;
    public string Artist;
    public Sprite AlbumCover;
    public SongObject[] songs;
    public string[] tags;
}
