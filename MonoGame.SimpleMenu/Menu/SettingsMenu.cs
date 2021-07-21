using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Reflection;
using MonoGame.SimpleMenu.Attributes;
using System.Linq;
using System.IO;
using System.Text.Json;

namespace MonoGame.SimpleMenu.Menu
{
    public class SettingsMenu<T> : DrawableGameComponent
    {        
        SoundEffect bloop;       
        protected SpriteBatch spriteBatch;

        int idx = 0;
        float y = 16;
        Boolean repeat;
        double repeatTime;

        protected KeyboardState current, last;

        protected List<MenuItem> items = new List<MenuItem>();
        private readonly T myConfiguration;
        private readonly string filename;
        private readonly Game game;
        private readonly ContentManager content;
       


        public SettingsMenu(Game game, ContentManager content, string filename):base(game)
        {            
            this.filename = filename;
            this.game = game;
            this.content = content;

            if (File.Exists(filename))
            {
                string jsonString = File.ReadAllText(filename);
                myConfiguration = JsonSerializer.Deserialize<T>(jsonString);
            }
            else
            {
                myConfiguration = Activator.CreateInstance<T>();
            }
        }

       
       
        public override void Initialize()
        {                        
            spriteBatch = new SpriteBatch(game.GraphicsDevice);           
            bloop = content.Load<SoundEffect>("Sounds/beep");            

            Type type = myConfiguration.GetType();
            IOrderedEnumerable<PropertyInfo> memberInfo = type.GetProperties().OrderBy(p =>
                            ((OrderAttribute)(
                            p.GetCustomAttributes(typeof(OrderAttribute))
                            .First())).Order);                            

            Boolean enabled = true;

            foreach (PropertyInfo m in memberInfo)
            {
                foreach (Attribute attribute in m.GetCustomAttributes())
                {
                    if (attribute is ConfigurationAttribute ca)
                    {                                        
                        AddMenuItem(ca.Name, m.GetValue(myConfiguration), ca.Values, p => m.SetValue(myConfiguration, p), enabled);                       
                    }
                    else if (attribute is ConfigurationSliderAttribute csa)
                    {                        
                        AddMenuItemVolume(csa.Name, 0, 10, 1, (int)m.GetValue(myConfiguration), p => m.SetValue(myConfiguration, p), enabled);
                    }
                    else if (attribute is ConfigurationKeyAttribute cka)
                    {                        
                        AddMenuItemKey(cka.Name, (Keys)m.GetValue(myConfiguration), p => m.SetValue(myConfiguration, p), enabled);
                    }                    
                    else if (attribute is SpacerAttribute spacer)
                    {                        
                        AddSpacer();                        
                    }
                }
                enabled = false;
            }

            AddSpacer();
            MenuItemAction exitAction = AddMenuItemAction("Save and exit", enabled);
            exitAction.Select += ExitAction_Select;

        }

        private void ExitAction_Select(object sender, EventArgs e)
        {
            JsonSerializerOptions options = new JsonSerializerOptions() { WriteIndented = true };            
            string jsonString = JsonSerializer.Serialize(myConfiguration, options);
            File.WriteAllText(filename, jsonString);
            //ScreenManager.RemoveScreen(this);
        }

        public void AddSpacer()
        {
            y+=16;
        }
       

        public MenuItem<T> AddMenuItem<T>(String name, T value, T[] values, Action<T> setter, Boolean enabled = false)
        {
            MenuItem<T> item = new MenuItem<T>(game, new Vector2(240, y),name, value, values,setter);
            item.Initialize();
            item.ItemEnabled = enabled;
            items.Add(item);
            y += 24;
            return item;
        }

        public MenuItemKey AddMenuItemKey(String name, Keys value,  Action<Keys> setter, Boolean enabled = false)
        {
            MenuItemKey item = new MenuItemKey(game, new Vector2(240, y), name, value, setter);
            item.Initialize();
            item.ItemEnabled = enabled;
            items.Add(item);
            y += 24;
            return item;
        }

        public MenuItemAction AddMenuItemAction(String name, Boolean enabled = false)
        {
            MenuItemAction item = new MenuItemAction(game, new Vector2(240, y), name);
            item.Initialize();
            item.ItemEnabled = enabled;
            items.Add(item);
            y += 24;
            return item;
        }


        public MenuItemVolume AddMenuItemVolume(String name, int min, int max, int step, int value, Action<int> setter, Boolean enabled = false)
        {
            MenuItemVolume item = new MenuItemVolume(game, new Vector2(240, y), name, min,max, step,value, setter);
            item.Initialize();
            item.ItemEnabled = enabled;
            items.Add(item);
            y += 24;
            return item;
        }

     

        public Boolean KeyPressed(Keys key, Boolean repeat = false)
        {
            if (current.IsKeyDown(key) && (last.IsKeyUp(key) || repeat)) return true;
            return false;
        }

        public void UnloadContent()
        {            
            content.Unload();
            foreach(MenuItem item in items) item.Dispose();
        }        

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            last = current;
            current = Keyboard.GetState();

            if (gameTime.TotalGameTime.TotalMilliseconds > repeatTime + 250)
            {
                repeatTime = gameTime.TotalGameTime.TotalMilliseconds;
                repeat = true;
            }

            if (KeyPressed(Keys.Down, repeat))
            {
                if (idx < items.Count - 1)
                {
                    items[idx].ItemEnabled = false;
                    idx++;
                    items[idx].ItemEnabled = true;
                    bloop.Play();
                }
                repeatTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (KeyPressed(Keys.Up, repeat))
            {
                if (idx > 0)
                {
                    items[idx].ItemEnabled = false;
                    idx--;
                    items[idx].ItemEnabled = true;
                    bloop.Play();
                }
                repeatTime = gameTime.TotalGameTime.TotalMilliseconds;
            }
            if (KeyPressed(Keys.Enter))
            {
                items[idx].SelectItem();
                if (!bloop.IsDisposed) bloop.Play();
            }

            last = current;
            repeat = false;

            foreach (MenuItem item in items) item.Update(gameTime);
        }


       
        public override void Draw(GameTime gameTime)
        {            
            spriteBatch.Begin();
            foreach(MenuItem item in items)
            {
                item.Draw(gameTime);
            }           
            spriteBatch.End();
        }

    }

    
}
