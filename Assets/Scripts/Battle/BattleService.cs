using System.Collections.Generic;
using Command.Main;

namespace Command.Battle
{
    public class BattleService
    {
        private List<BattleScriptableObject> battleScriptableObjects;
        private int currentBattleId;

        public BattleService(List<BattleScriptableObject> battleScriptableObjects)
        {
            this.battleScriptableObjects = battleScriptableObjects;
            SubscribeToEvents();
        }

        private void SubscribeToEvents() => GameService.Instance.EventService.OnBattleSelected.AddListener(LoadBattle);

        private void LoadBattle(int battleId)
        {
            currentBattleId = battleId;
            var battleDataToLoad = GetBattleDataByID(battleId);
            GameService.Instance.UIService.SetBattleBackgroundImage(battleDataToLoad.BattleBackgroundImage);
            GameService.Instance.UIService.ShowGameplayView();
            GameService.Instance.SoundService.PlaySoundEffects(Sound.SoundType.BATTLE_START);
            GameService.Instance.PlayerService.Init(battleDataToLoad.Player1Data, battleDataToLoad.Player2Data);
        }

        private BattleScriptableObject GetBattleDataByID(int battleId) => battleScriptableObjects.Find(battleSO => battleSO.BattleID == battleId);
    }
}