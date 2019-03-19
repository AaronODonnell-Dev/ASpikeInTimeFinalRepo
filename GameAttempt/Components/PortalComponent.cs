using Microsoft.Xna.Framework;
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
        TRef myframe;
        TManager myManager;
        Rectangle imageRect;
        Vector2 Position;
        Rectangle boundingRect;

        public override void Initialize()
        {
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

                    if (player.Collectables == 5)
                    {
                        if (boundingRect.Intersects(player.Bounds) && PortalOpen)
                        {
                            player.Collectables = 0;
                            trender.Collectables.Clear();
                            trender.Portal.Visible = false;

                            trender._current = TRender.LevelStates.LevelTwo;
                            trender.tileManager.ActiveLayer = trender.tileManager.GetLayer("LevelTwo");
                            trender.hasLevelChanged = true;
                            //trender.Portal.Visible = true;

                            // Creates a set of impassable tiles
                            trender.collisons.Clear(); // Important for removing colliders from screen
                            trender.tileManager.ActiveLayer.makeImpassable(trender.impassableTiles);
                            trender.SetupCollison();
                        }
                    }

                    break;

                case TRender.LevelStates.LevelTwo:

                    if (player.Collectables == 5)
                    {
                        if (boundingRect.Intersects(player.Bounds) && PortalOpen)
                        {
                            player.Collectables = 0;
                            trender.Collectables.Clear();

                            trender._current = TRender.LevelStates.LevelThree;
                            trender.tileManager.ActiveLayer = trender.tileManager.GetLayer("LevelThree");
                            trender.hasLevelChanged = true;

                            // Creates a set of impassable tiles
                            trender.collisons.Clear();
                            trender.tileManager.ActiveLayer.makeImpassable(trender.impassableTiles);
                            trender.SetupCollison();

                        }
                    }

                    break;

                case TRender.LevelStates.LevelThree:

                    if (player.Collectables == 5)
                    {
                        if (boundingRect.Intersects(player.Bounds) && PortalOpen)
                        {
                            player.Collectables = 0;
                            trender.Collectables.Clear();

                            trender._current = TRender.LevelStates.LevelFour;
                            trender.tileManager.ActiveLayer = trender.tileManager.GetLayer("LevelFour");
                            trender.hasLevelChanged = true;

                            // Creates a set of impassable tiles
                            trender.collisons.Clear();
                            trender.tileManager.ActiveLayer.makeImpassable(trender.impassableTiles);
                            trender.SetupCollison();
                        }
                    }

                    break;

                case TRender.LevelStates.LevelFour:

                    if (player.Collectables == 5)
                    {
                        if (boundingRect.Intersects(player.Bounds) && PortalOpen)
                        {
                            player.Collectables = 0;
                            trender.Collectables.Clear();

                            trender._current = TRender.LevelStates.LevelOne;
                            trender.tileManager.ActiveLayer = trender.tileManager.GetLayer("LevelOne");
                            trender.hasLevelChanged = true;

                            // Creates a set of impassable tiles
                            trender.collisons.Clear();
                            trender.tileManager.ActiveLayer.makeImpassable(trender.impassableTiles);
                            trender.SetupCollison();
                        }
                    }

                    break;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Game.Services.GetService<SpriteBatch>();
            TRender trender = Game.Services.GetService<TRender>();
            Camera Cam = Game.Services.GetService<Camera>();
            PlayerComponent player = Game.Services.GetService<PlayerComponent>();

            foreach (CollectableComponent collectable in trender.Collectables)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Cam.CurrentCamTranslation);
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
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
