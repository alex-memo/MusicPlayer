using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "New Song", menuName = "Create Song")]
public class SongObject : ScriptableObject
{
    public string SongName;
    public string Artist;
    public string feauture;
    public Sprite SongCover;
    public AudioClip song;
    public string[] tags;
    public string ID;
}
