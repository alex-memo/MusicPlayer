using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class SaveData
{

    public string stringData;
    //public int[] noPlayed;
    //public int[] skipped;
    public float volume;
    public bool isShuffle;

    public SaveData(MainController Controller)
    {
        //noPlayed = Controller.saveData.noPlayed;
        //skipped = Controller.saveData.skipped;
        volume=Controller.getVolume();
        isShuffle = Controller.getShuffle();
        stringData =  Controller.getSaveData().getDataAsString();
    }

}
