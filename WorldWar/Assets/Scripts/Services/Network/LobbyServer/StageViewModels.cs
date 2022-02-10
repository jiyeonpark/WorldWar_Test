namespace BattleofRedCliffsVRService.ViewModels
{
    public class StageViewModel
    {
        public string Id { get; set; }  // stageIndex + ":" + stageMode  // ex) 1:1
        public int State { get; set; }
        public int Score { get; set; }
    }
}
