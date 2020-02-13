using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Content;
using System;

namespace Client.M4.Droid
{
    [Activity(Label = "CustomUrlSchemeInterceptorActivity", NoHistory = true, LaunchMode = LaunchMode.SingleTop)]
    [IntentFilter(
     new[] { Intent.ActionView },
     Categories = new[] { Intent.CategoryDefault, Intent.CategoryBrowsable },
     DataSchemes = new[] { "com.pluralsight.windows" },
     DataPath = "/callback")]
    public class CustomUrlSchemeInterceptorActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Convert Android.Net.Url to Uri
            var uri = new Uri(Intent.Data.ToString());

            // Load redirectUrl page
            //AuthenticationState.Authenticator.OnPageLoading(uri);

            

           // Finish();
        }
    }
}
