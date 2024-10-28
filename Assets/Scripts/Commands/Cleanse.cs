using Command.Main;
using Command.Actions;
using Commands;

public class Cleanse : UnitCommands
{
    private bool willHitTarget;

    public Cleanse(CommandData commandData)
    {
        this.commandData = commandData;
        willHitTarget = WillHitTarget();
    }

    public override bool WillHitTarget() => true;

    public override void Execute() => GameService.Instance.ActionService.GetActionByType(CommandType.Attack).PerformAction(actorUnit, targetUnit, willHitTarget);
}