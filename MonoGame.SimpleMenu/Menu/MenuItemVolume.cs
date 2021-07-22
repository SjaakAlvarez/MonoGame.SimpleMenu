using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.SimpleMenu.Events;

namespace MonoGame.SimpleMenu.Menu
{

    public class MenuItemVolume : MenuItem 
    {
        SpriteFont font;
        
        ContentManager content;
        SpriteBatch spriteBatch;


        public override String ItemName { get; set; }
        
        public int min;
        public int max;
        public int step;
        public int value;
      
        public Vector2 pos;
        Texture2D wingdings;
        Game game;
        Color[] colors;
        public override Boolean ItemEnabled {get;set;}

        
        private Action<int> setter;

        

        public MenuItemVolume(Game game,Vector2 pos,String itemName, int min, int max, int step,int value, Action<int> setter)
            : base(game)
        {
            this.ItemName = itemName;                  

            this.min = min;
            this.max = max;
            this.step = step;            
            this.pos = pos;
            this.game = game;
            this.setter = setter;
            this.value = value;

            colors = new Color[10];
            for(int i=0;i<5;i++)
            {
                colors[i] = Color.Lerp(Color.DarkGreen, Color.Yellow, (float)i / 5.0f);
                colors[i+5] = Color.Lerp(Color.Yellow, Color.Red, (float)i / 5.0f);
            }
        }
    

     

        public override void SelectItem()
        {
            
            if (value<max)
            {                
                value += step;                
            }
            else
            {
                value = min;
            }


            setter(value);
            OnSelect(new MenuItemChangedEventArgs(ItemName, value));
           
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            if (content == null)
                content = new ContentManager(Game.Services, "Content");

            font = content.Load<SpriteFont>("Fonts/Arcade");            
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);            
            wingdings = content.Load<Texture2D>("Graphics/wingdings");

        }

        
        protected override void UnloadContent()
        {
            content.Unload();
            base.UnloadContent();
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
            for (int i = value; i < 10; i++)
            {
                spriteBatch.Draw(wingdings, new Vector2(16 * 16 + i * 16, pos.Y), new Rectangle(16, 0, 16, 16), (ItemEnabled ? colors[i] : ColorToGray(colors[i])) * alpha);                
            }
            for (int i = 0; i < value; i++)
            {
                spriteBatch.Draw(wingdings, new Vector2(16 * 16 + i * 16, pos.Y),new Rectangle(0,0,16,16), (ItemEnabled ? colors[i] : ColorToGray(colors[i])) * alpha);                
            }            
            


            spriteBatch.End();

        }
    }
}
