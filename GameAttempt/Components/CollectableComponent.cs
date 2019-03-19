using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TileEngine;

namespace GameAttempt.Components
{
    public class CollectableComponent : DrawableGameComponent
    {
        Texture2D texture;

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        TRef myframe;
        TManager myManager;
        Rectangle imageRect;
        Vector2 Position;
        Rectangle boundingRect;

        public override void Initialize()
        {
            base.Initialize();
        }

        public CollectableComponent(Game game, Texture2D tsheet, TRef Frame,TManager manager, Vector2 pos) : base(game)
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

            foreach (CollectableComponent collectable in trender.Collectables)
            {
                if (boundingRect.Intersects(player.Bounds))
                {
                    player.Collectables++;
                    Enabled = false;
                    Visible = false;
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Game.Services.GetService<SpriteBatch>();
            TRender trender = Game.Services.GetService<TRender>();
            Camera Cam = Game.Services.GetService<Camera>();

            foreach (CollectableComponent collectable in trender.Collectables)
            {
                spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Cam.CurrentCamTranslation);
                if (Visible)
                {
                    spriteBatch.Draw(Texture, boundingRect, imageRect, Color.White);
                }
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}
