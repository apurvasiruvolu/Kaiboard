using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;

namespace Kaiboard
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        
        public MainPage()
        {
            InitializeComponent();
        }

        //shanthi edit1

        //Working fine for Android 7.1 version and IOS - 13.0.0 Sept25 2019 - Apurva
        public async void camera_onbuttonClick(object sender, EventArgs e)
        {
            await CrossMedia.Current.Initialize();
            if (!CrossMedia.Current.IsCameraAvailable || !CrossMedia.Current.IsTakePhotoSupported)
                {
                    await DisplayAlert("No Camera", ":( No camera avaialble.", "OK");
                    return;
                }

            var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions()
            {
            }) ;

            if (photo != null)
                image.Source = ImageSource.FromStream(() => { return photo.GetStream(); });

        }
    }
}
