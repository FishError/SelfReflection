using System;

[Serializable]
public class SaveData
{
    public SaveData()
    {
        LastVisitedLevel = LevelStage.Level1;
    }
    
    public LevelStage LastVisitedLevel;
}