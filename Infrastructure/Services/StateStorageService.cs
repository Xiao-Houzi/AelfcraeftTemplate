using Godot;
using System.Collections.Generic;
using System; // Added to resolve the CS0103 error for 'Convert'.

namespace Aelfcraeft.Infrastructure.Services
{
    public class StateStorageService : BaseService
    {
        public StateStorageService()
        {
            // Constructor no longer calls LoadStateFromConfig directly
        }

        public void Initialize()
        {
            // Load state after ConfigService is initialized
            LoadStateFromConfig();
        }

        private void LoadStateFromConfig()
        {
            if (ConfigService == null)
            {
                LogService.LogErr("ConfigService is not initialized. Cannot load state from config.");
                return;
            }

            var stateFilePath = ConfigService.GetConfigValue("StateFile");
            if (!string.IsNullOrEmpty(stateFilePath))
            {
                LoadState(stateFilePath);
            }
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
            var stateFilePath = ConfigService.GetConfigValue("StateFile");
            LoadState(stateFilePath);
        }

        public void SaveState()
        {
            var stateFilePath = ConfigService.GetConfigValue("StateFile");
            SaveState(stateFilePath);
        }

        private Dictionary<string, object> state = new Dictionary<string, object>();
    }
}