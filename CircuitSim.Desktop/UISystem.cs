using CircuitSim.Core.Annotations;
using CircuitSim.Core.Common;
using Raylib_cs;
using System.Reflection;

namespace CircuitSim.Desktop
{

    public class UISystem
    {
        private int screenWidth;
        private int screenHeight;
        private List<DrawerButton[]> drawers;
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

        public void AddDrawer(DrawerButton[] drawer)
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
            for(int i = 0; i < drawers.Count; i++)
            {
                string drawerTxt = "Drawer " + i;
                int drawerWidth = Raylib.MeasureText(drawerTxt, 20);
                var rect = new Rectangle(i * 100, 0, drawerWidth + 20, 40);
                DrawButton(rect, drawerTxt);
                // Check if is hovering drawer 
                if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), rect))
                {
                    if(Raylib.IsMouseButtonPressed(MouseButton.Left))
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

                if(i == selectedDrawer)
                {
                    for (int j = 0; j < drawers[i].Length; j++)
                    {
                        var width = Raylib.MeasureText(drawers[i][j].Text, 20) + 20;
                        var rect2 = new Rectangle(10 + i * 100, 50 + j * 40, width, 40);
                        DrawButton(rect2, drawers[i][j].Text);
                        if(Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), rect2) &&
                                                       Raylib.IsMouseButtonPressed(MouseButton.Left))
                        {
                            drawers[i][j].Action();
                        }
                    }
                }
            }

        }

        public void DrawButton(Rectangle rect, string text)
        {
            Raylib.DrawRectangleRec(rect, Constants.BackgroundColor);
            Raylib.DrawText(text, (int)rect.X + 10, (int)rect.Y + 5, 20, Color.White);
            if(Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), rect))
            {
                Raylib.DrawRectangleLinesEx(rect, Constants.GridLineWidth, Constants.GridColor);
            }
        }
        public void DrawButton(int x, int y, int width, int height, string text)
        {
            Rectangle button = new Rectangle(x, y, width, height);
            Raylib.DrawRectangleRec(button, Color.DarkGray);
            Raylib.DrawText(text, (int)button.X + 10, (int)button.Y + 5, 20, Color.White);
        }

        public void DrawPropertyInputFields()
        {
            if (selectedWire == null)
                return;

            (int x, int y) = ((int)selectedWire.Center.X, (int)selectedWire.Center.Y);
            int yOffset = 0;
            for (int i = 0; i < properties.Length; i++)
            {
                Raylib.DrawText(properties[i].Name + ": ", x, y + yOffset, 20, Color.White);
                Rectangle inputField = new Rectangle(x + 150, y + yOffset - 5, 200, 30);
                Raylib.DrawRectangleRec(inputField, Color.White);
                if (Raylib.CheckCollisionPointRec(Raylib.GetMousePosition(), inputField) &&
                    Raylib.IsMouseButtonPressed(MouseButton.Left))
                {
                    selectedProp = properties[i];
                    selectedPropValue = "";
                }

                if (selectedProp == properties[i])
                {
                    Raylib.DrawRectangleLinesEx(inputField, 2, Color.Black);
                    char c = (char) Raylib.GetCharPressed();

                    // Only allow printable ASCII characters
                    if (c >= 32 && c <= 126)
                    {
                        selectedPropValue += c;
                    }
                    else if (c == 8 && selectedPropValue.Length > 0)
                    {
                        selectedPropValue = selectedPropValue[..^1];
                    }
                }

                // Save value when enter is pressed
                if(selectedProp != null && Raylib.IsKeyPressed(KeyboardKey.Enter))
                {
                    selectedProp.SetValue(selectedWire, Convert.ChangeType(selectedPropValue, selectedProp.PropertyType));
                    selectedProp = null;
                }

                string txt = selectedProp == properties[i] ? selectedPropValue : properties[i].GetValue(selectedWire)!.ToString();
                Raylib.DrawText(txt, (int)inputField.X + 5, (int)inputField.Y + 5, 20, Color.Black);
                yOffset += 40;
            }
        }

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
}
