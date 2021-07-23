using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.SimpleMenu.Events;

namespace MonoGame.SimpleMenu.Menu
{
 

    public class MenuItemMulti<T> : MenuItem
    {       
        private readonly Action<T> setter;        
        private T[] Values { get; set; }
        public T Value { get; set; }
       

        public MenuItemMulti(Game game,Vector2 pos,String itemName, T value, T[] itemValues,  Action<T> setter, SpriteFont font, Texture2D wingdings)
            : base(game)
        {
            this.ItemName = itemName;
            this.Value = value;
            this.Values = itemValues;            
            this.pos = pos;
            this.setter = setter;
            this.font = font;
            this.wingdings = wingdings;
        }

        public override void SelectItem()
        {            
            if (Values != null)
            {
                int idx = Values.ToList().IndexOf(Value);
                if (idx < Values.Length - 1) idx++; else idx = 0;
                Value = Values[idx];
                setter(Value);
            }
            OnSelect(new MenuItemChangedEventArgs(ItemName,Value));           
        }       

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            Vector2 offset = Vector2.Zero;

            float size=1.0f;            
            float rotation=0.0f;
            float alpha = 1.0f;

            if (ItemEnabled) spriteBatch.DrawString(font, ">", new Vector2(8, (int)pos.Y), Color.White);
            spriteBatch.DrawString(font, ItemName.ToUpper(), new Vector2(2*16,pos.Y), (ItemEnabled?Color.White:Color.DarkGray)*alpha,rotation, offset, size, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, Value==null?"":Value.ToString().ToUpper(), new Vector2(16*16, pos.Y), (ItemEnabled ? Color.White : Color.DarkGray) * alpha, rotation, offset, size, SpriteEffects.None, 0.0f);           
            spriteBatch.End();

        }
    }
}
