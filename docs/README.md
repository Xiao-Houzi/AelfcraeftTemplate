# Template Project Documentation

Welcome to the Template Project! This documentation will guide you on how to consume, extend, and contribute to this project.

## Table of Contents
- [Template Project Documentation](#template-project-documentation)
  - [Table of Contents](#table-of-contents)
  - [Introduction](#introduction)
  - [Getting Started](#getting-started)
    - [Setting Up Your Project](#setting-up-your-project)
    - [Overriding the Default Scene Structure](#overriding-the-default-scene-structure)
  - [Using Services](#using-services)
    - [Overview](#overview)
    - [Accessing Services](#accessing-services)
    - [Commonly Used Services](#commonly-used-services)
    - [Adding Custom Services](#adding-custom-services)
    - [Best Practices](#best-practices)
  - [Extending the Project](#extending-the-project)
    - [Extensible Services](#extensible-services)
    - [General Guidelines for Extending Services](#general-guidelines-for-extending-services)
  - [Testing](#testing)

## Introduction
This project is designed to be consumed as a Git submodule and provides extensible services and components for game development.

## Getting Started

### Setting Up Your Project
1. **Add the Template Project as a Submodule**:
   - Run the following command to add the template project as a Git submodule:
     ```bash
     git submodule add <repository-url> path/to/submodule
     ```
   - Replace `<repository-url>` with the URL of the template project repository and `path/to/submodule` with the desired path in your project.

2. **Reference the Template Project**:
   - Open your `.sln` file in your IDE (e.g., Visual Studio).
   - Add the `Aelfcraeft.csproj` file from the submodule as an existing project to your solution.
   - Set up a project reference to `Aelfcraeft` in your main project.

3. **Build the Template Project**:
   - Ensure the template project builds successfully as part of your solution.

### Overriding the Default Scene Structure
The template project provides a default scene structure under the `Assets/GameStates` folder. You can override this structure in your project as follows:

1. **Copy the Default Scenes**:
   - Copy the scenes you want to override from `Assets/GameStates` to your own project folder.

2. **Modify the Scenes**:
   - Open the copied scenes in the Godot editor and make the necessary changes.
   - For example, you can customize `MainMenu.tscn` to add new buttons or change the layout.

3. **Update the Scene References**:
   - The `SceneManager` now supports configurable scene paths through the `config.txt` file.
   - To configure scenes, add entries in the following format to `config.txt`:
     ```
     SceneName=Path/To/Scene.tscn
     ```
     Example:
     ```
     MainMenu=res://Assets/GameStates/MainMenu.tscn
     Game=res://Assets/GameStates/Game.tscn
     ```
   - Use the `SceneManager.PreloadScene(sceneName)` method to preload scenes based on the configuration.
   - Example:
     ```csharp
     var sceneManager = new SceneManager();
     sceneManager.PreloadScene("MainMenu");
     ```
   - Ensure the `config.txt` file is correctly formatted and accessible at `res://Data/config.txt`.

4. **Test Your Changes**:
   - Run your project to ensure the overridden scenes are loaded and function as expected.

## Using Services

### Overview
Services in this project are designed to provide modular and reusable functionality. They can be accessed through the `BaseService` class, which acts as a service locator. This section explains how to use the provided services in your project.

### Accessing Services
To access a service, use the `BaseService.Services.GetService<T>()` method, where `T` is the type of the service you want to access.

Example:
```csharp
var logService = BaseService.Services.GetService<LogService>();
logService.Log("This is a log message.");
```

### Commonly Used Services

1. **LogService**
   - **Purpose**: Logs messages for debugging and monitoring.
   - **Usage**:
     ```csharp
     var logService = BaseService.Services.GetService<LogService>();
     logService.Log("Application started.");
     ```

2. **MessengerService**
   - **Purpose**: Handles message passing and event-driven communication.
   - **Usage**:
     ```csharp
     var messengerService = BaseService.Services.GetService<MessengerService>();
     messengerService.SendMessage(MessageType.StartGame);
     ```

3. **StateStorageService**
   - **Purpose**: Manages application state storage and retrieval.
   - **Usage**:
     ```csharp
     var stateStorageService = BaseService.Services.GetService<StateStorageService>();
     stateStorageService.SetState("PlayerHealth", 100);
     var health = stateStorageService.GetState<int>("PlayerHealth");
     ```

4. **ConfigService**
   - **Purpose**: Loads and manages configuration settings.
   - **Usage**:
     ```csharp
     var configService = BaseService.Services.GetService<ConfigService>();
     var option = configService.GetOption("GraphicsQuality");
     ```

5. **InputService**
   - **Purpose**: Manages input bindings and actions.
   - **Usage**:
     ```csharp
     var inputService = BaseService.Services.GetService<InputService>();
     if (inputService.IsActionPressed("jump"))
     {
         Console.WriteLine("Jump action triggered.");
     }
     ```

### Adding Custom Services
You can add your own services to the `BaseService` locator.

Example:
```csharp
public class CustomService
{
    public void DoSomething()
    {
        Console.WriteLine("Custom service action.");
    }
}

// Register the custom service
BaseService.Services.AddService(new CustomService());

// Access the custom service
var customService = BaseService.Services.GetService<CustomService>();
customService.DoSomething();
```

### Best Practices
- Use services to encapsulate reusable functionality.
- Avoid directly instantiating services; use the service locator to ensure consistency.
- Document custom services to make them easier to use and extend.

## Extending the Project

### Extensible Services
The following services are designed to be extensible, allowing you to customize their behavior:

1. **MessengerService**
   - **Purpose**: Handles message passing and event-driven communication.
   - **How to Extend**:
     - Use the `RegisterCustomMessageHandler` method to add custom handlers for specific message types.
     - Example:
       ```csharp
       var messengerService = new MessengerService();
       messengerService.RegisterCustomMessageHandler(MessageType.CustomEvent, () => {
           Console.WriteLine("Custom event triggered!");
       });
       ```

3. **StateStorageService**
   - **Purpose**: Manages application state storage and retrieval.
   - **How to Extend**:
     - Inherit from `StateStorageService` and override methods like `SaveState` or `LoadState` to customize state management.
     - Example:
       ```csharp
       public class CustomStateStorageService : StateStorageService
       {
           public override void SaveState()
           {
               // Custom save logic
               Console.WriteLine("Saving custom state...");
           }
       }
       ```

4. **ApplicationManager**
   - **Purpose**: Manages the lifecycle and initialization of the application.
   - **How to Extend**:
     - Use dependency injection to provide custom implementations of services.
     - Override methods like `_Ready` to add custom initialization logic.
     - Example:
       ```csharp
       public class CustomApplicationManager : ApplicationManager
       {
           public CustomApplicationManager(IServiceProvider serviceProvider) : base(serviceProvider) { }

           public override void _Ready()
           {
               base._Ready();
               // Custom initialization logic
               Console.WriteLine("Custom ApplicationManager ready!");
           }
       }
       ```

### General Guidelines for Extending Services
- Always use inheritance or composition to extend functionality.
- Avoid modifying the base services directly to ensure compatibility with future updates.
- Use the provided public APIs and virtual methods to add custom behavior.

## Testing
Run the provided PowerShell scripts in the `Tests` folder to validate functionality.

