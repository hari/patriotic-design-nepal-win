using System;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.Graphics.Display;
using Windows.Graphics.Imaging;
using System.Runtime.InteropServices.WindowsRuntime;
using System.IO;

namespace Patriotic_Design_Nepal
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string ImageUri = "ms-appx:///Assets/buddha_one.jpg", FontColor = "#fefefe";
        private string FileName = "output.jpg";
        private int FACTOR = 3, WIDTH = 851, HEIGHT = 315;
        public MainPage()
        {
            InitializeComponent();
            DataTransferManager dataTransferManager = DataTransferManager.GetForCurrentView();
            dataTransferManager.DataRequested += new TypedEventHandler<DataTransferManager, DataRequestedEventArgs>(ShareImageHandler);
        }

        private async void ShareImageHandler(DataTransferManager sender, DataRequestedEventArgs args)
        {
            DataRequest request = args.Request;
            request.Data.Properties.Title = "Your patriotic image";
            request.Data.Properties.Description = "Share your patriotic image via email or social services.";
            DataRequestDeferral deferral = request.GetDeferral();
            try
            {
                StorageFile thumbnailFile = await StorageFile.GetFileFromPathAsync( ApplicationData.Current.LocalFolder.Path + "\\" + FileName);
                request.Data.Properties.Thumbnail = RandomAccessStreamReference.CreateFromFile(thumbnailFile);
                request.Data.SetBitmap(RandomAccessStreamReference.CreateFromFile(thumbnailFile));
            }
            finally
            {
                deferral.Complete();
            }
        }

        public void CreateImage()
        {
            if (FontTop == null || FontLeft == null || TxtFontSize == null) return;
            ImagePreview.Width = Overlay.Width = PreviewCanvas.Width = WIDTH / FACTOR;
            ImagePreview.Height = Overlay.Height = PreviewCanvas.Height = HEIGHT / FACTOR;
            MainText.Text = UserText.Text;
            MainText.FontSize = TxtFontSize.Value / FACTOR;
            MainText.Margin = new Windows.UI.Xaml.Thickness(FontLeft.Value / FACTOR, FontTop.Value / FACTOR, 0, 0);
            byte R = Convert.ToByte(FontColor.Substring(1, 2), 16);
            byte G = Convert.ToByte(FontColor.Substring(3, 2), 16);
            byte B = Convert.ToByte(FontColor.Substring(5, 2), 16);
            MainText.Foreground = new SolidColorBrush(Color.FromArgb(255, R, G, B));
            if (ImageUri.StartsWith("#"))
            {
                R = Convert.ToByte(ImageUri.Substring(1, 2), 16);
                G = Convert.ToByte(ImageUri.Substring(3, 2), 16);
                B = Convert.ToByte(ImageUri.Substring(5, 2), 16);
                Overlay.Fill = new SolidColorBrush(Color.FromArgb(255, R, G, B));
                ImagePreview.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            }
            else
            {
                Overlay.Fill = new SolidColorBrush(Color.FromArgb(68, 0, 0, 0));
                ImagePreview.Visibility = Windows.UI.Xaml.Visibility.Visible;
                ImagePreview.Source = new BitmapImage(new Uri(ImageUri));
            }
        }

        private int CurrentIndex = 0;
        private void BtnRandomImgClicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            CurrentIndex++;
            var images = new String[]
            {
                "Assets/buddha_one.jpg",
                "Assets/pokhara_one.jpg",
                "Assets/texture_one.jpg",
                "Assets/buddha_two.jpg",
                "Assets/everest_one.jpg",
                "Assets/pokhara_two.jpg",
                "Assets/buddha_three.jpg",
                "Assets/buddha_four.jpg",
                "Assets/texture_two.jpg",
                "Assets/pokhara_three.jpg",
                "Assets/everest_two.jpg",
                "Assets/pokhara_four.jpg",
                "Assets/buddha_five.jpg",
                "Assets/buddha_six.jpg",
                "Assets/texture_three.jpg",
                "Assets/pokhara_five.jpg",
                "Assets/buddha_seven.jpg",
                "Assets/everest_three.jpg",
                "Assets/buddha_eight.jpg",
                "Assets/texture_four.jpg",
                "Assets/buddha_nine.jpg"
            };
            if (CurrentIndex >= images.Length)
            {
                CurrentIndex = 0;
            }
            ImageUri = "ms-appx:///" + images[CurrentIndex];
            CreateImage();
        }

        private void FontSizeChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            CreateImage();
        }

        private void FontTopChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            CreateImage();
        }

        private void FontLeftChanged(object sender, Windows.UI.Xaml.Controls.Primitives.RangeBaseValueChangedEventArgs e)
        {
            CreateImage();
        }
        private int ColorIndex = 0, ForeColorIndex = -1;

        private void BtnRandomClrClicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var colors = new string[] {
                "ececec", "5eb990", "a75d3a",
                "bd4036", "d0aa08", "d80675",
                "c67369", "61818c", "ac91b4",
                "fd5c63", "378e4c", "be47ad",
                "987405", "212121", "7e9d1c",
                "fd5c63", "378e4c", "be47ad",
                "987405", "212121"
            };
            ColorIndex++;
            if (ColorIndex >= colors.Length)
            {
                ColorIndex = 0;
            }
            ImageUri = "#" + colors[ColorIndex];
            CreateImage();
        }

        private void BtnRandomFor_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var colors = new string[] {
                "ececec", "5eb990", "a75d3a",
                "bd4036", "d0aa08", "d80675",
                "c67369", "61818c", "ac91b4",
                "fd5c63", "378e4c", "be47ad",
                "987405", "212121", "7e9d1c",
                "fd5c63", "378e4c", "be47ad",
                "987405", "212121"
            };
            if (ForeColorIndex == -1)
            {
                ForeColorIndex = colors.Length;
            }
            ForeColorIndex--;
            if (ForeColorIndex < 0)
            {
                ForeColorIndex = 0;
            }
            FontColor = "#" + colors[ForeColorIndex];
            CreateImage();
        }

        private async void ShareImage(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            if (!File.Exists(ApplicationData.Current.LocalFolder.Path + "\\" + FileName))
            {
                Windows.UI.Popups.MessageDialog msg = new Windows.UI.Popups.MessageDialog("No image found to share.\nCreate your image first.", "Not found");
                await msg.ShowAsync();
                return;
            }
            SaveImage(sender, e);
            DataTransferManager.ShowShareUI();
        }

        private async void SaveImage(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            FACTOR = 1;
            CreateImage();
            var displayInformation = DisplayInformation.GetForCurrentView();
            var imageSize = new Size(851, 315);
            var renderTargetBitmap = new RenderTargetBitmap();
            await renderTargetBitmap.RenderAsync(PreviewCanvas, Convert.ToInt32(imageSize.Width), Convert.ToInt32(imageSize.Height));
            var pixelBuffer = await renderTargetBitmap.GetPixelsAsync();
            var file = await ApplicationData.Current.LocalFolder.CreateFileAsync(FileName, CreationCollisionOption.ReplaceExisting);
            using (var fileStream = await file.OpenAsync(FileAccessMode.ReadWrite))
            {
                var encoder = await BitmapEncoder.CreateAsync(BitmapEncoder.JpegEncoderId, fileStream);
                encoder.SetPixelData(
                        BitmapPixelFormat.Bgra8,
                        BitmapAlphaMode.Ignore,
                        (uint)renderTargetBitmap.PixelWidth,
                        (uint)renderTargetBitmap.PixelHeight,
                        displayInformation.LogicalDpi,
                        displayInformation.LogicalDpi,
                        pixelBuffer.ToArray());

                await encoder.FlushAsync();
            }
            FACTOR = 3;
            CreateImage();
        }
    }
}
