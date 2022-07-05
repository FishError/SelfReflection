using System;

[Serializable]
public class SaveData
{
    public SceneIndex LastVisitedLevel;
    public SaveData()
    {
        LastVisitedLevel = SceneIndex.DemoStage1;
    }

    public SaveData(SceneIndex level)
    {
        LastVisitedLevel = level;
    }
}