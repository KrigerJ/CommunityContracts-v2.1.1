using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.Menus;
using static ModEntry;

namespace CommunityContracts.Core
{
    public class CCToolbar : IClickableMenu
    {
        private List<ClickableTextureComponent> buttons = new();
        private static string MainMenuTooltip => T("MainMenuTooltip");
        private ClickableComponent mainMenuButton;
        private static string DropLocationTooltip => T("DropLocationTooltip");
        private ClickableComponent setDropLocationButton;
        private static string RectangleTooltip => T("RectangleTooltip");
        private ClickableComponent RectangleButton;
        public CCToolbar()
        {
            int toolbarWidth = 800;
            int toolbarHeight = 86;

            this.xPositionOnScreen = 30;
            this.yPositionOnScreen = 20;
            this.width = toolbarWidth;
            this.height = toolbarHeight;

            int x = this.xPositionOnScreen + 12;
            int y = this.yPositionOnScreen + 12;

            int menuButtonX = x + 1;
            int menuButtonY = y + 1;

            mainMenuButton = new ClickableComponent(
                new Rectangle(menuButtonX, menuButtonY, 140, 60),
                T("MainMenuButton")
            );

            int dropButtonX = x + 300;
            int dropButtonY = y + 1;

            setDropLocationButton = new ClickableComponent(
                new Rectangle(dropButtonX, dropButtonY, 140, 60),
                T("SetDropLocationButton")
            );

            int RectangleButtonX = x + 540;
            int RectangleButtonY = y + 1;

            RectangleButton = new ClickableComponent(
                new Rectangle(RectangleButtonX, RectangleButtonY, 140, 60),
                T("RectangleButton")
            );
        }

        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            if (mainMenuButton.containsPoint(x, y))
            {
                Game1.activeClickableMenu = new NPCDropdownMenu();

                return;
            }

            if (setDropLocationButton.containsPoint(x, y))
            {
                Vector2 tile = new Vector2((int)(Game1.player.Position.X / Game1.tileSize), (int)(Game1.player.Position.Y / Game1.tileSize));
                string locationName = Game1.player.currentLocation.Name;

                Game1.exitActiveMenu();

                Config.DropLocationName = locationName;
                Config.PresetLocations[locationName] = tile;
                Instance.Helper.WriteConfig(Config);

                Game1.playSound("coin");
                Game1.showGlobalMessage(T("DropLocationSet", new { location = locationName, x = (int)tile.X, y = (int)tile.Y }));

                HighlightedDropTile = new Vector2(
                    (int)(Game1.player.Position.X / Game1.tileSize),
                    (int)(Game1.player.Position.Y / Game1.tileSize)
                );

                return;
            }

