using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
    #region InstanceRegion
    public static MainController Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    #endregion

    public AudioSource musicPlayer;
    [SerializeField]
    List<SongObject> playedMusic;
    [SerializeField]
    int musicCount=-1;

    public GameObject DiscoverPage;
    public GameObject AlbumSongsPage;
    public GameObject MusicPlayerView;

    public GameObject SongsPageSongsHolder;
    public Image AlbumCoverPageCover;
    public Text AlbumText;

    public GameObject AlbumSongsPageBig;
    public GameObject SongsPageSongsHolderBig;
    public Image AlbumCoverPageBigCover;
    public Text AlbumTextBig;

    public GameObject SongContentViewPrefab;

    public AlbumObject[] Albums;
    public Transform DiscoverAlbumsTransform;
    //public AlbumObject[] DiscoverAlbums;

    public Transform DiscoverSongsTransform;
    //public SongObject[] DiscoverSongs;

    private AlbumObject selectedAlbum;
    private bool isPlay = false;

    public CurrentlyPlayingScript CurrentlyPlayingScript;
    public PlayerViewScript PlayerViewScript;

    private float volume = .1f;

    private bool isShuffle = false;
    public Image ShuffleImage;
    public Slider VolSlider;
    public Slider SongTimeSlider;

    //private int noSongs;
    private playerSaveData saveData;
    List<SongObject> allSongs = new List<SongObject>();

    public Transform FavouriteSongsTransform;

    public GameObject searchPage;


    private void Start()
    {
        
        saveData=GetComponent<playerSaveData>();
        Application.targetFrameRate = 60;
        CreateAlbumsStart();
        CreateSongsStart();
        musicPlayer.volume = volume;
        VolSlider.value = volume;
        SetSongIDS();
        //ResetSongIDs();
        LoadData();
        CreateFavouriteSongs();
        
    }
    private void LoadData()
    {
        saveData.SaveValues(GetAllSongs());
        SaveData data = SaveSystem.loadPlayer();
        if(data != null)
        {
            GetComponent<playerSaveData>().LoadValues(data.stringData);
            volume = data.volume;
            isShuffle = data.isShuffle;
        }
        else//first time using app or deleted progress
        {
            
        }
    }




    private void audioFinished()
    {
        Debug.Log("audio finished");
        CurrentlyPlayingScript.noMusicPlaying();
        if (isPlay)
        {
            if (musicCount < playedMusic.Count - 1)//checks if there is something in queue
            {
                playNextSongInQueue();
            }
            else//if there is nothing on queue then get rand song or next in album
            {
                if (isShuffle||selectedAlbum==null)
                {
                    SongObject newRandSong = CurrentlyPlayingScript.song;
                    while(newRandSong == CurrentlyPlayingScript.song)
                    {
                        newRandSong = GetRandomSong(selectedAlbum);
                    }
                    playSong(newRandSong,selectedAlbum);
                }
                else//if shuffle is off
                {
                    int pos = getSongPosOnAlbum(CurrentlyPlayingScript.song, selectedAlbum);
                    if (pos != -1)
                    {
                        if (pos != selectedAlbum.songs.Length - 1)
                        {
                            playSong(selectedAlbum.songs[pos + 1], selectedAlbum);
                        }
                        else//album is over
                        {
                            //find another album from same artist and play it in order
                            AlbumObject a= getSameArtistAlbum(selectedAlbum);
                            if(a != null)//found another album from  same artist
                            {
                                selectedAlbum = a;
                                playSong(selectedAlbum.songs[0], selectedAlbum);
                            }
                            else
                            {
                                Debug.Log("No album from same artist found!");
                            }

                        }

                    }
                    else//the song was not found in album, will play random song on album
                    {
                        int rand = GetRandomSongPosition(selectedAlbum);
                        if (rand == pos)//gotten song is just played song then
                        {
                            while (rand == pos)
                            {
                                rand = GetRandomSongPosition(selectedAlbum);
                            }
                        }
                        playSong(selectedAlbum.songs[rand], selectedAlbum);
                    }
                }
                
            }


            
        }
    }
   



    #region buttons
    public void changetoDiscover()
    {
        MusicPlayerView.SetActive(false);
        AlbumSongsPage.SetActive(false);
        AlbumSongsPageBig.SetActive(false);
        searchPage.SetActive(false);

        //generate discover page
        Application.targetFrameRate = 60;
        DiscoverPage.SetActive(true);
    }
    public void changetoPlayerView()
    {
        AlbumSongsPage.SetActive(false);
        AlbumSongsPageBig.SetActive(false);
        DiscoverPage.SetActive(false);
        searchPage.SetActive(false);

        //set song perview

        PlayerViewScript.setPlayPause(isPlay);
        Application.targetFrameRate = 30;
        MusicPlayerView.SetActive(true);
        StartCoroutine(updateTimer());
    }
    public void changetoAlbumView(AlbumObject album)
    {
        MusicPlayerView.SetActive(false);
        DiscoverPage.SetActive(false);
        searchPage.SetActive(false);

        //set album songs

        //delete all songs from before
        DeletePageSongs();
        //set new album cover
        AlbumCoverPageCover.sprite = album.AlbumCover;
        AlbumText.text = album.AlbumName + "\n" + album.Artist;

        AlbumCoverPageBigCover.sprite = album.AlbumCover;
        AlbumTextBig.text = album.AlbumName + "\n" + album.Artist;

        //selectedAlbum = album;
        //set new songs
        int i = 0;
        while (i < album.songs.Length)
        {
            GameObject a = Instantiate(SongContentViewPrefab, SongsPageSongsHolder.transform);
            a.GetComponent<SongContentScript>().SetSong(album.songs[i], album);
            GameObject b = Instantiate(SongContentViewPrefab, SongsPageSongsHolderBig.transform);
            b.GetComponent<SongContentScript>().SetSong(album.songs[i], album);
            i++;
        }
        SongsPageSongsHolder.GetComponent<RectTransform>().sizeDelta = new Vector2(SongsPageSongsHolder.GetComponent<RectTransform>().sizeDelta.x, 100 * album.songs.Length);
        float y = 50 * album.songs.Length;
        if (y % 100 != 0)
        {
            y += 50;
        }
        SongsPageSongsHolderBig.GetComponent<RectTransform>().sizeDelta = new Vector2(SongsPageSongsHolder.GetComponent<RectTransform>().sizeDelta.x, y);

        if (Screen.width > Screen.height)//if on wide device
        {
            AlbumSongsPageBig.SetActive(true);
        }
        else//prob mobile
        {
            AlbumSongsPage.SetActive(true);
        }
        Application.targetFrameRate = 60;
    }
    public void ChangeToSearch()
    {
        AlbumSongsPage.SetActive(false);
        AlbumSongsPageBig.SetActive(false);
        DiscoverPage.SetActive(false);
        MusicPlayerView.SetActive(false);
        searchPage.SetActive(true);

    }
    public void pausePlay()
    {
        if (musicPlayer.isPlaying)
        {
            isPlay = false;
            CancelInvoke();
            print("invokeCancelled");
            //CurrentlyPlayingScript.noMusicPlaying();
            musicPlayer.Pause();
        }
        else
        {
            isPlay = true;
            musicPlayer.Play();
            Invoke("audioFinished", musicPlayer.clip.length - musicPlayer.time);
        }
        PlayerViewScript.setPlayPause(isPlay);

    }
    public void skipSong()
    {
        Debug.Log(musicCount);
        saveData.addSkipped(CurrentlyPlayingScript.song);
        SaveSystem.SavePlayer(this);
        if (musicCount < playedMusic.Count - 1)//if there is already something in queue
        {
            playNextSongInQueue();  
        }
        else//there is nothing on queue
        {
            Debug.Log("SkipSong, nothing on queue");//get random song on album or get next soing depending on mode user selects
            //check if shuffle on
            playSong(GetRandomSong(selectedAlbum),selectedAlbum);
        }
    }
    public void previousSong()
    {
        if (musicCount > 0)
        {
            musicPlayer.clip = playedMusic[musicCount-1].song;
            isPlay = true;
            musicPlayer.Play();
            SongTimeSlider.maxValue = musicPlayer.clip.length;
            SongTimeSlider.value = musicPlayer.time;
            CurrentlyPlayingScript.setCurrentPlaying(playedMusic[musicCount - 1], selectedAlbum);
            PlayerViewScript.setCurrentPlaying(CurrentlyPlayingScript.song);
            CancelInvoke();
            Invoke("audioFinished", musicPlayer.clip.length);
            musicCount--;
            saveData.addPlayed(CurrentlyPlayingScript.song);
            SaveSystem.SavePlayer(this);
        }
    }
    public void toggleShuffle()
    {
        if (isShuffle)
        {
            isShuffle = false;
            ShuffleImage.color = Color.white;
        }
        else
        {
            isShuffle = true;
            ShuffleImage.color=Color.black;
        }
    }
    public void VolumeSlider(float sliderValue)
    {
        musicPlayer.volume = sliderValue;
    }
    public void userUpdateSongTime(float sliderValue)
    {
        musicPlayer.time = sliderValue;
        CancelInvoke();
        Invoke("audioFinished", musicPlayer.clip.length - musicPlayer.time);
    }
    #endregion
    #region utility
    private void DeletePageSongs()
    {
        foreach (Transform child in SongsPageSongsHolder.transform)
        {
            child.GetComponent<SongContentScript>().DestroySongObj();
        }
        foreach (Transform child in SongsPageSongsHolderBig.transform)
        {
            child.GetComponent<SongContentScript>().DestroySongObj();
        }

    }
    private int getSongPosOnAlbum(SongObject song, AlbumObject album)
    {
        if(album == null)//song not existent on album
        {
            return -1;
        }
        int i = 0;
        while (i < album.songs.Length)
        {
            if (album.songs[i] == song)
            {
                return i;
            }
            i++;
        }
        return -1;//song not existent on album
    }
    public float getVolume()
    {
        return volume;
    }
    public void playSong(SongObject song, AlbumObject selectAlbum)
    {
        musicPlayer.clip = song.song;
        selectedAlbum = selectAlbum;
        isPlay = true;
        musicPlayer.Play();
        SongTimeSlider.maxValue = musicPlayer.clip.length;
        SongTimeSlider.value = 0;
        print(song.SongName);
        CurrentlyPlayingScript.setCurrentPlaying(song, selectAlbum);
        PlayerViewScript.setCurrentPlaying(CurrentlyPlayingScript.song);
        CancelInvoke();
        Invoke("audioFinished", musicPlayer.clip.length);
        playedMusic.Add(song);
        musicCount++;
        saveData.addPlayed(CurrentlyPlayingScript.song);
        SaveSystem.SavePlayer(this);
    }
    private void playNextSongInQueue()
    {
        musicPlayer.clip = playedMusic[musicCount + 1].song;
        isPlay = true;
        musicPlayer.Play();
        SongTimeSlider.maxValue = musicPlayer.clip.length;
        SongTimeSlider.value = musicPlayer.time;
        CurrentlyPlayingScript.setCurrentPlaying(playedMusic[musicCount + 1], selectedAlbum);
        PlayerViewScript.setCurrentPlaying(CurrentlyPlayingScript.song);
        CancelInvoke();
        Invoke("audioFinished", musicPlayer.clip.length);
        musicCount++;
        saveData.addPlayed(CurrentlyPlayingScript.song);
        SaveSystem.SavePlayer(this);
    }
    private int GetRandomSongPosition(AlbumObject album)//gets a random song position in an album
    {
        float rand = Random.Range(0, album.songs.Length);
        rand += .5f;
        int r = (int)rand;
        return r;
    }
    private int GetNewAlbum()//Gets a random album position from all the album list
    {
        float rand = Random.Range(0, Albums.Length);
        rand += .5f;
        int r = (int)rand;
        return r;
    }
    private AlbumObject getSameArtistAlbum(AlbumObject referenceAlbum)
    {
        int i = 0;
        while(i< Albums.Length)
        {
            if (referenceAlbum.Artist.Equals(Albums[i].Artist)&&referenceAlbum!=Albums[i])
            {
                return Albums[i];
            }
            i++;
        }
        return null;//means there where no albums with that artist found
    }
    private SongObject GetRandomSong(AlbumObject album)
    {
        
        if(album == null)//get random song from everything
        {
            int randAlbum = GetNewAlbum();//gets a random album
            int rand = GetRandomSongPosition(Albums[randAlbum]);//gets a random song from the album
            //print(Albums[randAlbum].AlbumName + " " + rand);
            return Albums[randAlbum].songs[rand];
        }
        else//get random song inside album
        {
            int rand = GetRandomSongPosition(album);
            return album.songs[rand];
        }
    }
    public void SongTimerTick()
    {
        SongTimeSlider.value = musicPlayer.time;
    }
    private IEnumerator updateTimer()
    {
        while (MusicPlayerView.gameObject.activeSelf)
        {
            SongTimerTick();
            yield return new WaitForSeconds(1f);
        }
        print("ended coroutine");
    }
    private void SetSongIDS()
    {
        //List<SongObject> songListNoID = new List<SongObject>();
        int id = 0;
        int i = 0;
        //int temp = -1;
        while (i< Albums.Length)
        {
            int j = 0; 
            while (j < Albums[i].songs.Length)
            {
                
                if (!allSongs.Contains(Albums[i].songs[j]))//if  allsongs does not contain the song selected
                {
                    allSongs.Add(Albums[i].songs[j]);
                    if (Albums[i].songs[j].ID.Equals(""))//if song has no id give one to it
                    {
                        Albums[i].songs[j].ID = "" + id;
                    }
                    id++;
                    /**
                    if (!Albums[i].songs[j].ID.Equals(""))//if song has id
                    {
                        allSongs.Add(Albums[i].songs[j]);
                    }
                    else//if song has no id
                    {
                        songListNoID.Add(Albums[i].songs[j]);
                    }
                    
                    //allSongs.Add(Albums[i].songs[j]);
                    // if (Albums[i].songs[j].ID.Equals(""))//if song has no id
                    //{
                    //    Albums[i].songs[j].ID = "" + id;
                    //}
                    //id++;
                    **/
                }
                /**
                if(Albums[i].songs[j].ID.Equals("")){//if it has no id then give an id to it
                    songListNoID.Add(Albums[i].songs[j]);
                    //print(Albums[i].songs[j].SongName);
                    id++;
                }
                else
                {
                    print(Albums[i].songs[j].ID);
                    id = int.Parse(Albums[i].songs[j].ID);
                    allSongs.Add(null);
                    print(id+" pass1");
                    allSongs[id] = Albums[i].songs[j];
                    print(id);
                    print (temp);
                    //if (id - 1 == temp)
                    //{
                    //    allSongs.Add(Albums[i].songs[j]);
                    //   temp = id;
                    //}
                    
                }
                **/
                j++;
            }
            i++;
        }
        /**
        i = 0;
        while (i < allSongs.Count)
        {
            if (allSongs[i] == null)
            {
                allSongs.Remove(allSongs[i]);
                i -= 1;
            }
            i++;
        }
        
        //i = 0;
        //print(id);
        //print(GetAllSongs()[id].SongName);
        
        while (i < songListNoID.Count)
        {
            id += 1;
            songListNoID[i].ID = "" + id;
            allSongs.Add(songListNoID [i]);
            //print(songListNoID[i].SongName);
            //print(i);
            i++;
        }
        i = 0;
        while (i < allSongs.Count)
        {
            if (allSongs[i] == null)
            {
                allSongs.Remove(allSongs[i]);
                i -= 1;
            }
            i++;
        }
        noSongs = id - 1;
        **/
    }
    private void ResetSongIDs()
    {
        int i = 0;
        while (i < Albums.Length)
        {
            int j = 0;
            while (j < Albums[i].songs.Length)
            {
                Albums[i].songs[j].ID = "";
                j++;
            }
            i++;
        }
    }
    public SongObject[] GetAllSongs()
    { 
        SongObject[] songs = new SongObject[allSongs.Count];
        int i = 0;
        while (i<allSongs.Count)
        {
            songs[i] = allSongs[i];
            i++;
        }
        
        return songs;
    }
    public bool getShuffle()
    {
        return isShuffle;
    }
    public playerSaveData getSaveData()
    {
        return saveData;
    }
    #endregion
    #region fyp
    private void CreateAlbumsStart()
    {
        AlbumObject[] gottenAlbums = new AlbumObject[DiscoverAlbumsTransform.childCount];
        int i = 0;
        int trys = 0;
        while (i < DiscoverAlbumsTransform.childCount)
        {

            int rand = GetNewAlbum();

            //check if album already added
            int j = 0; bool repeat = false;
            while (j < gottenAlbums.Length)
            {
                if (gottenAlbums[j] == Albums[rand])
                {
                    trys++;
                    //print("Album Duped!"+gottenAlbums[j].AlbumName+" "+trys);
                    repeat = true;
                    break;
                }
                j++;
            }
            if (repeat == false)
            {
                gottenAlbums[i] = Albums[rand];
                i++;
            }
            else if (repeat == false || trys > 20)
            {
                Debug.LogWarning("Repeated album!");
                gottenAlbums[i] = Albums[rand];
                i++;
            }
        }
        i = 0;
        while (i < gottenAlbums.Length)
        {
            DiscoverAlbumsTransform.GetChild(i).gameObject.GetComponent<AlbumCoverScript>().SetAlbum(gottenAlbums[i]);
            //print(gottenAlbums[i].AlbumName);
            i++;
        }
        //DiscoverAlbums = gottenAlbums;
    }
    private void CreateSongsStart()
    {
        SongObject[] gottenSongs = new SongObject[DiscoverSongsTransform.childCount];
        int i = 0;
        int trys = 0;
        while (i < DiscoverSongsTransform.childCount)
        {
            /**
            int randAlbum = GetNewAlbum();//gets a random album
            int rand=GetRandomSongPosition(Albums[randAlbum]);//gets a random song from the album
            //check if song already added
            print(Albums[randAlbum].AlbumName+" "+rand);
            SongObject randSong = Albums[randAlbum].songs[rand];
            **/
            SongObject randSong = GetRandomSong(null);
            int j = 0;bool repeat = false;
            while (j < gottenSongs.Length)
            {
                if(gottenSongs[j] == randSong)
                {
                    trys++;
                    repeat = true;
                    break;
                }
                j++;
            }
            if (repeat == false)
            {
                gottenSongs[i] = randSong;
                i++;
            }
            else if(repeat == false || trys > 20)
            {
                Debug.Log("Repeated Song");
                gottenSongs[i] = randSong;
                i++;
            }
        }
        i = 0;
        while (i < gottenSongs.Length)
        {
            DiscoverSongsTransform.GetChild(i).GetComponent<SongDiscoverScript>().SetSong(gottenSongs[i]);
            i++;
        }
        //DiscoverSongs = gottenSongs;
    } 
    private void CreateFavouriteSongs()
    {
        int i = 0;
        SongObject[]mostPlayed= saveData.getMostPlayed(FavouriteSongsTransform.childCount);
        while (i < FavouriteSongsTransform.childCount)
        {
            FavouriteSongsTransform.GetChild(i).GetComponent<SongDiscoverScript>().SetSong(mostPlayed[i]);
            i++;
        }
    }
    #endregion
}
