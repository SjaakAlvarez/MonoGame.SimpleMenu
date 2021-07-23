using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.SimpleMenu.Menu;

namespace MonoGame.SimpleMenu.Demo
{
    public class CustomSettingsMenu<T> : SettingsMenu<T>
    {
        public CustomSettingsMenu(Game game, T myConfiguration) : base(game, myConfiguration)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach (MenuItem item in items)
            {

                if (item.ItemEnabled) spriteBatch.DrawString(font, "*", new Vector2(8, (int)item.pos.Y), Color.LimeGreen);
                spriteBatch.DrawString(font, item.ItemName.ToUpper(), new Vector2(2 * 16, item.pos.Y), (item.ItemEnabled ? Color.LimeGreen : Color.Green), 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);

                if (item is MenuItemMulti<object> mim)
                {
                    spriteBatch.DrawString(font, mim.Value == null ? "" : mim.Value.ToString().ToUpper(), new Vector2(16 * 16, mim.pos.Y), (mim.ItemEnabled ? Color.LimeGreen : Color.Green), 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                }
                else if (item is MenuItemVolume miv)
                {
                    for (int i = miv.Value; i < 10; i++)
                    {
                        spriteBatch.Draw(wingdings, new Rectangle(256 + i * 16, (int)miv.pos.Y + font.LineSpacing - 4, 14, 2), new Rectangle(0, 0, 1, 1), (miv.ItemEnabled ? miv.Colors[i] : miv.ColorToGray(miv.Colors[i])));
                    }
                    for (int i = 0; i < miv.Value; i++)
                    {
                        spriteBatch.Draw(wingdings, new Rectangle(256 + i * 16, (int)miv.pos.Y - 2, 14, font.LineSpacing), new Rectangle(0, 0, 1, 1), (miv.ItemEnabled ? miv.Colors[i] : miv.ColorToGray(miv.Colors[i])));
                    }
                }
                else if (item is MenuItemKey mik)
                {
                    if (mik.Polling)
                    {
                        spriteBatch.DrawString(font, "Press any key", new Vector2(16 * 16, mik.pos.Y), (mik.ItemEnabled ? mik.blinkColor : Color.Green), 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    }
                    else
                    {
                        spriteBatch.DrawString(font, mik.Value.ToString().ToUpper(), new Vector2(16 * 16, mik.pos.Y), (mik.ItemEnabled ? Color.LimeGreen : Color.Green), 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    }
                }

             
            }
            spriteBatch.End();
        }
    }
}