            if (RectangleButton.containsPoint(x, y))
            {
                Game1.activeClickableMenu = new SetPlacement();
                Instance.SetPlacementMenuOpen = true;
                return;
            }
        }
        public override void draw(SpriteBatch b)
        {
            drawTextureBox(b, Game1.menuTexture,
                new Rectangle(0, 256, 60, 60),
                this.xPositionOnScreen,
                this.yPositionOnScreen,
                this.width,
                this.height,
                Color.White);

            string mainMenutext = T("MainMenuButton");
            Vector2 MaintextSize = Game1.smallFont.MeasureString(mainMenutext);

            int MainMenuadjustedWidth = (int)MaintextSize.X + 40;

            mainMenuButton.bounds = new Rectangle(
                mainMenuButton.bounds.X,
                mainMenuButton.bounds.Y,
                MainMenuadjustedWidth,
                mainMenuButton.bounds.Height
            );

            Color MainButtonColor = mainMenuButton.containsPoint(Game1.getMouseX(), Game1.getMouseY())
                                ? Color.LimeGreen
                                : Color.White;

            drawTextureBox(
                b,
                Game1.menuTexture,
                new Rectangle(0, 256, 60, 60),
                mainMenuButton.bounds.X,
                mainMenuButton.bounds.Y,
                mainMenuButton.bounds.Width,
                mainMenuButton.bounds.Height,
                MainButtonColor,
                1f,
                false
            );

            float MaintextX = mainMenuButton.bounds.X + (mainMenuButton.bounds.Width / 2f) - (MaintextSize.X / 2f);
            float MaintextY = mainMenuButton.bounds.Y + (mainMenuButton.bounds.Height / 2f) - (MaintextSize.Y / 2f);

            Utility.drawTextWithShadow(
                b,
                mainMenutext,
                Game1.smallFont,
                new Vector2(MaintextX, MaintextY),
                Game1.textColor
            );

            string dropText = T("SetDropLocationButton");
            Vector2 dropTextSize = Game1.smallFont.MeasureString(dropText);

            int DropLocationadjustedWidth = (int)dropTextSize.X + 40;

            setDropLocationButton.bounds = new Rectangle(
                setDropLocationButton.bounds.X,
                setDropLocationButton.bounds.Y,
                DropLocationadjustedWidth,
                setDropLocationButton.bounds.Height
            );

            Color DropButtonColor = setDropLocationButton.containsPoint(Game1.getMouseX(), Game1.getMouseY())
                                ? Color.LimeGreen
                                : Color.White;

            drawTextureBox(
                b,
                Game1.menuTexture,
                new Rectangle(0, 256, 60, 60),
                setDropLocationButton.bounds.X,
                setDropLocationButton.bounds.Y,
                setDropLocationButton.bounds.Width,
                setDropLocationButton.bounds.Height,
                DropButtonColor,
                1f,
                false
            );

            float droptextX = setDropLocationButton.bounds.X + (setDropLocationButton.bounds.Width / 2f) - (dropTextSize.X / 2f);
            float droptextY = setDropLocationButton.bounds.Y + (setDropLocationButton.bounds.Height / 2f) - (dropTextSize.Y / 2f);

            Utility.drawTextWithShadow(
                b,
                dropText,
                Game1.smallFont,
                new Vector2(droptextX, droptextY),
                Game1.textColor
            );

            string RectangleText = T("RectangleButton");
            Vector2 RectangleTextSize = Game1.smallFont.MeasureString(RectangleText);

            int RectangleAdjustedWidth = (int)RectangleTextSize.X + 40;

            RectangleButton.bounds = new Rectangle(
                RectangleButton.bounds.X,
                RectangleButton.bounds.Y,
                RectangleAdjustedWidth,
                RectangleButton.bounds.Height
            );

            Color RectangleButtonColor = RectangleButton.containsPoint(Game1.getMouseX(), Game1.getMouseY())
                                ? Color.LimeGreen
                                : Color.White;

            drawTextureBox(
                b,
                Game1.menuTexture,
                new Rectangle(0, 256, 60, 60),
                RectangleButton.bounds.X,
                RectangleButton.bounds.Y,
                RectangleButton.bounds.Width,
                RectangleButton.bounds.Height,
                RectangleButtonColor,
                1f,
                false
            );

            float RectangleTextX = RectangleButton.bounds.X + (RectangleButton.bounds.Width / 2f) - (RectangleTextSize.X / 2f);
            float RectangleTextY = RectangleButton.bounds.Y + (RectangleButton.bounds.Height / 2f) - (RectangleTextSize.Y / 2f);

            Utility.drawTextWithShadow(
                b,
                RectangleText,
                Game1.smallFont,
                new Vector2(RectangleTextX, RectangleTextY),
                Game1.textColor
            );

            drawMouse(b);

            if (mainMenuButton.containsPoint(Game1.getMouseX(), Game1.getMouseY()))
            {
                drawHoverText(
                    b,
                    MainMenuTooltip,
                    Game1.smallFont,
                    xOffset: -10
                );
            }

            if (setDropLocationButton.containsPoint(Game1.getMouseX(), Game1.getMouseY()))
            {
                drawHoverText(
                    b,
                    DropLocationTooltip,
                    Game1.smallFont,
                    xOffset: -150
                );
            }

            if (RectangleButton.containsPoint(Game1.getMouseX(), Game1.getMouseY()))
            {
                drawHoverText(
                    b,
                    RectangleTooltip,
                    Game1.smallFont,
                    xOffset: -300
                );
            }
        }
    }
}
