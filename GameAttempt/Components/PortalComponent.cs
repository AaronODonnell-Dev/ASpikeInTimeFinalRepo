using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine;

namespace GameAttempt.Components
{
    public class PortalComponent : DrawableGameComponent
    {
        Texture2D texture;

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        bool PortalOpen = false;
        int GemsLeft;
        TRef myframe;
        TManager myManager;
        Rectangle imageRect;
        Vector2 Position;
        Rectangle boundingRect;
        SoundEffect PortalWhoosh;

        public override void Initialize()
        {
            PortalWhoosh = Game.Content.Load<SoundEffect>("Audio/PortalWhoosh");
            base.Initialize();
        }

        public PortalComponent(Game game, Texture2D tsheet, TRef Frame, TManager manager, Vector2 pos) : base(game)
        {
            texture = tsheet;
            Position = pos;
            myframe = Frame;
            myManager = manager;
            imageRect = new Rectangle(myframe.TLocX, myframe.TLocY, 128, 128);
            boundingRect = new Rectangle(Position.ToPoint(), imageRect.Size);
            game.Components.Add(this);
        }

        public override void Update(GameTime gametime)
        {
            //call player service and keep track of how many more gems are needed to pass throught to the next level
            PlayerComponent player = Game.Services.GetService<PlayerComponent>();
            GemsLeft = 5 - player.Collectables;


            //potential for animation

            Collision();
            base.Update(gametime);
        }

        public void Collision()
        {
            // Check for collisions with player
            PlayerComponent player = Game.Services.GetService<PlayerComponent>();
            TRender trender = Game.Services.GetService<TRender>();

            switch(trender._current)
            {
                case TRender.LevelStates.LevelOne:
                    //check if the player has all gems to pass through the portal
                    if (player.Collectables == 5)
                    {
                        //check if the player intersects with the open portal
                        if (boundingRect.Intersects(player.Bounds) && PortalOpen)
                        {
                            PortalWhoosh.Play();
                            Visible = false;
                            Enabled = false;
                            player.Collectables = 0;

                            //change the level state 
                            trender._current = TRender.LevelStates.LevelTwo;
                            trender.tileManager.ActiveLayer = trender.tileManager.GetLayer("LevelTwo");
                            trender.hasLevelChanged = true;

                            //clear all the variable lists
                            trender.Collectables.Clear();
                            trender.Portal.Clear();
                            player.ResetPlayer();
                            trender.enemies.Clear();

                            // Creates a set of impassable tiles
                            trender.collisons.Clear(); // Important for removing colliders from screen
                            trender.tileManager.ActiveLayer.makeImpassable(trender.impassableTiles);
                            trender.SetupCollison();
                            trender.SetupComponents();
                        }
                    }

                    break;

                case TRender.LevelStates.LevelTwo:

                    if (player.Collectables == 5)
                    {
                        if (boundingRect.Intersects(player.Bounds) && PortalOpen)
                        {
                            Visible = false;
                            Enabled = false;
                            player.Collectables = 0;

                            trender._current = TRender.LevelStates.LevelThree;
                            trender.tileManager.ActiveLayer = trender.tileManager.GetLayer("LevelThree");
                            trender.hasLevelChanged = true;

                            trender.collisons.Clear();
                            trender.Collectables.Clear();
                            trender.Portal.Clear();
                            trender.enemies.Clear();
                            player.ResetPlayer();

                            trender.tileManager.ActiveLayer.makeImpassable(trender.impassableTiles);
                            trender.SetupCollison();
                            trender.SetupComponents();
                        }
                    }

                    break;

                case TRender.LevelStates.LevelThree:

                    if (player.Collectables == 5)
                    {
                        if (boundingRect.Intersects(player.Bounds) && PortalOpen)
                        {
                            Visible = false;
                            Enabled = false;
                            player.Collectables = 0;

                            trender._current = TRender.LevelStates.LevelFour;
                            trender.tileManager.ActiveLayer = trender.tileManager.GetLayer("LevelFour");
                            trender.hasLevelChanged = true;

                            trender.collisons.Clear();
                            trender.Collectables.Clear();

                            player.ResetPlayer();

                            trender.Portal.Clear();
                            trender.tileManager.ActiveLayer.makeImpassable(trender.impassableTiles);
                            trender.SetupCollison();
                            trender.SetupComponents();
                        }
                    }

                    break;

                case TRender.LevelStates.LevelFour:

                    if (player.Collectables == 5)
                    {
                        if (boundingRect.Intersects(player.Bounds) && PortalOpen)
                        {
                            Visible = false;
                            Enabled = false;
                            player.Collectables = 0;

                            trender._current = TRender.LevelStates.LevelOne;
                            trender.tileManager.ActiveLayer = trender.tileManager.GetLayer("LevelOne");
                            trender.hasLevelChanged = true;

                            trender.collisons.Clear();
                            trender.Collectables.Clear();
                            player.ResetPlayer();

                            trender.Portal.Clear();
                            trender.tileManager.ActiveLayer.makeImpassable(trender.impassableTiles);
                            trender.SetupCollison();
                            trender.SetupComponents();
                        }
                    }

                    break;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            //call back all the services
            SpriteBatch spriteBatch = Game.Services.GetService<SpriteBatch>();
            TRender trender = Game.Services.GetService<TRender>();
            Camera Cam = Game.Services.GetService<Camera>();
            PlayerComponent player = Game.Services.GetService<PlayerComponent>();
            SpriteFont font = Game.Services.GetService<SpriteFont>();

            //draw onto the camera
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Cam.CurrentCamTranslation);
            foreach (PortalComponent portal in trender.Portal)
            {
                if(portal.Visible)
                {
                    if (player.Collectables == 5)
                    {
                        spriteBatch.Draw(Texture, boundingRect, new Rectangle(trender.OpenPortal.TLocX, trender.OpenPortal.TLocY, 128, 128), Color.White);
                        PortalOpen = true;
                    }
                    else
                    {
                        spriteBatch.Draw(Texture, boundingRect, imageRect, Color.White);
                        PortalOpen = false;
                    }
                }
            }
            //inform player of how many gems are left
            spriteBatch.DrawString(font, "You need " + GemsLeft.ToString() + " more Portal Gems to open this door!", new Vector2(Position.X, Position.Y - 40), Color.Black);
            spriteBatch.DrawString(font, "Portal Gems : 0" + player.Collectables.ToString(), new Vector2(Position.X, Position.Y - 60), Color.Black);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
