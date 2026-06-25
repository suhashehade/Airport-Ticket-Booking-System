using System.Text.Json;

namespace AirportSystem.Data
{
    public class FileContext
    {

        private readonly string _storageDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage");

        private readonly string _filePath;

        public FileContext(string fileName)
        {

            if (!Directory.Exists(_storageDirectory))
            {
                Directory.CreateDirectory(_storageDirectory);
            }

            _filePath = Path.Combine(_storageDirectory, fileName);
        }


        public async Task Write<T>(List<T> flights)
        {

            var options = new JsonSerializerOptions { WriteIndented = true };

            string jsonString = JsonSerializer.Serialize(flights, options);

            await File.WriteAllTextAsync(_filePath, jsonString);
        }


        public async Task<List<T>> Read<T>()
        {

            if (!File.Exists(_filePath))
            {
                return new List<T>();
            }

            string jsonString = await File.ReadAllTextAsync(_filePath);

            List<T>? rows = JsonSerializer.Deserialize<List<T>>(jsonString);

            return rows ?? new List<T>();
        }


    }
}