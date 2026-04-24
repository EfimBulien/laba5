using Microsoft.Win32;
using Newtonsoft.Json;
using System.IO;

namespace laba5
{
    internal class JsonConverter
    {
        public static T DeserializeObject<T>()
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                string json = File.ReadAllText(dialog.FileName);
                
                T obj = JsonConvert.DeserializeObject<T>(json);
                
                return obj;
            }
            else
            {
                return default;
            }
        }
    }
}
