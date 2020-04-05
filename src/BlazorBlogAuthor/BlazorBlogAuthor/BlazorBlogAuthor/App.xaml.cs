using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using BlazorBlogAuthor.Services;
using BlazorBlogAuthor.Views;

namespace BlazorBlogAuthor
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            DependencyService.Register<IBlogStore, BlogStore>();
            MainPage = new MainPage();
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
