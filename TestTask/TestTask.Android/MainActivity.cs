using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace TestTask.Droid
{
    // Version 3 (OldStyle)
    [Activity(Label = "TestTask", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, ScreenOrientation = ScreenOrientation.Portrait)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());

        }
    }

    // Version 2 (Grey)
    //[Activity(Label = "TestTask", Icon = "@drawable/icon", Theme = "@style/Theme.AppCompat.NoActionBar", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    //public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    //{
    //    protected override void OnCreate(Bundle bundle)
    //    {
    //        TabLayoutResource = Resource.Layout.Tabbar;
    //        ToolbarResource = Resource.Layout.Toolbar;

    //        base.OnCreate(bundle);

    //        global::Xamarin.Forms.Forms.Init(this, bundle);
    //        LoadApplication(new App());

    //    }
    //}

    // Version 1 (White)
    //[Activity(Label = "TestTask", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    //public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    //{
    //    protected override void OnCreate(Bundle bundle)
    //    {
    //        TabLayoutResource = Resource.Layout.Tabbar;
    //        ToolbarResource = Resource.Layout.Toolbar;

    //        base.OnCreate(bundle);

    //        global::Xamarin.Forms.Forms.Init(this, bundle);
    //        LoadApplication(new App());
    //    }
    //}
}

