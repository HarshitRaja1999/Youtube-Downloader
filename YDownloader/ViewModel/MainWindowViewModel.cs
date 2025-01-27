using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using YoutubeExtractor;

namespace YDownloader.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            SelectDestination = new RelayCommand<object>(SelecetDestinationHandler);
            StartDownload = new RelayCommand<object>(DownloadHandler);

        }
        private int _progress;

        public string YoutubeURL { get; set; }
        public string ShowPercentage { get; set; }
        public string VideoQuality { get; set; }
        public ICommand SelectDestination { get; set; }
        public ICommand StartDownload { get; set; }
        public int Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                NotifyPropertyChanged(nameof(Progress));
            }
        }
        
        private void SelecetDestinationHandler(object obj)
        {

        }
        private void DownloadHandler(object obj)
        {
            IEnumerable<VideoInfo> videos = DownloadUrlResolver.GetDownloadUrls(YoutubeURL);
            VideoInfo video = videos.First(p => p.VideoType == VideoType.Mp4 && p.Resolution == 720);
            if (video.RequiresDecryption)
            {
                DownloadUrlResolver.DecryptDownloadUrl(video);
            }
            VideoDownloader downloader = new VideoDownloader(video, Path.Combine(@"D:\.NET\" + video.Title + video.VideoExtension));
            downloader.DownloadProgressChanged += DownloadProgress;
            downloader.Execute();
        }
        private void DownloadProgress(object sender, ProgressEventArgs e)
        {
            Progress = Convert.ToInt32( e.ProgressPercentage );
        }
    }
}
