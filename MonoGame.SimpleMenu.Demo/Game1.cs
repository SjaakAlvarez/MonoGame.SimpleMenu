using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.SimpleMenu.Demo.Configuration;
using MonoGame.SimpleMenu.Events;
using MonoGame.SimpleMenu.Menu;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace MonoGame.SimpleMenu.Demo
{
    public class Game1 : Game
    {
        private readonly GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        string lastChange="LAST CHANGE:";
        SpriteFont font;
        private KeyboardState current, last;
        private DemoConfiguration demoConfiguration;        

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

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/Arcade");
            base.LoadContent();
        }

        protected override void Update(GameTime gameTime)
        {
            current = Keyboard.GetState();

            if (current.IsKeyDown(Keys.Escape))
                Exit();  
            
            if(current.IsKeyDown(Keys.D1) && last.IsKeyUp(Keys.D1) && Components.Count==0)
            {
                createDemoMenu();
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
            _spriteBatch.DrawString(font, "PRESS '1' FOR DEMO MENU", new Vector2(16, 584-24),Color.Cyan);
            _spriteBatch.DrawString(font, lastChange, new Vector2(16, 584), Color.Cyan);
            _spriteBatch.End();
        }

        #region Menu Stuff
        private void Menu_MenuItemChangedEvent(object sender, Events.MenuItemChangedEventArgs e)
        {
            lastChange = string.Format("LAST CHANGE: {0}={1}", e.Name, e.Value);
        }

        private void Menu_DemoMenuCloseEvent(object sender, MenuCloseEventArgs e)
        {
            if (e.Save)
            {
                JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(demoConfiguration, options);
                File.WriteAllText("game.json", jsonString);
            }
            Components.Remove((DrawableGameComponent)sender);
        }

        private void createDemoMenu()
        {
            if (File.Exists("game.json"))
            {
                string jsonString = File.ReadAllText("game.json");
                demoConfiguration = JsonSerializer.Deserialize<DemoConfiguration>(jsonString);
            }
            else
            {
                demoConfiguration = new DemoConfiguration();
            }
            SettingsMenu<DemoConfiguration> menu = new SettingsMenu<DemoConfiguration>(this, demoConfiguration);
            menu.MenuCloseEvent += Menu_DemoMenuCloseEvent;
            menu.MenuItemChangedEvent += Menu_MenuItemChangedEvent;
            Components.Add(menu);
        }
        #endregion
    }
}
