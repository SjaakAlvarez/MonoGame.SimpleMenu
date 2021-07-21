using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame.SimpleMenu.Menu
{

    public class MenuItemAction: MenuItem
    {
        SpriteFont font;
        
        ContentManager content;
        SpriteBatch spriteBatch;

        public override String ItemName { get; set; }             

        public Vector2 pos;
        Texture2D hammer;

        public override Boolean ItemEnabled {get;set;}

        public delegate void SelectHandler(object sender, EventArgs e);
        public event SelectHandler Select;

        protected virtual void OnSelect(EventArgs e)
        {
            if (Select != null)
                Select(this, e);
        }

        public MenuItemAction(Game game,Vector2 pos,String itemName)
            : base(game)
        {
            this.ItemName = itemName;                       
            this.pos = pos;      
        }

     

       

        public override void SelectItem()
        {                        
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



        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            Vector2 offset = Vector2.Zero;

            float size=1.0f;            
            float rotation=0.0f;
            float alpha = 1.0f;                           

            if(ItemEnabled) spriteBatch.Draw(hammer, new Vector2(4, pos.Y), new Rectangle(32,0,16,16), Color.White);
            spriteBatch.DrawString(font, ItemName.ToUpper(), new Vector2(2*16,pos.Y), (ItemEnabled?Color.White:Color.DarkGray)*alpha,rotation, offset, size, SpriteEffects.None, 0.0f);                       
            spriteBatch.End();

        }
    }
}
