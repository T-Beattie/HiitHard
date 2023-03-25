using HiitHard.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HiitHard.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlaylistsPage : ContentPage
    {
        public PlaylistsPage()
        {
            InitializeComponent();

            List<Track> trackList = new List<Track>() { new Track("Foo", "Bar", 231, "test/test.bar")};
            Playlist myPlaylist = new Playlist("My Playlist", "test description for running around a track. Big pumper music!", trackList);
            PlaylistUI newPlaylistUI = new PlaylistUI(myPlaylist);

            playlistStack.Children.Add(newPlaylistUI);
        }

        private void AddPlaylist_Clicked(object sender, EventArgs e)
        {

        }
    }

    public class PlaylistUI : Frame
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int NumberOfTracks { get; set; }
        public int TotalDuration { get; set; }

        private Playlist myPlaylist;

        public PlaylistUI(Playlist playlist)
        {
            myPlaylist = playlist;
            Name = myPlaylist.name;
            Description = myPlaylist.description;
            NumberOfTracks = myPlaylist.tracks.Count;

            BackgroundColor = Color.Red;

            CreateUI();
        }

        void CreateUI()
        {
            var nameLabel = new Label()
            {
                Text = Name,
                FontSize = 12,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                VerticalOptions = LayoutOptions.Center,
                TextColor = Color.White,
                FontFamily = "Montserrat"
            };

            var seperationBox = new BoxView()
            {
                HeightRequest = 2,
                WidthRequest = nameLabel.Width,
                Color = Color.FromHex("#B69213")
            };

           

            var layout = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    nameLabel,seperationBox,
                }
            };

            Content = layout;
        }
    }
}