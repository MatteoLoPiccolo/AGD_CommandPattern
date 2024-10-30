using Command.Main;
using Command.Actions;

namespace Commands
{
    public class MeditateCommand : UnitCommand
    {
        private bool willHitTarget;

        public MeditateCommand(CommandData commandData)
        {
            this.commandData = commandData;
            willHitTarget = WillHitTarget();
        }

        public override bool WillHitTarget() => true;

        public override void Execute() => GameService.Instance.ActionService.GetActionByType(CommandType.Attack).PerformAction(actorUnit, targetUnit, willHitTarget);

        public override void Undo()
        {
            //if (willHitTarget)
            //{
            //    targetUnit.TakeDamage(actorUnit.CurrentPower);
            //    actorUnit.Owner.ResetCurrentActiveUnit();
            //}
        }
    }
}