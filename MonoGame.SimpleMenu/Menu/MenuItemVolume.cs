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


            if(ItemEnabled) spriteBatch.Draw(wingdings, new Vector2(4, pos.Y), new Rectangle(32, 0, 16, 16), Color.White);
            spriteBatch.DrawString(font, ItemName.ToUpper(), new Vector2(2*16,pos.Y), (ItemEnabled?Color.White:Color.DarkGray)*alpha,rotation, offset, size, SpriteEffects.None, 0.0f);
            for (int i = Value; i < 10; i++)
            {
                spriteBatch.Draw(wingdings, new Vector2(16 * 16 + i * 16, pos.Y), new Rectangle(16, 0, 16, 16), (ItemEnabled ? colors[i] : ColorToGray(colors[i])) * alpha);                
            }
            for (int i = 0; i < Value; i++)
            {
                spriteBatch.Draw(wingdings, new Vector2(16 * 16 + i * 16, pos.Y),new Rectangle(0,0,16,16), (ItemEnabled ? colors[i] : ColorToGray(colors[i])) * alpha);                
            }                        

            spriteBatch.End();

        }
        
    }
}
