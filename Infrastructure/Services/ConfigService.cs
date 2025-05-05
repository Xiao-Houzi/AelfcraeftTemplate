using Godot;
using System;
using System.Collections.Generic;

namespace Aelfcraeft.Infrastructure.Services
{
    public class ConfigService : BaseService
    {
        private Dictionary<string, string> config = new Dictionary<string, string>();

        public void LoadConfig(string configPath)
        {
            if (!System.IO.File.Exists(configPath))
            {
                LogService.LogErr($"Config file not found: {configPath}");
                return;
            }

            var lines = System.IO.File.ReadAllLines(configPath);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;

                var parts = line.Split('=');
                if (parts.Length == 2)
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Trim();
                    config[key] = value;
                    LogService.Log($"Config loaded: {key} = {value}");
                }
            }
        }

        public void LoadOptions()
        {
            LoadOptionsFile("Data/machine_options.txt");
            LoadOptionsFile("Data/user_options.txt");
        }

        private void LoadOptionsFile(string optionsPath)
        {
            if (!System.IO.File.Exists(optionsPath))
            {
                LogService.LogErr($"Options file not found: {optionsPath}");
                return;
            }

            var lines = System.IO.File.ReadAllLines(optionsPath);
            foreach (var line in lines)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#"))
                    continue;

                var parts = line.Split('=');
                if (parts.Length == 2)
                {
                    var key = parts[0].Trim();
                    var value = parts[1].Trim();
                    config[key] = value;
                }
            }
        }

        public string GetConfigValue(string key)
        {
            return config.ContainsKey(key) ? config[key] : null;
        }

        public IEnumerable<string> GetAllConfigKeys()
        {
            return config.Keys;
        }
    }
}