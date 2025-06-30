using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFPointApp.Configuration;
using WPFPointApp.Models;
using System;

namespace WPFPointApp.ViewModels.Services
{
    internal class JsonFileService : IFileService
    {
        public event Action<string> OnError;

        public JsonFileService()
        {
            m_DirectoryPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        }

        public async Task SavePointAsync(Point point)
        {
            var filePath = $"{m_DirectoryPath}\\{m_LastIndex:000000}{AppConfiguration.FileExtension}";
            using (var writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                await writer.WriteAsync(JsonConvert.SerializeObject(point));
            }

            m_LastIndex++;
        }

        public async Task<T> LoadPointAsync<T>(string filePath)
        {
            using (var reader = new StreamReader(filePath, Encoding.UTF8))
            {
                string json = await reader.ReadToEndAsync();
                return JsonConvert.DeserializeObject<T>(json);
            }
        }

        public async Task<List<Point>> LoadPointsAsync()
        {
            if (!Directory.Exists(m_DirectoryPath))
            {
                return default;
            }

            var points = new List<Point>();
            var files = Directory.GetFiles(m_DirectoryPath, $"*{AppConfiguration.FileExtension}").OrderBy(f => f).ToList();
            if(files != null && files.Count > 0)
            {
                int.TryParse(Path.GetFileNameWithoutExtension(files.Last()), out m_LastIndex);
                m_LastIndex++;
            }
            
            foreach (var file in files)
            {
                var data = await LoadPointAsync<Point>(file).ConfigureAwait(false);
                if(data != null)
                {
                    points.Add(data);
                }
                else
                {
                    OnError?.Invoke($"Invalid file {Path.GetFileName(file)}");
                }
            }

            return points;
        }

        private int m_LastIndex;

        private readonly string m_DirectoryPath;
    }
}
