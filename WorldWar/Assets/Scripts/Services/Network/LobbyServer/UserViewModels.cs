using System.Collections.Generic;

namespace BattleofRedCliffsVRService.ViewModels
{
    public class UserViewModel
    {
        public string Id { get; set; }  // storeId + ":" + storeTarget  // ex) fdafewrarewarewarewarewaera:0
        public string NickName { get; set; }

        public List<StageViewModel> Stages { get; set; }
    }
}
