using Components;
using GameAttempt.Components;
using Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using TileEngine;

namespace GameAttempt
{
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        ServiceManager serviceManager;
        TRender tiles;

        public Song Lvl1Song;
        public Song Lvl2Song;
        public Song Lvl3Song;
        public Song Lvl4Song;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 720;
            graphics.PreferredBackBufferWidth = 1280;
            graphics.ApplyChanges();

            new InputManager(this);
            serviceManager = new ServiceManager(this);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            ////Audio
            //Lvl1Song = Content.Load<Song>("Audio/dinoParkLvl1");
            //Lvl2Song = Content.Load<Song>("Audio/dinoParkLvl1");
            //Lvl3Song = Content.Load<Song>("Audio/dinoParkLvl1");
            //Lvl4Song = Content.Load<Song>("Audio/dinoParkLvl1");

            tiles = Services.GetService<TRender>();
            //if(tiles._current == TRender.LevelStates.LevelOne )
            //{
            //    tiles.BackgroundMusic = Lvl1Song;
            //}
            //else if(tiles._current == TRender.LevelStates.LevelTwo)
            //{
            //    tiles.BackgroundMusic = Lvl2Song;
            //}
            //else tiles.BackgroundMusic = Lvl1Song;

        }
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            //    Exit();


            //switch (tiles._current)
            //{
            //    case TRender.LevelStates.LevelOne:
            //        tiles.BackgroundMusic = Lvl1Song;
            //        break;

            //    case TRender.LevelStates.LevelTwo:
            //        tiles.BackgroundMusic = Lvl2Song;
            //        break;

            //    case TRender.LevelStates.LevelThree:
            //        tiles.BackgroundMusic = Lvl3Song;
            //        break;

            //    case TRender.LevelStates.LevelFour:
            //        tiles.BackgroundMusic = Lvl4Song;
            //        break;
            //}

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
