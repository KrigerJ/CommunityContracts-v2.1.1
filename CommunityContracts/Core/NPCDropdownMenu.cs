using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Menus;
using SDV_NPC = StardewValley.NPC;
using static ModEntry;

namespace CommunityContracts.Core
{
    public class NPCDropdownMenu : IClickableMenu
    {
        private List<string> npcNames = new List<string> { "Abigail", "Alex", "Caroline", "Demetrius", "Elliott", "Emily", "Evelyn", "George", "Haley", "Jas", "Jodi", "Leah", "Leo", "Linus", "Maru", "Pam", "Penny", "Sam", "Sandy", "Sebastian", "Shane", "Vincent", "Wizard" };
        private List<NPCMenuOption> options = new List<NPCMenuOption>();
        private Dictionary<string, Texture2D> npcPortraits = new();
        private int selectedIndex = -1;
        private const int ButtonWidth = 160;
        private const int ButtonHeight = 70;
        private const int Columns = 4;
        private const int HSpacing = 10;
        private const int WSpacing = 120;
        private static string ReturnToolbarTooltip => T("ReturnToolbarTooltip");
        private ClickableComponent ReturnToolbarButton;
        public class NPCMenuOption
        {
            public string name;
            public ClickableComponent nameButton;
            public Rectangle portraitRect;
        }
        public NPCDropdownMenu()
        {
            int startX = 120;
            int startY = 120;


            foreach (var name in npcNames)
            {
                try
                {
                    npcPortraits[name] = Game1.content.Load<Texture2D>($"Portraits/{name}");
                }
                catch
                {

                }
            }

            for (int i = 0; i < npcNames.Count; i++)
            {
                string name = npcNames[i];
                SDV_NPC npc = Game1.getCharacterFromName(name, mustBeVillager: false);

                if (npc == null || npc.currentLocation == null || npc.Position == Vector2.Zero)
                {
                    Instance.Monitor.Log(T("SkippingNPC", new { npc = name }), LogLevel.Trace);
                    continue;
                }

                int col = options.Count % Columns;
                int row = options.Count / Columns;

                int x = startX + col * (ButtonWidth + WSpacing);
                int y = startY + row * (ButtonHeight + HSpacing);

                var nameButton = new ClickableComponent(new Rectangle(x, y, ButtonWidth, ButtonHeight), name);
                var portraitRect = new Rectangle(x - 70, y, 64, 64);

                options.Add(new NPCMenuOption
                {
                    name = name,
                    nameButton = nameButton,
                    portraitRect = portraitRect
                });
            }

            int buttonX = startX + 330;
            int buttonY = startY - 70;

            ReturnToolbarButton = new ClickableComponent(
                new Rectangle(buttonX, buttonY, 440, 60),
                "ReturnToolbar"
            ); 
        }
        public override void draw(SpriteBatch b)
        {
            int framePadding = 20;
            int frameWidth = (ButtonWidth + WSpacing) * Columns + framePadding * 6 - 80;
            int frameHeight = ((npcNames.Count + Columns) / Columns) * (ButtonHeight + HSpacing * 2) + framePadding * 2 + 120;
            int frameX = 20;
            int frameY = 20;

            drawTextureBox(
                b,
                Game1.menuTexture,
                new Rectangle(0, 256, 60, 60),
                frameX,
                frameY,
                frameWidth,
                frameHeight,
                Color.White,
                drawShadow: false
            );

            Color ReturnToolbarColor = ReturnToolbarButton.containsPoint(Game1.getMouseX(), Game1.getMouseY())
                                ? Color.LimeGreen
                                : Color.White;

            drawTextureBox(
                b,
                Game1.menuTexture,
                new Rectangle(0, 256, 60, 60),
                ReturnToolbarButton.bounds.X,
                ReturnToolbarButton.bounds.Y,
                ReturnToolbarButton.bounds.Width,
                ReturnToolbarButton.bounds.Height,
                ReturnToolbarColor,
                1f,
                false
            );

            string text = T("ReturnToolbar");
            Vector2 textSize = Game1.smallFont.MeasureString(text);

            float textX = ReturnToolbarButton.bounds.X + (ReturnToolbarButton.bounds.Width / 2f) - (textSize.X / 2f);
            float textY = ReturnToolbarButton.bounds.Y + (ReturnToolbarButton.bounds.Height / 2f) - (textSize.Y / 2f);

            Utility.drawTextWithShadow(
                b,
                text,
                Game1.smallFont,
                new Vector2(textX, textY),
                Game1.textColor
            );

            string ReturnToolbarText = T("ReturnToolbar");
            Vector2 ReturnToolbarTextSize = Game1.smallFont.MeasureString(ReturnToolbarText);

            int ReturnToolbaradjustedWidth = (int)ReturnToolbarTextSize.X + 40;

            ReturnToolbarButton.bounds = new Rectangle(
                ReturnToolbarButton.bounds.X,
                ReturnToolbarButton.bounds.Y,
                ReturnToolbaradjustedWidth,
                ReturnToolbarButton.bounds.Height
            );

            base.draw(b);

            foreach (var option in options)
            {
                Color boxColor = option.nameButton.containsPoint(Game1.getMouseX(), Game1.getMouseY()) ? Color.Gold : Color.White;

                drawTextureBox(
                    b,
                    Game1.menuTexture,
                    new Rectangle(0, 256, 60, 60),
                    option.nameButton.bounds.X,
                    option.nameButton.bounds.Y,
                    option.nameButton.bounds.Width,
                    option.nameButton.bounds.Height,
                    boxColor,
                    1f,
                    false
                );

                if (npcPortraits.TryGetValue(option.name, out var portrait))
                {
                    Rectangle sourceRect = new Rectangle(0, 0, 64, 64);
                    b.Draw(portrait, option.portraitRect, sourceRect, Color.White);
                }

                Vector2 NametextSize = Game1.smallFont.MeasureString(option.name);

                float NametextX = option.nameButton.bounds.X + (option.nameButton.bounds.Width / 2f) - (NametextSize.X / 2f);
                float NametextY = option.nameButton.bounds.Y + (option.nameButton.bounds.Height / 2f) - (NametextSize.Y / 2f);

                Utility.drawTextWithShadow(
                    b,
                    option.name,
                    Game1.smallFont,
                    new Vector2(NametextX, NametextY),
                    Game1.textColor
                );
            }

            drawMouse(b);

            foreach (var option in options)
            {
                if (option.portraitRect.Contains(Game1.getMouseX(), Game1.getMouseY()))
                {
                    string tooltip = T("WarpToLocation", new { npc = option.name });

                    drawHoverText(
                        b,
                        tooltip,
                        Game1.smallFont
                    );
                    break;
                }
            }

            foreach (var option in options)
            {
                if (option.nameButton.containsPoint(Game1.getMouseX(), Game1.getMouseY()))
                {
                    string tooltip = T("GoToMenu", new { npc = option.name });

                    drawHoverText(
                        b,
                        tooltip,
                        Game1.smallFont,
                        xOffset: -300
                    );
                    break;
                }
            }
            
            if (ReturnToolbarButton.containsPoint(Game1.getMouseX(), Game1.getMouseY()))
            {
                drawHoverText(
                    b,
                    ReturnToolbarTooltip,
                    Game1.smallFont,
                    xOffset: -300
                );
            }
        }
        public override void receiveLeftClick(int x, int y, bool playSound = true)
        {
            foreach (var option in options)
            {
                string npcName = option.name;
                SDV_NPC npc = Game1.getCharacterFromName(npcName, mustBeVillager: false);

                if (option.portraitRect.Contains(x, y))
                {
                    Vector2 tile = npc.Position / Game1.tileSize;
                    Game1.warpFarmer(npc.currentLocation.Name, (int)tile.X, (int)tile.Y, false);
                    Game1.exitActiveMenu();
                    Game1.playSound("wand");
                    return;
                }

                if (option.nameButton.containsPoint(x, y))
                {
                    Game1.exitActiveMenu();
                    Game1.activeClickableMenu = new NPCServiceMenu(npcName);
                    return;
                }
                
                if (ReturnToolbarButton.containsPoint(x, y))
                {
                    Game1.exitActiveMenu();
                    Game1.activeClickableMenu = new CCToolbar();
                    return;
                } 
            }

            base.receiveLeftClick(x, y, playSound);
        }
    }
}
