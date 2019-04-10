using System;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Accord.Video.FFMPEG;
using AForge.Video;
using AForge.Video.DirectShow;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;

namespace VideoRecorder
{
    internal class MainWindowViewModel : ObservableObject, IDisposable
    {
        #region Private fields

        private FilterInfo _currentDevice;

        private BitmapImage _image;
        private string _ipCameraUrl;
        
        private bool _isIpCameraSource;
        private bool _isWebcamSource;

        private IVideoSource _videoSource;
        private VideoFileWriter _writer;
        private bool _recording;
        private DateTime? _firstFrameTime;

        #endregion

        #region Constructor

        public MainWindowViewModel()
        {
            VideoDevices = new ObservableCollection<FilterInfo>();
            GetVideoDevices();
            StartSourceCommand = new RelayCommand(StartCamera);
            StopSourceCommand = new RelayCommand(StopCamera);
            StartRecordingCommand = new RelayCommand(StartRecording);
            StopRecordingCommand = new RelayCommand(StopRecording);
            IpCameraUrl = VideoRecorder.Constants.CameraAddressConstants.CAMERA_1_IP;
        }

        #endregion

        #region Properties

        public ObservableCollection<FilterInfo> VideoDevices { get; set; }

        public BitmapImage Image
        {
            get { return _image; }
            set { Set(ref _image, value); }
        }

        public bool IsWebcamSource
        {
            get { return _isWebcamSource; }
            set { Set(ref _isWebcamSource, value); }
        }

        public bool IsIpCameraSource
        {
            get { return _isIpCameraSource; }
            set { Set(ref _isIpCameraSource, value); }
        }

        public string IpCameraUrl
        {
            get { return _ipCameraUrl; }
            set { Set(ref _ipCameraUrl, value); }
        }

        public FilterInfo CurrentDevice
        {
            get { return _currentDevice; }
            set { Set(ref _currentDevice, value); }
        }

        public ICommand StartRecordingCommand { get; private set; }

        public ICommand StopRecordingCommand { get; private set; }

        public ICommand StartSourceCommand { get; private set; }

        public ICommand StopSourceCommand { get; private set; }

        #endregion

        private void GetVideoDevices()
        {
            var devices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            foreach (FilterInfo device in devices)
            {
                VideoDevices.Add(device);
            }
            if (VideoDevices.Any())
            {
                CurrentDevice = VideoDevices[0];
            }
            else
            {
                MessageBox.Show("No webcam found");
            }
        }

        private void StartCamera()
        {
            if (IsWebcamSource)
            {
                if (CurrentDevice != null)
                {
                    _videoSource = new VideoCaptureDevice(CurrentDevice.MonikerString);
                    _videoSource.NewFrame += video_NewFrame;
                    _videoSource.Start();
                }
                else
                {
                    MessageBox.Show("Current device can't be null");
                }
            }
            else if (IsIpCameraSource)
            {
                _videoSource = new MJPEGStream(IpCameraUrl);
                _videoSource.NewFrame += video_NewFrame;
                _videoSource.Start();
            }
        }

        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            try
            {
                if (_recording)
                {
                    using (var bitmap = (Bitmap) eventArgs.Frame.Clone())
                    {
                        if (_firstFrameTime != null)
                        {
                            _writer.WriteVideoFrame(bitmap, DateTime.Now - _firstFrameTime.Value);
                        }
                        else
                        {
                            _writer.WriteVideoFrame(bitmap);
                            _firstFrameTime = DateTime.Now;
                        }
                    }
                }
                using (var bitmap = (Bitmap) eventArgs.Frame.Clone())
                {
                    var bi = bitmap.ToBitmapImage();
                    bi.Freeze();
                    Dispatcher.CurrentDispatcher.Invoke(() => Image = bi);
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Error on _videoSource_NewFrame:\n" + e.Message, "Error", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                StopCamera();
               /* if(e is InvalidOperationException)
                {
                    // Just printing a message for now.  We may need to restart the camera immediately after
                }*/
            }
        }

        private void StopCamera()
        {
            if (_videoSource != null && _videoSource.IsRunning)
            {
                _videoSource.SignalToStop();
                _videoSource.NewFrame -= video_NewFrame;
            }
            Image = null;
        }

        private void StopRecording()
        {
            _recording = false;
            _writer.Close();
            _writer.Dispose();
        }

        private void StartRecording()
        {
            DateTime saveTime = new DateTime();
            saveTime = DateTime.Now;
            string fileName = "DATE_" + saveTime.Month + "_" + saveTime.Day + "_" + "TIME_" + saveTime.Hour + "_" +saveTime.Minute + "_" + saveTime.Second;

            var dialog = new SaveFileDialog();
            dialog.FileName = fileName; 
            dialog.DefaultExt = ".mp4"; // Could also be .avi since both wrappers work for H.265
            dialog.AddExtension = true;
            var dialogresult = dialog.ShowDialog();
            if (dialogresult != true)
            {
                return;
            }
            _firstFrameTime = null;
            _writer = new VideoFileWriter();
            _writer.Open(dialog.FileName, (int)Math.Round(Image.Width, 0), (int)Math.Round(Image.Height, 0));
            _recording = true;
        }

        public void Dispose()
        {
            if (_videoSource != null && _videoSource.IsRunning)
            {
                _videoSource.SignalToStop();
            }
            _writer?.Dispose();
        }
    }
}
