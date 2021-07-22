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
        protected Color blinkColor = Color.White;           
        private readonly Action<Keys> setter;                
        public Keys Value { get; set; }              
        Boolean polling = false;
              

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
            polling = true;            
        }
       

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (polling)
            {
                KeyboardState kbs = Keyboard.GetState();
                Keys[] keys = kbs.GetPressedKeys();

                if (kbs.GetPressedKeyCount() == 1 && keys[0] != Keys.Enter)
                {
                    Value = keys[0];
                    setter(Value);
                    polling = false;
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
           
            if(ItemEnabled) spriteBatch.Draw(wingdings, new Vector2(4, pos.Y), new Rectangle(32,0,16,16), Color.White);
            spriteBatch.DrawString(font, ItemName.ToUpper(), new Vector2(2*16,pos.Y), (ItemEnabled?Color.White:Color.DarkGray)*alpha,rotation, offset, size, SpriteEffects.None, 0.0f);
            Color c = polling ? Color.Yellow : Color.White;
            if (polling)
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
