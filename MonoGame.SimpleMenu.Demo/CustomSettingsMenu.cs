using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.SimpleMenu.Menu;

namespace MonoGame.SimpleMenu.Demo
{
    public class CustomSettingsMenu<T> : SettingsMenu<T>
    {
        private SoundEffect lasersound;        
        private Texture2D laseranim;
        private bool showAnimation;
        private double animationTime;
        private int animationFrame;

        public CustomSettingsMenu(Game game, T myConfiguration) : base(game, myConfiguration)
        {
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            lasersound = content.Load<SoundEffect>("Sounds/laser");            
            laseranim = content.Load<Texture2D>("Graphics/lasergunanim");
        }

        protected override void ItemChanged()
        {
            lasersound.Play();
            showAnimation = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (showAnimation)
            {
                if (gameTime.TotalGameTime.TotalMilliseconds > animationTime + 40)
                {
                    animationFrame++;
                    if (animationFrame == 9)
                    {
                        animationFrame = 0;
                        showAnimation = false;
                    }
                    animationTime = gameTime.TotalGameTime.TotalMilliseconds;
                }
            }

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            foreach (MenuItem item in items)
            {

                if (item.ItemEnabled)
                {
                    if (!showAnimation)
                    {
                        spriteBatch.Draw(laseranim, new Vector2(2, item.pos.Y - 8), new Rectangle(0, 0, 820, 64), Color.White, 0.0f, new Vector2(0, 16), 1.0f, SpriteEffects.None, 0);
                    }
                    else
                    {
                        spriteBatch.Draw(laseranim, new Vector2(2, item.pos.Y - 8), new Rectangle(0, (animationFrame * 64), 820, 64), Color.White, 0.0f, new Vector2(0, 16), 1.0f, SpriteEffects.None, 0);
                    }
                }
                
                spriteBatch.DrawString(font, item.ItemName.ToUpper(), new Vector2(3 * 16, item.pos.Y), (item.ItemEnabled ? Color.White : Color.DarkGray), 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);

                if (item is MenuItemMulti<object> mim)
                {
                    spriteBatch.DrawString(font, mim.Value == null ? "" : mim.Value.ToString().ToUpper(), new Vector2(16 * 16, mim.pos.Y), (mim.ItemEnabled ? Color.White : Color.DarkGray), 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
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
                        spriteBatch.DrawString(font, "Press any key", new Vector2(16 * 16, mik.pos.Y), (mik.ItemEnabled ? mik.blinkColor : Color.DarkGray), 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    }
                    else
                    {
                        spriteBatch.DrawString(font, mik.Value.ToString().ToUpper(), new Vector2(16 * 16, mik.pos.Y), (mik.ItemEnabled ? Color.White : Color.DarkGray), 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0.0f);
                    }
                }

             
            }
            spriteBatch.End();
        }
    }
}
