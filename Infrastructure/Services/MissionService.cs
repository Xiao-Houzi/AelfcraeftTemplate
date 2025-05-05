using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Aelfcraeft.Infrastructure.Services
{
    public class MissionService : BaseService
    {
        private List<MissionStage> _stages;
        private int _currentStageIndex;

        public MissionService()
        {
           
            _stages = new List<MissionStage>();
            _currentStageIndex = 0;
        }

        public void LoadMission(string missionFilePath)
        {
            if (!File.Exists(missionFilePath))
            {
                GD.PrintErr($"Mission file not found: {missionFilePath}");
                return;
            }

            var json = File.ReadAllText(missionFilePath);
            _stages = JsonConvert.DeserializeObject<List<MissionStage>>(json);
            _currentStageIndex = StateStorage.GetValueType<int>("CurrentMissionStage");
        }

        public void CompleteTask(string taskId)
        {
            if (_currentStageIndex >= _stages.Count)
            {
                GD.Print("All mission stages completed.");
                return;
            }

            var currentStage = _stages[_currentStageIndex];
            var task = currentStage.Tasks.Find(t => t.Id == taskId);
            if (task != null)
            {
                task.IsCompleted = true;
                GD.Print($"Task {taskId} completed.");

                if (currentStage.Tasks.TrueForAll(t => t.IsCompleted))
                {
                    AdvanceToNextStage();
                }
            }
            else
            {
                GD.PrintErr($"Task {taskId} not found in current stage.");
            }
        }

        private void AdvanceToNextStage()
        {
            _currentStageIndex++;
            StateStorage.SetState("CurrentMissionStage", _currentStageIndex);

            if (_currentStageIndex < _stages.Count)
            {
                var nextStage = _stages[_currentStageIndex];
                var args = new MissionStageChangedArgs
                {
                    NewStage = nextStage,
                    StageIndex = _currentStageIndex
                };
                Messenger.SendMessage(MessengerService.MessageType.MissionStageChanged, args);
                GD.Print($"Advanced to stage {_currentStageIndex}: {nextStage.Description}");
            }
            else
            {
                GD.Print("Mission completed.");
            }
        }

        public MissionStage GetCurrentStage()
        {
            return _currentStageIndex < _stages.Count ? _stages[_currentStageIndex] : null;
        }
    }

    public class MissionStage
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public List<MissionTask> Tasks { get; set; }
    }

    public class MissionTask
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
    }

    public class MissionStageChangedArgs : IArguments
    {
        public MissionStage NewStage { get; set; }
        public int StageIndex { get; set; }
    }
}