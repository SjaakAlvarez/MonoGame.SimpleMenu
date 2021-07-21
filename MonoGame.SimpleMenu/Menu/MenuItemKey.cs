using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace MonoGame.SimpleMenu.Menu
{

    public class MenuItemKey : MenuItem
    {
        SpriteFont font;
        
        ContentManager content;
        SpriteBatch spriteBatch;
        protected double lastBlink;
        protected Color blinkColor = Color.White;

        public override String ItemName { get; set; }       

        private Action<Keys> setter;
        
        
        public Keys value;

        public Vector2 pos;
        Texture2D hammer;
        Boolean polling = false;

        public override Boolean ItemEnabled {get;set;}

        public delegate void SelectHandler(object sender, EventArgs e);
        public event SelectHandler Select;

        protected virtual void OnSelect(EventArgs e)
        {
            if (Select != null)
                Select(this, e);
        }

        public MenuItemKey(Game game,Vector2 pos,String itemName, Keys value,  Action<Keys> setter)
            : base(game)
        {
            this.ItemName = itemName;
            this.value = value;                  
            this.pos = pos;
            this.setter = setter;          
        }

     

       

        public override void SelectItem()
        {
            polling = true;
            OnSelect(new EventArgs());          
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

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (polling)
            {
                KeyboardState kbs = Keyboard.GetState();
                Keys[] keys = kbs.GetPressedKeys();

                if (kbs.GetPressedKeyCount() == 1 && keys[0] != Keys.Enter)
                {
                    value = keys[0];
                    setter(value);
                    polling = false;
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
           
            if(ItemEnabled) spriteBatch.Draw(hammer, new Vector2(4, pos.Y), new Rectangle(32,0,16,16), Color.White);
            spriteBatch.DrawString(font, ItemName.ToUpper(), new Vector2(2*16,pos.Y), (ItemEnabled?Color.White:Color.DarkGray)*alpha,rotation, offset, size, SpriteEffects.None, 0.0f);
            Color c = polling ? Color.Yellow : Color.White;
            if (polling)
            {
                spriteBatch.DrawString(font, "Press any key", new Vector2(16 * 16, pos.Y), (ItemEnabled ? blinkColor : Color.DarkGray) * alpha, rotation, offset, size, SpriteEffects.None, 0.0f);
            }
            else
            {
                spriteBatch.DrawString(font, value == null ? "" : value.ToString().ToUpper(), new Vector2(16 * 16, pos.Y), (ItemEnabled ? Color.White : Color.DarkGray) * alpha, rotation, offset, size, SpriteEffects.None, 0.0f);
            }
           
            spriteBatch.End();

        }
    }
}
