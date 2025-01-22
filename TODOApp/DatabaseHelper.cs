using System.Collections.Generic;
using System.IO;
using System.Xml;
using Newtonsoft.Json;

public static class JsonHelper
{
    private const string FilePath = "tasks.json";

    public static void SaveTasks(List<Task> tasks)
    {
        var json = JsonConvert.SerializeObject(tasks, Newtonsoft.Json.Formatting.Indented);
        File.WriteAllText(FilePath, json);
    }

    public static List<Task> LoadTasks()
    {
        if (!File.Exists(FilePath))
        {
            return new List<Task>();
        }

        var json = File.ReadAllText(FilePath);
        return JsonConvert.DeserializeObject<List<Task>>(json);
    }
}
