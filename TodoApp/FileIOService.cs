using Newtonsoft.Json;
using System.ComponentModel;
using System.IO;

namespace TodoApp
{
    class FileIOService
    {
        private readonly string PATH;

        public FileIOService(string path)
        {
            PATH = path;
        }

        public BindingList<TaskModel> LoadData()
        {
            var fileExist = File.Exists(PATH);
            if (!fileExist)
            {
                File.Create(PATH).Dispose();
                return new BindingList<TaskModel>();
            }
            using (var reader = File.OpenText(PATH))
            {
                string json = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<BindingList<TaskModel>>(json);
            }
        }

        public void SaveData(object taskList)
        {
            using (StreamWriter writer = new StreamWriter(PATH))
            {
                var text = JsonConvert.SerializeObject(taskList);
                writer.Write(text);
            }
        }
    }
}
