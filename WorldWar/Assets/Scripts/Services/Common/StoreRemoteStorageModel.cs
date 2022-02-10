using System.Collections.Generic;

public class StoreRemoteStorageModel : StoreModel
{
    public List<StageDB> Stages { get; set; }
    public List<StageDB> ChallengeStages { get; set; }
    public List<StageDB> ExtremeStages { get; set; }
    public List<StageDB> ArcadeStages { get; set; }

    //option
    public int FirstGame { get; set; }
    public int SelectGraphic { get; set; }
    public int SelectController { get; set; }
    public int SelectLanguage { get; set; }
    public float SelectSoundFX { get; set; }
    public float SelectSoundBGM { get; set; }
}
