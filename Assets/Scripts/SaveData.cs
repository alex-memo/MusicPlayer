using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
/**
* @memo 2022
* script to only call one thing when saving
*/
public class SaveData
{

    public string stringData;
    //public int[] noPlayed;
    //public int[] skipped;
    public float volume;
    public bool isShuffle;
    /**
* @memo 2022
* calls script that saves the data in file
*/
    public SaveData(MainController Controller)
    {
        //noPlayed = Controller.saveData.noPlayed;
        //skipped = Controller.saveData.skipped;
        volume=Controller.getVolume();
        isShuffle = Controller.getShuffle();
        stringData =  Controller.getSaveData().getDataAsString();
    }

}
