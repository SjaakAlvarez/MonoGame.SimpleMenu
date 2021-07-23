using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.SimpleMenu.Events;

namespace MonoGame.SimpleMenu.Menu
{

    public class MenuItemAction: MenuItem
    {
        
        public MenuItemAction(Game game,Vector2 pos,String itemName, SpriteFont font, Texture2D wingdings)
            : base(game)
        {
            this.ItemName = itemName;                       
            this.pos = pos;
            this.font = font;
            this.wingdings = wingdings;
        }


        public override void SelectItem()
        {                        
            OnSelect(new MenuItemChangedEventArgs("",""));           
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
            spriteBatch.End();
        }
    }
}
