namespace Roomba.Systems.Input
{
    public class ActionSignal
    {
        public InputCollector.Actions actions;
        public bool down;

        public ActionSignal(InputCollector.Actions actions, bool down)
        {
            this.actions = actions;
            this.down = down;
        }

        public override string ToString()
        {
            return $"(action: {actions}, down: {down})";
        }
    }
}