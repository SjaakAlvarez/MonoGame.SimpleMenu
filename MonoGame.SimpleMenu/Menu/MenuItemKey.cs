using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.SimpleMenu.Events;

namespace MonoGame.SimpleMenu.Menu
{

    public class MenuItemKey : MenuItem
    {               
        protected double lastBlink;
        public Color blinkColor { get; set; } = Color.White;           
        private readonly Action<Keys> setter;                
        public Keys Value { get; set; }              
        public Boolean Polling { get; set; }
              

        public MenuItemKey(Game game,Vector2 pos,String itemName, Keys value,  Action<Keys> setter, SpriteFont font, Texture2D wingdings)
            : base(game)
        {
            this.ItemName = itemName;
            this.Value = value;                  
            this.pos = pos;
            this.setter = setter;
            this.font = font;
            this.wingdings = wingdings;
        }
    

        public override void SelectItem()
        {
            Polling = true;            
        }
       

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (Polling)
            {
                KeyboardState kbs = Keyboard.GetState();
                Keys[] keys = kbs.GetPressedKeys();

                if (kbs.GetPressedKeyCount() == 1 && keys[0] != Keys.Enter)
                {
                    Value = keys[0];
                    setter(Value);
                    Polling = false;
                    OnSelect(new MenuItemChangedEventArgs(ItemName, Value));
                }

            }

            if (gameTime.TotalGameTime.TotalMilliseconds > lastBlink + 500)
            {
                lastBlink = gameTime.TotalGameTime.TotalMilliseconds;
                blinkColor = blinkColor == Color.Red ? Color.White : Color.Red;
            }
        }


        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            Vector2 offset = Vector2.Zero;

            float size=1.0f;            
            float rotation=0.0f;
            float alpha= 1.0f;


            if (ItemEnabled) spriteBatch.DrawString(font,">", new Vector2(8, (int)pos.Y ), Color.White);
            spriteBatch.DrawString(font, ItemName.ToUpper(), new Vector2(2*16,pos.Y), (ItemEnabled?Color.White:Color.DarkGray)*alpha,rotation, offset, size, SpriteEffects.None, 0.0f);            
            if (Polling)
            {
                spriteBatch.DrawString(font, "Press any key", new Vector2(16 * 16, pos.Y), (ItemEnabled ? blinkColor : Color.DarkGray) * alpha, rotation, offset, size, SpriteEffects.None, 0.0f);
            }
            else
            {
                spriteBatch.DrawString(font, Value.ToString().ToUpper(), new Vector2(16 * 16, pos.Y), (ItemEnabled ? Color.White : Color.DarkGray) * alpha, rotation, offset, size, SpriteEffects.None, 0.0f);
            }
           
            spriteBatch.End();

        }
    }
}
