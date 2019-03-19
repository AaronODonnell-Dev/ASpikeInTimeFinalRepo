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
        TRef myframe;
        TManager myManager;
        Rectangle imageRect;
        private Vector2 Position;

        public CollectableComponent(Game game, TRef Frame,TManager manager, Vector2 pos) : base(game)
        {
            
            myframe = Frame;
            myManager = manager;
            imageRect = new Rectangle(myframe.TLocX * 128, myframe.TLocY * 128, 128, 128);
            Position = pos;
            game.Components.Add(this);
        }
        public override void Update(GameTime gametime)
        {        
            
            // Check for collisions with player
            PlayerComponent player = Game.Services.GetService<PlayerComponent>();
            //if ()
            //{
            //    Enabled = false;
            //    Visible = false;
            //}
            base.Update(gametime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Game.Services.GetService<SpriteBatch>();
            TRender render = Game.Services.GetService<TRender>();
            spriteBatch.Begin();

            spriteBatch.Draw(render.tSheet, new Vector2(myframe.TLocX, myframe.TLocY), imageRect, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
