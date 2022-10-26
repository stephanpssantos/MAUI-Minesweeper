using System.Text.Json;

namespace MauiTest1
{
    public static class LocalConfig
    {
        private static readonly string _fileName = Path.Combine(FileSystem.AppDataDirectory, "minesweeper.json");

        static LocalConfig()
        {
            if (File.Exists(_fileName) == false)
            {
                OverwriteConfig();
                return;
            }
            using (StreamReader sr = new(_fileName))
            {
                string json = sr.ReadToEnd();
                ConfigJson = JsonSerializer.Deserialize<ConfigJson>(json);
                return;
            }
        }

        public static ConfigJson ConfigJson { get; set; } = new();

        public static void OverwriteConfig()
        {
            string jsonString = JsonSerializer.Serialize(ConfigJson);

            using (StreamWriter sw = new(_fileName))
            {
                sw.WriteLine(jsonString);
            }
        }
    }

    public class ConfigJson
    {
        public string LastGameDifficulty { get; set; } = "Beginner";
        public int CustomBoardWidth { get; set; } = 0;
        public int CustomBoardHeight { get; set; } = 0;
        public int CustomBoardMines { get; set; } = 0;
        public int BeginnerTime { get; set; } = 999;
        public string BegginerName { get; set; } = "Anonymous";
        public int IntermediateTime { get; set; } = 999;
        public string IntermediateName { get; set; } = "Anonymous";
        public int ExpertTime { get; set; } = 999;
        public string ExpertName { get; set; } = "Anonymous";
    }
}
