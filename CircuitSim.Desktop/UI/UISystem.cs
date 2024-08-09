using CircuitSim.Core.Annotations;
using CircuitSim.Core.Common;
using Raylib_cs;
using System.Reflection;
using static Raylib_cs.Raylib;

namespace CircuitSim.Desktop.UI;


public class UISystem
{
    private int screenWidth;
    private int screenHeight;
    private List<DrawerContainer> drawers;
    private PropertyInfo? selectedProp = null;
    private string selectedPropValue = "";

    int selectedDrawer = -1;

    private Wire? selectedWire = null;
    private PropertyInfo[] properties = [];
    public UISystem(int width, int height)
    {
        screenWidth = width;
        screenHeight = height;
        drawers = new();
    }

    public void AddDrawer(DrawerContainer drawer)
    {
        drawers.Add(drawer);
    }
    public void SelectWire(Wire? wire)
    {
        selectedWire = wire;
        properties = wire?.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanWrite)
            .Where(p => p.GetCustomAttribute<PropertyEditableAttribute>() != null).ToArray()
            ?? [];
    }

    public void DrawUI()
    {
        // Draw drawers
        int xOffset = 0;
        for (int i = 0; i < drawers.Count; i++)
        {
            string drawerTxt = drawers[i].Title;
            int drawerWidth = MeasureText(drawerTxt, 20);
            var rect = new Rectangle(xOffset, 0, drawerWidth + 20, 40);
            xOffset += drawerWidth + 20;
            DrawButton(rect, drawerTxt);
            // Check if is hovering drawer 
            if (CheckCollisionPointRec(GetMousePosition(), rect))
            {
                if (IsMouseButtonPressed(MouseButton.Left))
                {
                    if (selectedDrawer == i)
                    {
                        selectedDrawer = -1;
                    }
                    else
                    {
                        selectedDrawer = i;
                    }
                }
            }

            if (i == selectedDrawer)
            {
                int j = 0;
                foreach (var button in drawers[i].Buttons)
                {
                    var width = MeasureText(button.Text, 20) + 20;
                    var rect2 = new Rectangle(10 + i * 100, 50 + j * 40, width, 40);
                    DrawButton(rect2, button.Text);
                    if (CheckCollisionPointRec(GetMousePosition(), rect2) &&
                        IsMouseButtonPressed(MouseButton.Left))
                    {
                        button.Action();
                        selectedDrawer = -1;
                    }
                    j++;
                }
            }
        }

    }

    public void DrawButton(Rectangle rect, string text)
    {
        DrawRectangleRec(rect, Constants.BackgroundColor);
        DrawText(text, (int)rect.X + 10, (int)rect.Y + 5, 20, Color.White);
        if (CheckCollisionPointRec(GetMousePosition(), rect))
        {
            DrawRectangleLinesEx(rect, Constants.GridLineWidth, Constants.GridColor);
        }
    }
    public void DrawButton(int x, int y, int width, int height, string text)
    {
        Rectangle button = new Rectangle(x, y, width, height);
        DrawRectangleRec(button, Color.DarkGray);
        DrawText(text, (int)button.X + 10, (int)button.Y + 5, 20, Color.White);
    }

    public void DrawPropertyInputFields()
    {
        if (selectedWire == null)
            return;

        (int x, int y) = ((int)selectedWire.Center.X, (int)selectedWire.Center.Y);
        int yOffset = 0;
        for (int i = 0; i < properties.Length; i++)
        {
            DrawText(properties[i].Name + ": ", x, y + yOffset, 20, Color.White);
            Rectangle inputField = new Rectangle(x + 150, y + yOffset - 5, 200, 30);
            DrawRectangleRec(inputField, Color.White);
            if (CheckCollisionPointRec(GetMousePosition(), inputField) &&
                IsMouseButtonPressed(MouseButton.Left))
            {
                selectedProp = properties[i];
                selectedPropValue = "";
            }

            if (selectedProp == properties[i])
            {
                DrawRectangleLinesEx(inputField, 2, Color.Black);
                char c = (char)GetCharPressed();

                // Only allow printable ASCII characters
                if (c >= 32 && c <= 126)
                {
                    selectedPropValue += c;
                }
                else if (IsKeyPressed(KeyboardKey.Backspace) && selectedPropValue.Length > 0)
                {
                    selectedPropValue = selectedPropValue[..^1];
                }
            }

            // Save value when enter is pressed
            if (selectedProp != null && IsKeyPressed(KeyboardKey.Enter))
            {
                selectedProp.SetValue(selectedWire, Convert.ChangeType(Utils.ParseValue(selectedPropValue), selectedProp.PropertyType));
                selectedProp = null;
            }

            string txt = selectedProp == properties[i] ? selectedPropValue : properties[i].GetValue(selectedWire)!.ToString();
            DrawText(txt, (int)inputField.X + 5, (int)inputField.Y + 5, 20, Color.Black);
            yOffset += 40;
        }
    }
}
