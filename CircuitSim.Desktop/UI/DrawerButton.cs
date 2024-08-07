namespace CircuitSim.Desktop.UI
{
    public class DrawerButton
    {
        public string Text { get; set; }
        public Action Action { get; set; }

        public DrawerButton(string text, Action action)
        {
            Text = text;
            Action = action;
        }
    }
}
