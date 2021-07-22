using System;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.SimpleMenu.Events;

namespace MonoGame.SimpleMenu.Menu
{
    public abstract class MenuItem : DrawableGameComponent
    {
        public MenuItem(Game game):base(game)
        {
        }

        public abstract void SelectItem();    

        public abstract Boolean ItemEnabled {get;set;}
        public abstract String ItemName { get; set; }

        public delegate void SelectHandler(object sender, MenuItemChangedEventArgs e);
        public event SelectHandler SelectEvent;

        protected virtual void OnSelect(MenuItemChangedEventArgs e)
        {
            SelectEvent?.Invoke(this, e);
        }
    }

    public class MenuItem<T> : MenuItem
    {
        SpriteFont font;        
        ContentManager content;
        SpriteBatch spriteBatch;

        public override string ItemName { get; set; }       

        private Action<T> setter;
        
        private T[] Values { get; set; }
        private T Value { get; set; }

        public Vector2 pos;
        Texture2D hammer;

        public override Boolean ItemEnabled {get;set;}

        

        public MenuItem(Game game,Vector2 pos,String itemName, T value, T[] itemValues,  Action<T> setter)
            : base(game)
        {
            this.ItemName = itemName;
            this.Value = value;
            this.Values = itemValues;            
            this.pos = pos;
            this.setter = setter;          
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

        protected override void LoadContent()
        {
            base.LoadContent();
            if (content == null)
                content = new ContentManager(Game.Services, "Content");

            font = content.Load<SpriteFont>("Fonts/Arcade");            
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            hammer = content.Load<Texture2D>("Graphics/wingdings");
        }

        
        protected override void UnloadContent()
        {
            content.Unload();
            base.UnloadContent();
        }



        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            Vector2 offset = Vector2.Zero;

            float size=1.0f;            
            float rotation=0.0f;
            float alpha = 1.0f;                           

            if(ItemEnabled) spriteBatch.Draw(hammer, new Vector2(4, pos.Y), new Rectangle(32,0,16,16), Color.White);
            spriteBatch.DrawString(font, ItemName.ToUpper(), new Vector2(2*16,pos.Y), (ItemEnabled?Color.White:Color.DarkGray)*alpha,rotation, offset, size, SpriteEffects.None, 0.0f);
            spriteBatch.DrawString(font, Value==null?"":Value.ToString().ToUpper(), new Vector2(16*16, pos.Y), (ItemEnabled ? Color.White : Color.DarkGray) * alpha, rotation, offset, size, SpriteEffects.None, 0.0f);           
            spriteBatch.End();

        }
    }
}
