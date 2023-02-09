using HiitHard.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace HiitHard.Managers
{
    class NativePlaylistManager
    {
        private static NativePlaylistManager _instance = new NativePlaylistManager();
        private string playlists_file = "playlists.json";

        public List<Playlist> playlistStash;

        //Static method which allows the instance creation  
        static internal NativePlaylistManager Instance()
        {
            //All you need to do it is just return the  
            //already initialized which is thread safe  
            return _instance;
        }

        public NativePlaylistManager()
        {
            playlistStash = new List<Playlist>();
        }

        public void LoadPlaylists()
        {
            string contents = DependencyService.Get<INativeFileService>().ReadJson(playlists_file);

            if (contents != "No File present")
            {
                playlistStash = JsonConvert.DeserializeObject<List<Playlist>>(contents);
            }
        }

        public void SavePlaylists()
        {
            string stash_string = JsonConvert.SerializeObject(playlistStash, Formatting.Indented);
            DependencyService.Get<INativeFileService>().WriteJson(stash_string, playlists_file);
        }

        public void AddPlaylist(Playlist newPlaylist)
        {
            playlistStash.Add(newPlaylist);
            string my_string = JsonConvert.SerializeObject(playlistStash, Formatting.Indented);
            DependencyService.Get<INativeFileService>().WriteJson(my_string, playlists_file);
        }

        public void RemovePlaylist(Playlist playlistToDelete)
        {
            playlistStash.Remove(playlistToDelete);
            string my_string = JsonConvert.SerializeObject(playlistStash, Formatting.Indented);
            DependencyService.Get<INativeFileService>().WriteJson(my_string, playlists_file);
        }

        public void ClearPlaylists()
        {
            List<Playlist> empty_stash = new List<Playlist>();
            string my_string = JsonConvert.SerializeObject(empty_stash, Formatting.Indented);
            DependencyService.Get<INativeFileService>().WriteJson(my_string, playlists_file);
            playlistStash = empty_stash;
        }
    }


    public class Playlist
    {
        public string name { get; set; }
        public string description { get; set; }
        public List<Track> tracks { get; set; }

        public Playlist(string Name, string Description, List<Track> Tracks)
        {
            name = Name;
            description = Description;
            tracks = Tracks;
        }

        public void AddTrack(Track newTrack)
        {
            if (!tracks.Contains(newTrack))
            {
                tracks.Add(newTrack);
            }
        }

        public void RemoveTrack(Track trackToRemove)
        {
            tracks.Remove(trackToRemove);
        }

        public string GetTotalDuration()
        {
            int totalTimeSeconds = 0;
            
            for (int i = 0; i < tracks.Count; i++)
            {
                totalTimeSeconds += tracks[i].duration;
            }

            TimeSpan t = TimeSpan.FromSeconds(totalTimeSeconds);
            string totalTime;
            if (t.Hours == 0)
            {
                totalTime = string.Format("{0:D2}m {1:D2}s",
                            t.Minutes,
                            t.Seconds);
            }
            else
            {
                totalTime = string.Format("{0:D2}h {1:D2}m {2:D2}s",
                            t.Hours,
                            t.Minutes,
                            t.Seconds);
            }


            return totalTime;
        }
    }

    public class Track
    {
        public string name { get; set; }
        public string artist { get; set; }
        public int duration { get; set; }
        public string path { get; set; }

        public Track(string Name, string Artist, int Duration, string Path)
        {
            name = Name;
            artist = Artist;
            duration = Duration;
            path = Path;
        }
    }
}
