/// <summary>
/// The index of a scene that is specified in the Build Settings (Ctrl/Command + Shift + B).
/// </summary>
public enum SceneIndex
{
    // The integer value of a scene should correspond to its index in the Build Settings (Ctrl/Command + Shift + B).
    // For example, the index of Main Menu is 0, so its integer value should also be 0.
    // If in the future, there is a change to the order or the indices of the scenes, this enum also needs to be updated accordingly.
    
    // Main menu related
    MainMenu = 0,
    
    // Demo related
    DemoStage1 = 1,
    DemoStage2 = 2,
    DemoStage3 = 3,
    
    // Level one hub
    LevelOneHubIncomplete = 4,
    LevelOneHubSemiComplete = 5,
    LevelOneHubComplete = 6,
    
    // Bedroom time
    BedroomTime1 = 7,
    BedroomTime2 = 8,
    BedroomTime3 = 9,
    
    // Bedroom sequence
    BedroomSequence1 = 10,
}
