using Command.Main;
using System.Collections.Generic;

namespace Commands
{
    public class CommandInvoker
    {
        public CommandInvoker() => SubscribeToEvents();

        private void SubscribeToEvents() => GameService.Instance.EventService.OnReplayButtonClicked.AddListener(SetReplayStack);

        private Stack<ICommand> commandRegistry = new Stack<ICommand>();

        public void ProcessCommand(ICommand commandToProcess)
        {
            ExecuteCommand(commandToProcess);
            RegisterCommand(commandToProcess);
        }

        public void ExecuteCommand(ICommand commandToExecute) => commandToExecute.Execute();

        public void RegisterCommand(ICommand commandToRegister) => commandRegistry.Push(commandToRegister);

        public void Undo()
        {
            if (!RegistryEmpty() && CommandBelongsToActivePlayer())
                commandRegistry.Pop().Undo();
        }

        public void SetReplayStack()
        {
            GameService.Instance.ReplayService.SetCommandStack(commandRegistry);
            commandRegistry.Clear();
        }

        private bool RegistryEmpty() => commandRegistry.Count == 0;

        private bool CommandBelongsToActivePlayer() => (commandRegistry.Peek() as UnitCommand).commandData.ActorPlayerID == GameService.Instance.PlayerService.ActivePlayerID;
    }
}