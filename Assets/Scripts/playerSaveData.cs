using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerSaveData : MonoBehaviour
{
    SongObject[] songs;
    public int[] noPlayed;
    public int[] skipped;



    /**
* @memo 2022
* saves the values from this song skipped times and played times
*/
    public void SaveValues(SongObject[] songs)
    {
        this.songs = songs;
        noPlayed=new int[songs.Length];
        skipped=new int[songs.Length];
    }
    /**
* @memo 2022
* loads values from file
*/
    public void LoadValues(string stringData)//(int[]noP,int[]ski)//(string stringData)
    {
        List<int> np=new List<int>();
        List<int> sk = new List<int>();

        SaveValues(MainController.Instance.GetAllSongs());
        string[] lines = stringData.Split('\n');
        int i = 0;
        while(i< lines.Length)
        {
            if (lines[i].Contains("ID "))
            {
                np.Add( int.Parse(lines[i + 1]));
                sk.Add( int.Parse(lines[ i + 2]));
            }
            i++;
        }
        i = 0;
        while(i < sk.Count)
        {
            noPlayed[i] = np[i];
            skipped[i] = sk[i];
            i++;
        }
        
    }
    /**
* @memo 2022
* adds skipped to song
*/
    public void addSkipped(SongObject song)
    {
        skipped[FindSong(song)] += 1;
    }
    /**
* @memo 2022
* adds played to song
*/
    public void addPlayed(SongObject song)
    {
        noPlayed[FindSong(song)] += 1;
    }
    /**
* @memo 2022
* finds a song in the file
*/
    private int FindSong(SongObject song)
    {
        int i = 0;
        while(i < songs.Length)
        {
            if(songs[i] == song)
            {
                return i;
            }
            i++;
        }
        return -1;//song not found
    }
    /**
* @memo 2022
* gets all data as string
*/
    public string getDataAsString()
    {
        string data = "\n";
        for(int i = 0; i < songs.Length; i++)
        {
            data+="ID "+songs[i].ID+"\n"+noPlayed[i]+"\n"+skipped[i]+"\n";
            
        }
        return data;
    }
    /**
* @memo 2022
* gets the most played songs the number of songs returned is defined by sending it to the method
*/
    public SongObject[] getMostPlayed(int length)
    {
        SongObject[] mostPlayed = new SongObject[length];
        int i = 0;
        while (i < length)
        {
            
            mostPlayed[i] = songs[i];
            //Debug.LogWarning(mostPlayed[i]);//works, gets first 10 songs
            i++;
        }
        i = 0;//i loops though most played
        while (i < mostPlayed.Length)
        {
            int j = 0;//j loops through all songs
            while(j < songs.Length)
            {
                if (noPlayed[j]-skipped[j]>noPlayed[int.Parse(mostPlayed[i].ID)]-skipped[int.Parse(mostPlayed[i].ID)])
                     //if the song selected has more plays
                     //than the one in most played
                                           
                {
                    Debug.LogWarning(songs[j].SongName);
                    Debug.Log(songs[int.Parse(mostPlayed[i].ID)]);
                    int k = 0; bool add = true;
                    while (k < mostPlayed.Length)
                    {
                        //print(songs[j].SongName);
                        //print(mostPlayed[i].SongName);
                        if(songs[j] == mostPlayed[k]) //checks if song is alr added
                        {
                            //repeated song
                            add = false;
                        }
                        k++;
                    }
                    if (add == true)
                    {
                        mostPlayed[i] = songs[j];//then swap that one out
                    }
                    
                }
                j++;
            }


            i++;
        }
        return mostPlayed;
    }
}
