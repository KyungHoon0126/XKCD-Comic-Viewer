using System;
using System.Diagnostics;
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
            // btnNextImage.IsEnabled = false;
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            tbSearchComic.TextDecorations = TextDecorations.Underline;
            await LoadComicImage();
        }

        private async Task LoadComicImage(int imageNumber = 0)
        {
            var comic = await ComicProcessor.LoadComic(imageNumber);
            tbComicPageNum.Text = Convert.ToString(comic.Num);

            if(imageNumber == 0)
            {
                maxNumber = comic.Num;
                tbMaxComicPageNum.Text = Convert.ToString(maxNumber);
            }

            currentNumber = comic.Num;

            var uriSource = new Uri(comic.Img, UriKind.Absolute);
            imgComic.Source = new BitmapImage(uriSource);
        }

        #region Previous & Next
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
            else
            {
                MessageBox.Show("이미 첫화 페이지 입니다.");
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
            else
            {
                MessageBox.Show("다음화가 존재하지 않습니다.");
            }
        }
        #endregion

        #region Start & End
        private async void btnStartImage_Click(object sender, RoutedEventArgs e)
        {
            if(currentNumber == 0)
            {
                MessageBox.Show("이미 첫화 페이지 입니다.");
            }

            var comic = await ComicProcessor.LoadComic(614);
            tbComicPageNum.Text = Convert.ToString(0);
            currentNumber = 0;

            var uriSource = new Uri(comic.Img, UriKind.Absolute);
            imgComic.Source = new BitmapImage(uriSource);
        }

        private async void btnEndImage_Click(object sender, RoutedEventArgs e)
        {
            if(currentNumber == maxNumber)
            {
                MessageBox.Show("다음화가 존재하지 않습니다.");
            }

            var comic = await ComicProcessor.LoadComic(maxNumber);
            tbComicPageNum.Text = Convert.ToString(maxNumber);
            currentNumber = maxNumber;

            var uriSource = new Uri(comic.Img, UriKind.Absolute);
            imgComic.Source = new BitmapImage(uriSource);
        }
        #endregion

        private async void btnSpecificComicImageSearch_Click(object sender, RoutedEventArgs e)
        {
            if(tbSearchComic.Text.Length > 0)
            {
                try
                {
                    int specificPageNum = Convert.ToInt32(tbSearchComic.Text);
                    if (tbSearchComic.Text != null && tbSearchComic.Text.Length > 0 && specificPageNum > 0 && specificPageNum < maxNumber)
                    {
                        var comic = await ComicProcessor.LoadComic(specificPageNum);
                        tbComicPageNum.Text = tbSearchComic.Text;
                        currentNumber = specificPageNum;

                        var uriSource = new Uri(comic.Img, UriKind.Absolute);
                        imgComic.Source = new BitmapImage(uriSource);
                    }
                }
                catch (Exception error)
                {
                    Debug.WriteLine(error.Message);
                }
                finally
                {
                    tbSearchComic.Text = string.Empty;
                }
            }
            else
            {
                MessageBox.Show("보고싶은 화의 번호를 입력해 주세요!");
            }
        }
    }
}
