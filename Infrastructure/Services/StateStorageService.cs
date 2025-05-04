using Godot;
using System.Collections.Generic;
using System; // Added to resolve the CS0103 error for 'Convert'.

namespace Survival.Infrastructure.Services
{
    public class StateStorageService : BaseService
    {
        public StateStorageService()
        {
            
        }

        public void SetState(string key, object value)
        {
            state[key] = value;
        }

        public T GetValueType<T>(string key) where T : struct
        {
            if (!state.TryGetValue(key, out var value)) return default;

            try
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch (Exception ex)
            {
                LogService.LogErr($"Failed to convert state value for key '{key}' to value type {typeof(T)}. Returning default value. Exception: {ex.Message}");
                return default;
            }
        }

        public T GetReferenceType<T>(string key) where T : class
        {
            if (!state.TryGetValue(key, out var value)) return default;

            try
            {
                return value as T;
            }
            catch (InvalidCastException)
            {
                LogService.LogErr($"Failed to cast state value for key '{key}' to reference type {typeof(T)}. Returning default value.");
                return default;
            }
        }


        public void SaveState(string filePath)
        {
            var lines = new List<string>();
            foreach (var kvp in state)
            {
                lines.Add($"{kvp.Key}={kvp.Value}");
            }
            System.IO.File.WriteAllLines(filePath, lines);
        }

        public void LoadState(string filePath)
        {
            if (!System.IO.File.Exists(filePath))
            {
                LogService.LogErr($"State file not found: {filePath}");
                return;
            }

            var lines = System.IO.File.ReadAllLines(filePath);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;

                var parts = line.Split('=');
                if (parts.Length == 2)
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Trim();
                    SetState(key, value);
                }
            }
        }

        public void LoadState()
        {
            var stateFilePath = _configLoader.GetConfigValue("StateFile");
            LoadState(stateFilePath);
        }

        public void SaveState()
        {
            var stateFilePath = _configLoader.GetConfigValue("StateFile");
            SaveState(stateFilePath);
        }



        private readonly LogService _logservice = Services.GetService<LogService>();
        private readonly ConfigService _configLoader = Services.GetService<ConfigService>();
      
        private Dictionary<string, object> state = new Dictionary<string, object>();
    }
}