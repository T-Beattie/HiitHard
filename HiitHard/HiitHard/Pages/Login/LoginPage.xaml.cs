using HiitHard.Managers;
using HiitHard.Navigation;
using HiitHard.Objects;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HiitHard.Pages.Login
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        private SignInManager sign_in_manager = SignInManager.Instance();
        private DBUser db_user = new DBUser();
        private ProfileManager profile_manager = ProfileManager.Instance();

        public LoginPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        protected override async void OnAppearing()
        {

            if (profile_manager.user.email == null)
            {
                AutoSigninAsync();
            }
        }

        async Task AutoSigninAsync()
        {
            profile_manager.GetLocalProfile();
            User user = profile_manager.user;

            if (user.email != null)
            {
                //await Navigation.PushPopupAsync(loading_overlay);

                bool successful = await sign_in_manager.AutoSignIn(user);

                if (successful)
                {
                    //loading_overlay.CloseLoading();
                    await Navigation.PushModalAsync(new Navigation.Navigation());

                }
            }
        }


        private async void sign_in_btn_Clicked(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(username_entry.Text) || !string.IsNullOrWhiteSpace(username_entry.Text) || !string.IsNullOrEmpty(password_entry.Text) || !string.IsNullOrWhiteSpace(password_entry.Text))
                if (password_entry.Text.Length > 7)
                    sign_in_manager.SignInNative(username_entry.Text, password_entry.Text);
                else
                {
                    await App.Current.MainPage.DisplayAlert("Password Length", "Password must be 8 characters or higher", "Ok");
                }
            else
            {
                await App.Current.MainPage.DisplayAlert("Incorrect Credentials", "No Email or Password Provided", "Ok");
            }
        }

        private void spotify_sign_in_btn_Clicked(object sender, EventArgs e)
        {
            sign_in_manager.SignInSpotify();
        }

        private void main_sign_in_btn_Clicked(object sender, EventArgs e)
        {
            MainFrame.IsVisible = false;
            LoginFrame.IsVisible = true;
            RegisterFrame.IsVisible = false;
        }

        private void register_btn_Clicked(object sender, EventArgs e)
        {
            MainFrame.IsVisible = false;
            LoginFrame.IsVisible = false;
            RegisterFrame.IsVisible = true;
        }

        private void back_btn_Clicked(object sender, EventArgs e)
        {
            MainFrame.IsVisible = true;
            LoginFrame.IsVisible = false;
            RegisterFrame.IsVisible = false;

            username_entry.Text = null;
            password_entry.Text = null;

            register_username_entry.Text = null;
            register_password_entry.Text = null;
            register_reenter_password_entry.Text = null;
        }

        public bool ValidateEmail(string email)
        {
            Regex EmailRegex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");

            if (string.IsNullOrWhiteSpace(email))
                return false;

            return EmailRegex.IsMatch(email);
        }

        public bool DoIHaveInternet()
        {
            return CrossConnectivity.Current.IsConnected;
        }

        private async void register_register_btn_Clicked(object sender, EventArgs e)
        {
            string email = register_username_entry.Text;
            string password = register_password_entry.Text;
            string confirm_password = register_reenter_password_entry.Text;

            int password_length = 8;

            if (DoIHaveInternet())
            {
                if (email.Length == 0 || password.Length == 0 || confirm_password.Length == 0)
                {
                    await DisplayAlert("Error!", "You have not filled out all of the fields", "Ok");
                }
                else if (confirm_password != password)
                {
                    await DisplayAlert("Error!", "Passwords do not match", "Ok");
                }
                else if (password.Length < password_length)
                {
                    await DisplayAlert("Error!", "Password must be at least 8 characters", "Ok");
                }
                else if (!ValidateEmail(email))
                {
                    await DisplayAlert("Error!", "Invalid email format", "Ok");
                }
                else if (await db_user.DoesUsernameExistAsync(email))
                {
                    await DisplayAlert("Error!", "Account already exists", "Ok");
                }
                else
                {
                    await CreateUser(email, password);
                }
            }
            else
            {
                await DisplayAlert("Error!", "No internet connection could be found", "Ok");
            }
        }

        public async Task CreateUser(string email, string password)
        {
            User new_user = new User()
            {
                email = email,
                password = password,
                spotify_id = "",
                display_name = "Native User"
            };

            bool success = await db_user.CreateUser(new_user);
            if (success)
            {
                profile_manager.SetLocalProfile(new_user);
                await Navigation.PushModalAsync(new Navigation.Navigation());
            }
        }
    }
}