using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.SimpleMenu.Events;

namespace MonoGame.SimpleMenu.Menu
{

    public class MenuItemVolume : MenuItem 
    {       
        public int Value { get; set; }
        private readonly Color[] colors;
        private readonly Action<int> setter;        

        public MenuItemVolume(Game game,Vector2 pos,String itemName, int value, Action<int> setter, SpriteFont font, Texture2D wingdings)
            : base(game)
        {
            this.ItemName = itemName;                                      
            this.pos = pos;  
            this.setter = setter;
            this.Value = value;
            this.font = font;
            this.wingdings = wingdings;

            colors = new Color[10];
            for(int i=0;i<5;i++)
            {
                colors[i] = Color.Lerp(Color.DarkGreen, Color.Yellow, (float)i / 5.0f);
                colors[i+5] = Color.Lerp(Color.Yellow, Color.Red, (float)i / 5.0f);
            }
         
        }

        

        public override void SelectItem()
        {            
            if (Value<10)
            {                
                Value ++;                
            }
            else
            {
                Value = 0;
            }

            setter(Value);
            OnSelect(new MenuItemChangedEventArgs(ItemName, Value));           
        }      

        private Color ColorToGray(Color c)
        {
            int avg = (c.R + c.G + c.B) / 3;
            return new Color(avg,avg,avg);
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            Vector2 offset = Vector2.Zero;
            float size=1.0f;            
            float rotation=0.0f;
            float alpha= 1.0f;


            if (ItemEnabled) spriteBatch.DrawString(font, ">", new Vector2(8, (int)pos.Y), Color.White);
            spriteBatch.DrawString(font, ItemName.ToUpper(), new Vector2(2*16,pos.Y), (ItemEnabled?Color.White:Color.DarkGray)*alpha,rotation, offset, size, SpriteEffects.None, 0.0f);
            for (int i = Value; i < 10; i++)
            {                
                spriteBatch.Draw(wingdings, new Rectangle(256 + i * 16, (int)pos.Y+ font.LineSpacing-4, 14, 2), new Rectangle(0,0,1,1), (ItemEnabled ? colors[i] : ColorToGray(colors[i])) * alpha);                
            }
            for (int i = 0; i < Value; i++)
            {
                spriteBatch.Draw(wingdings, new Rectangle(256 + i * 16, (int)pos.Y-2, 14, font.LineSpacing), new Rectangle(0, 0, 1, 1), (ItemEnabled ? colors[i] : ColorToGray(colors[i])) * alpha);                
            }                        

            spriteBatch.End();

        }
        
    }
}
