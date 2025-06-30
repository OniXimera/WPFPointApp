using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WPFPointApp.Models;

namespace WPFPointApp.ViewModels.Services
{
    internal interface IFileService
    {
        event Action<string> OnError;
        Task SavePointAsync(Point point);
        Task<List<Point>> LoadPointsAsync();
    }
}
