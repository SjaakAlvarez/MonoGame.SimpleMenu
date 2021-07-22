using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.SimpleMenu.Demo.Configuration;
using MonoGame.SimpleMenu.Menu;
using System.Linq;

namespace MonoGame.SimpleMenu.Demo
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        string lastChange="Last Change: <nothing>";
        SpriteFont font;


        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);           
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
            _graphics.ApplyChanges();

            SettingsMenu<DemoConfiguration> menu = new SettingsMenu<DemoConfiguration>(this, Content, "game.json");
            menu.MenuCloseEvent += Menu_MenuCloseEvent;
            menu.MenuItemChangedEvent += Menu_MenuItemChangedEvent;
            Components.Add(menu);
           
            base.Initialize();
        }

        private void Menu_MenuItemChangedEvent(object sender, Events.MenuItemChangedEventArgs e)
        {
            lastChange = string.Format("Last Change: {0}={1}", e.Name, e.Value);
        }

        private void Menu_MenuCloseEvent(object sender, System.EventArgs e)
        {
            Components.Remove((DrawableGameComponent)sender);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/Arcade");
        }

        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();            

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            base.Draw(gameTime);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, lastChange, new Vector2(16, 512),Color.Cyan);
            _spriteBatch.End();
        }
    }
}
