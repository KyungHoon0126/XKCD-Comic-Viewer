using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using XKCD_Viewer.XKCD;

namespace XKCD_Viewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private int maxNumber = 0;
        private int currentNumber = 0;

        public MainWindow()
        {
            InitializeComponent();
            ApiHelper.InitializeClient();
            btnNextImage.IsEnabled = false;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await LoadComicImage();
        }

        private async Task LoadComicImage(int imageNumber = 0)
        {
            var comic = await ComicProcessor.LoadComic(imageNumber);

            if(imageNumber == 0)
            {
                maxNumber = comic.Num;
            }

            currentNumber = comic.Num;

            var uriSource = new Uri(comic.Img, UriKind.Absolute);
            imgComic.Source = new BitmapImage(uriSource);
        }

        private async void btnPreviousImage_Click(object sender, RoutedEventArgs e)
        {
            if (currentNumber > 1)
            {
                currentNumber -= 1;
                btnNextImage.IsEnabled = true;
                await LoadComicImage(currentNumber);

                if (currentNumber == 1)
                {
                    btnPreviousImage.IsEnabled = false;
                }
            }
        }

        private async void btnNextImage_Click(object sender, RoutedEventArgs e)
        {
            if (currentNumber < maxNumber)
            {
                currentNumber += 1;
                btnPreviousImage.IsEnabled = true;
                await LoadComicImage(currentNumber);

                if(currentNumber == maxNumber)
                {
                    btnNextImage.IsEnabled = false;
                }
            }
        }
    }
}
