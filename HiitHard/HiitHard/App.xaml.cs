﻿using HiitHard.Pages.Login;
using MediaManager;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HiitHard
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            //MainPage = new NavigationPage(new Navigation.NavigationDetail());
            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
