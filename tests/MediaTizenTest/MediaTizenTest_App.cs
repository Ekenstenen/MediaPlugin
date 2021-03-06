﻿using Tizen.Applications;
using ElmSharp;
using System.Diagnostics;

namespace MediaTizenTest
{
    class App : CoreUIApplication
    {
		private Image image;

        protected override void OnCreate()
        {
            base.OnCreate();
            Initialize();
        }

        void Initialize()
        {
            Window window = new Window("MediaTizenTest")
            {
                AvailableRotations = DisplayRotation.Degree_0 | DisplayRotation.Degree_180 | DisplayRotation.Degree_270 | DisplayRotation.Degree_90
            };
            window.BackButtonPressed += (s, e) =>
            {
                Exit();
            };
            window.Show();			

            var box = new Box(window)
            {
                AlignmentX = -1,
                AlignmentY = -1,
                WeightX = 1,
                WeightY = 1,
            };
			box.SetPadding(5, 5);
			box.Show();
			
            var bg = new Background(window)
            {
                Color = Color.White
            };
            bg.SetContent(box);

            var conformant = new Conformant(window);
            conformant.Show();
            conformant.SetContent(bg);			

			var takePhoto = new Button(window)
			{
				Text = "Take Photo",
				AlignmentX = -1,
				AlignmentY = -1,
				WeightX = 1,
			};
			takePhoto.Clicked += TakePhoto_ClickedAsync;
			takePhoto.Show();			
			box.PackEnd(takePhoto);

			var pickPhoto = new Button(window)
			{
				Text = "Pick Photo",
				AlignmentX = -1,
				AlignmentY = -1,
				WeightX = 1,
			};
			pickPhoto.Clicked += PickPhoto_ClickedAsync;
			pickPhoto.Show();
			box.PackEnd(pickPhoto);

			var takeVideo = new Button(window)
			{
				Text = "Take Video",
				AlignmentX = -1,
				AlignmentY = -1,
				WeightX = 1,
			};
			takeVideo.Clicked += TakeVideo_ClickedAsync;
			takeVideo.Show();
			box.PackEnd(takeVideo);

			var pickVideo = new Button(window)
			{
				Text = "Pick Video",
				AlignmentX = -1,
				AlignmentY = -1,
				WeightX = 1,
			};
			pickVideo.Clicked += PickVideo_ClickedAsync;
			pickVideo.Show();
			box.PackEnd(pickVideo);

			image = new Image(window)
			{
				AlignmentX = -1,
				AlignmentY = -1,
				WeightX = 1,
				WeightY = 1,				
			};
			image.Show();
			box.PackEnd(image);
		}


		private async void TakePhoto_ClickedAsync(object sender, System.EventArgs e)
		{
			var media = new Plugin.Media.MediaImplementation();
			if (!media.IsCameraAvailable || !media.IsTakePhotoSupported)
				throw new System.NotSupportedException();
			var file = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
			{
				DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear
			});
			var path = file.Path;
			image.Load(path);
		}
		private async void PickPhoto_ClickedAsync(object sender, System.EventArgs e)
		{
			var media = new Plugin.Media.MediaImplementation();
			if (!media.IsCameraAvailable || !media.IsPickPhotoSupported)
				throw new System.NotSupportedException();
			var file = await  Plugin.Media.CrossMedia.Current.PickPhotoAsync(new Plugin.Media.Abstractions.PickMediaOptions{ });
			if (file == null) return;
			var path = file.Path;
			image.Load(path);
		}
		private async void TakeVideo_ClickedAsync(object sender, System.EventArgs e)
		{
			var media = new Plugin.Media.MediaImplementation();
			if (!media.IsCameraAvailable || !media.IsTakePhotoSupported)
				throw new System.NotSupportedException();
			var file = await Plugin.Media.CrossMedia.Current.TakeVideoAsync(new Plugin.Media.Abstractions.StoreVideoOptions
			{
				DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear
			});
			var path = file.Path;
		}
		private async void PickVideo_ClickedAsync(object sender, System.EventArgs e)
		{
			var media = new Plugin.Media.MediaImplementation();
			var file = await Plugin.Media.CrossMedia.Current.PickVideoAsync();
			if (file == null)
				return;
			var path = file.Path;
		}
		static void Main(string[] args)
        {
            Elementary.Initialize();
            Elementary.ThemeOverlay();
            App app = new App();
            app.Run(args);
        }
    }
}
