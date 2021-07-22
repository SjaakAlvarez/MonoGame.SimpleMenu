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
        string lastChange="LAST CHANGE:";
        SpriteFont font;
        private KeyboardState current, last;


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
           
            base.Initialize();
        }

        private void Menu_MenuItemChangedEvent(object sender, Events.MenuItemChangedEventArgs e)
        {
            lastChange = string.Format("LAST CHANGE: {0}={1}", e.Name, e.Value);
        }

        private void Menu_MenuCloseEvent(object sender, System.EventArgs e)
        {            
            Components.Remove((DrawableGameComponent)sender);
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/Arcade");
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {

            KeyboardState current = Keyboard.GetState();

            if (current.IsKeyDown(Keys.Escape))
                Exit();  
            
            if(current.IsKeyDown(Keys.D1) && last.IsKeyUp(Keys.D1) && Components.Count==0)
            {
                SettingsMenu<DemoConfiguration> menu = new SettingsMenu<DemoConfiguration>(this, "game.json");
                menu.MenuCloseEvent += Menu_MenuCloseEvent;
                menu.MenuItemChangedEvent += Menu_MenuItemChangedEvent;
                Components.Add(menu);
            }

            if (current.IsKeyDown(Keys.D2) && last.IsKeyUp(Keys.D2) && Components.Count == 0)
            {
                SettingsMenu<ItemDemoConfiguration> menu = new SettingsMenu<ItemDemoConfiguration>(this, "game2.json");
                menu.MenuCloseEvent += Menu_MenuCloseEvent;
                menu.MenuItemChangedEvent += Menu_MenuItemChangedEvent;
                Components.Add(menu);
            }

            last = current;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);                     

            base.Draw(gameTime);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, "SIMPLE MENU DEMO", new Vector2(16, 584 - 48), Color.Yellow);
            _spriteBatch.DrawString(font, "PRESS '1' OR '2' FOR DEMO MENUS", new Vector2(16, 584-24),Color.Cyan);
            _spriteBatch.DrawString(font, lastChange, new Vector2(16, 584), Color.Cyan);
            _spriteBatch.End();
        }
    }
}
