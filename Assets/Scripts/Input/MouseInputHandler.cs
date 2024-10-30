using Command.Main;
using Command.Player;
using UnityEngine;

namespace Command.Input
{
    public class MouseInputHandler
    {
        private InputService inputService;
        private TargetType targetTypeToSelect;

        public MouseInputHandler(InputService inputService) => this.inputService = inputService;

        public void HandleTargetSelection(TargetType targetTypeToSelect)
        {
            this.targetTypeToSelect = targetTypeToSelect;

            if (UnityEngine.Input.GetMouseButtonDown(0))
            {
                TrySelectingTargetUnit();
            }
        }

        public void TrySelectingTargetUnit()
        {
            Vector3 mouseWorldPosition = GetMouseWorldPosition();
            if (IsTargetSelected(mouseWorldPosition, out UnitView selectedUnit))
                inputService.OnTargetSelected(selectedUnit.Controller);
        }

        private Vector3 GetMouseWorldPosition()
        {
            var mousePosition = Camera.main.ScreenToWorldPoint(UnityEngine.Input.mousePosition);
            return new Vector3(mousePosition.x, mousePosition.y, 0);
        }

        private bool IsTargetSelected(Vector3 mousePosition, out UnitView selectedUnit)
        {
            Collider2D collider = Physics2D.OverlapCircle(mousePosition, 0.1f);

            if (IsUnit(collider))
            {
                selectedUnit = collider.GetComponent<UnitView>();
                if (ValidateUnit(selectedUnit))
                    return true;
            }

            selectedUnit = null;
            return false;
        }

        private bool IsUnit(Collider2D collider) => collider != null && collider.GetComponent<UnitView>() != null;

        private bool ValidateUnit(UnitView selectedUnit)
        {
            switch (targetTypeToSelect)
            {
                case TargetType.Friendly:
                    return selectedUnit.Controller.Owner.PlayerID == GameService.Instance.PlayerService.ActivePlayerID && selectedUnit.Controller.IsAlive();
                case TargetType.Enemy:
                    return selectedUnit.Controller.Owner.PlayerID != GameService.Instance.PlayerService.ActivePlayerID && selectedUnit.Controller.IsAlive();
                case TargetType.Self:
                    return selectedUnit.Controller.UnitID == GameService.Instance.PlayerService.ActiveUnitID && selectedUnit.Controller.IsAlive();
                default:
                    throw new System.Exception($"Target Type to be selected might be null. Cannot Validate Selected Unit. Current Target Type to be selected is: {targetTypeToSelect}");
            }
        }
    }
}