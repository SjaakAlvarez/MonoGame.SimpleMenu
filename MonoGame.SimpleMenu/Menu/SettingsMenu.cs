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
using MonoGame.SimpleMenu.Events;

namespace MonoGame.SimpleMenu.Menu
{
    public class SettingsMenu<T> : DrawableGameComponent
    {
        protected SpriteBatch spriteBatch;
        protected SoundEffect bloop;        
        protected SpriteFont font;
        protected Texture2D wingdings;
        protected ContentManager content;

        private int idx = 0;
        private float y = 16;
        private int spacing;
        private Boolean repeat;
        private double repeatTime;

        protected KeyboardState current, last;

        protected List<MenuItem> items = new List<MenuItem>();
        private readonly T myConfiguration;        
        private readonly Game game;        

        public delegate void MenuCloseEventHandler(object sender, MenuCloseEventArgs e);
        public event MenuCloseEventHandler MenuCloseEvent;
        protected virtual void OnMenuClose(MenuCloseEventArgs e)
        {
            MenuCloseEvent?.Invoke(this, e);
        }

        public delegate void MenuItemChangedEventHandler(object sender, MenuItemChangedEventArgs e);
        public event MenuItemChangedEventHandler MenuItemChangedEvent;
        protected virtual void MenuItemChanged(MenuItemChangedEventArgs e)
        {
            MenuItemChangedEvent?.Invoke(this, e);
        }


        public SettingsMenu(Game game, T myConfiguration) :base(game)
        {            
            this.myConfiguration=myConfiguration;
            this.game = game;                        
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            content = new ContentManager(Game.Services, "Content");
            bloop = content.Load<SoundEffect>("Sounds/beep");
            font = content.Load<SpriteFont>("Fonts/Arcade");               
        }

        protected override void UnloadContent()
        {
            content.Unload();
            foreach (MenuItem item in items) item.Dispose();
            base.UnloadContent();
        }


        
        public override void Initialize()
        {
            base.Initialize();

            spriteBatch = new SpriteBatch(game.GraphicsDevice);
            wingdings = new Texture2D(spriteBatch.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            wingdings.SetData(new[] { Color.White });

            spacing = (int)(font.LineSpacing * 1.5f);


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
                        MenuItem menuItem=AddMenuItem(ca.Name, m.GetValue(myConfiguration), ca.Values, p => m.SetValue(myConfiguration, p), enabled);
                        menuItem.SelectEvent += MenuItem_SelectEvent;
                    }
                    else if (attribute is ConfigurationSliderAttribute csa)
                    {
                        MenuItem menuItem = AddMenuItemVolume(csa.Name, (int)m.GetValue(myConfiguration), p => m.SetValue(myConfiguration, p), enabled);
                        menuItem.SelectEvent += MenuItem_SelectEvent;
                    }
                    else if (attribute is ConfigurationKeyAttribute cka)
                    {
                        MenuItem menuItem = AddMenuItemKey(cka.Name, (Keys)m.GetValue(myConfiguration), p => m.SetValue(myConfiguration, p), enabled);
                        menuItem.SelectEvent += MenuItem_SelectEvent;
                    }                    
                    else if (attribute is SpacerAttribute spacer)
                    {                        
                        AddSpacer();                        
                    }
                }
                enabled = false;
            }

            AddSpacer();
            MenuItemAction saveAndExitAction = AddMenuItemAction("Save and exit", enabled);
            saveAndExitAction.SelectEvent += SaveAndExitActionEvent_Select;

            MenuItemAction exitAction = AddMenuItemAction("Exit", enabled);
            exitAction.SelectEvent += ExitActionEvent_Select;


        }        

        protected virtual void ItemChanged()
        {
            if (!bloop.IsDisposed) bloop.Play();
        }

        private void MenuItem_SelectEvent(object sender, MenuItemChangedEventArgs e)
        {
            ItemChanged();
            MenuItemChanged(e);
        }

        private void SaveAndExitActionEvent_Select(object sender, EventArgs e)
        {            
            OnMenuClose(new MenuCloseEventArgs(myConfiguration,true));
        }

        private void ExitActionEvent_Select(object sender, EventArgs e)
        {
            OnMenuClose(new MenuCloseEventArgs(myConfiguration, false));
        }

        public void AddSpacer()
        {
            y+= font.LineSpacing;
        }
       

        public MenuItemMulti<T> AddMenuItem<T>(string name, T value, T[] values, Action<T> setter, Boolean enabled = false)
        {
            MenuItemMulti<T> item = new MenuItemMulti<T>(game, new Vector2(0, y),name, value, values,setter,font, wingdings);
            item.Initialize();
            item.ItemEnabled = enabled;
            items.Add(item);
            y += spacing;
            return item;
        }

        public MenuItemKey AddMenuItemKey(String name, Keys value,  Action<Keys> setter, Boolean enabled = false)
        {
            MenuItemKey item = new MenuItemKey(game, new Vector2(0, y), name, value, setter, font, wingdings);
            item.Initialize();
            item.ItemEnabled = enabled;
            items.Add(item);
            y += spacing;
            return item;
        }

        public MenuItemAction AddMenuItemAction(String name, Boolean enabled = false)
        {
            MenuItemAction item = new MenuItemAction(game, new Vector2(0, y), name, font, wingdings);
            item.Initialize();
            item.ItemEnabled = enabled;
            items.Add(item);
            y += spacing;
            return item;
        }


        public MenuItemVolume AddMenuItemVolume(String name, int value, Action<int> setter, Boolean enabled = false)
        {
            MenuItemVolume item = new MenuItemVolume(game, new Vector2(0, y), name, value, setter, font, wingdings);
            item.Initialize();
            item.ItemEnabled = enabled;
            items.Add(item);
            y += spacing;
            return item;
        }     

        public Boolean KeyPressed(Keys key, Boolean repeat = false)
        {
            return (current.IsKeyDown(key) && (last.IsKeyUp(key) || repeat));
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
