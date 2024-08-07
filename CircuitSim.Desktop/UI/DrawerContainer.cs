namespace CircuitSim.Desktop.UI;

public class DrawerContainer
{
    public string Title { get; set; }
    public IEnumerable<DrawerButton> Buttons { get; set; }

    public DrawerContainer(string title, IEnumerable<DrawerButton> buttons)
    {
        Title = title;
        Buttons = buttons;
    }
}
