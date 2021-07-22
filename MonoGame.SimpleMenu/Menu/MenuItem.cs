using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.SimpleMenu.Events;
using System;

namespace MonoGame.SimpleMenu.Menu
{
    public class MenuItem : DrawableGameComponent
    {
        public bool ItemEnabled { get; set; }
        public string ItemName { get; set; }
        protected Vector2 pos;
        protected SpriteFont font;        
        protected SpriteBatch spriteBatch;
        protected Texture2D wingdings;
        private readonly Game game;

        protected MenuItem(Game game) : base(game) {
            this.game = game;
        }
       
        public delegate void SelectHandler(object sender, MenuItemChangedEventArgs e);
        public event SelectHandler SelectEvent;

        protected virtual void OnSelect(MenuItemChangedEventArgs e)
        {
            SelectEvent?.Invoke(this, e);
        }
       
        public virtual void SelectItem()
        {
            throw new NotSupportedException();
        }

        protected override void LoadContent()
        {
            base.LoadContent();            
            spriteBatch = new SpriteBatch(game.GraphicsDevice);            
        }

        

    }
}
