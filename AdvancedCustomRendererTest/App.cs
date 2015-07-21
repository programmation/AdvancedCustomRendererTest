using System;

using Xamarin.Forms;

namespace AdvancedCustomRendererTest
{
    public class App : Application
    {
        public App()
        {
            var listPageModel = new AdvancedListViewPageModel();
            var listPage = new AdvancedListViewPage();
            listPage.Title = "Advanced ListView";

            listPage.BindingContext = listPageModel;

            var homePage = new NavigationPage(listPage);
            homePage.BarBackgroundColor = Color.Silver;
            homePage.BarTextColor = Color.White;

            // The root page of your application
            MainPage = homePage;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

