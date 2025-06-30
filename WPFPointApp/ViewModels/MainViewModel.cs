using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Globalization;
using Point = WPFPointApp.Models.Point;
using WPFPointApp.ViewModels.Services;
using WPFPointApp.Configuration;
using Newtonsoft.Json;
using System.IO;
using System;

namespace WPFPointApp.ViewModels
{
    internal class MainViewModel : ObservableObject, IDisposable
    {
        public ICommand AddPointCommand { get; }

        public string XCoordinate
        {
            get => m_XCoordinate;
            set => SetProperty(ref m_XCoordinate, value);
        }

        public string YCoordinate
        {
            get => m_YCoordinate;
            set => SetProperty(ref m_YCoordinate, value);
        }

        public string StatusMessage
        {
            get => m_StatusMessage;
            set => SetProperty(ref m_StatusMessage, value);
        }

        public ObservableCollection<Point> PointsListBox { get; } = new ObservableCollection<Point>();

        public MainViewModel(IFileService fileService, IPointDrawService drawService)
        {
            AddPointCommand = new RelayCommand(AddPoint, CanAddPoint);
            m_FileService = fileService;
            m_FileService.OnError += HandleFileServiceError;
            m_PointDrawService = drawService;
            LoadPointsAsync().ConfigureAwait(false);
        }

        public void Dispose()
        {
            m_FileService.OnError -= HandleFileServiceError;
        }

        private async Task LoadPointsAsync()
        {
            try
            {
                var points = await m_FileService.LoadPointsAsync();
                foreach (var point in points)
                {
                    PointsListBox.Add(point);
                    m_PointDrawService.DrawPoint(point);
                }
            }
            catch (DirectoryNotFoundException)
            {
                HandleFileServiceError("Data saves folder not found");
            }
            catch (JsonException)
            {
                HandleFileServiceError("Error in format of saved data");
            }
            catch (Exception ex)
            {
                HandleFileServiceError($"Error: {ex.Message}");
            }
        }

        private bool CanAddPoint(object parameter)
        {
            return XCoordinate != null 
                && YCoordinate != null 
                && ValidateCoordinateText(XCoordinate, out double x) 
                && ValidateCoordinateText(YCoordinate, out double y)
                && ValidateOverflow(x, y);
        }

        private bool ValidateCoordinateText(string text, out double value)
        {
            return double.TryParse(text.Trim().Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out value);
        }

        private bool ValidateOverflow(double x, double y)
        {
            return x >= AppConfiguration.PlaneMin 
                && x <= AppConfiguration.PlaneMax
                && y >= AppConfiguration.PlaneMin
                && y <= AppConfiguration.PlaneMax;
        }

        private void AddPoint(object parameter)
        {
            if(ValidateCoordinateText(XCoordinate, out double x) && ValidateCoordinateText(YCoordinate, out double y))
            {
                if (!ValidateOverflow(x, y))
                {
                    HandleFileServiceError($"Number out of range {AppConfiguration.PlaneMin} and {AppConfiguration.PlaneMax}");
                    return;
                }

                var point = new Point(x, y);
                PointsListBox.Add(point);
                m_PointDrawService.DrawPoint(point);
                m_FileService.SavePointAsync(point);
            }
        }

        private void HandleFileServiceError(string errorMessage)
        {
            StatusMessage = errorMessage;
        }

        private string m_XCoordinate;
        private string m_YCoordinate;
        private string m_StatusMessage;

        private readonly IFileService m_FileService;
        private readonly IPointDrawService m_PointDrawService;
    }
}
