using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameAttempt
{
	public class Camera
	{
		Vector2 camInitPos = Vector2.Zero;
		Vector2 worldBound;
		Viewport view;
        float scale = 1f;
		public float Scale
		{
			get  { return scale; }
			set { scale = value; }
		}
		public Vector2 CamInitPos
		{
			get { return camInitPos; }
			set { camInitPos = value; }
		}
		public Viewport View
		{
			get { return view; }
			set { view = value; }
		}
		public Vector2 WorldBound
		{
			get { return worldBound * scale; }
			set { worldBound = value; }
		}
		public Matrix CurrentCamTranslation 
		{ get
			{
                // how close the camera is to the player
				return Matrix.CreateTranslation(new Vector3(camInitPos, 0)) * 
					   Matrix.CreateScale(new Vector3(scale, scale, 0));
			}
		}

		public Camera(Vector2 startPos, Vector2 bounds, Viewport view)
		{
            // Sets Initial variables so they wont change
			CamInitPos = startPos;
			WorldBound = bounds;
			View = view;
		}

		public void FollowCharacter(Rectangle characterPos, Viewport v)
		{

            // Sets the players center  to the center of the screen
			CamInitPos = new Vector2(characterPos.X +(characterPos.Width /2), characterPos.Y +(characterPos.Height/2)) - new Vector2(v.Width / 2, v.Height / 2) / scale;
            // uses the players position as center sets its min scroll point to 0,0 then takes half the size of the map then centers the screen for the maximum scrollable point
            CamInitPos = -Vector2.Clamp(CamInitPos, Vector2.Zero, WorldBound / 2 - new Vector2(v.Width, v.Height) / scale);
		}
	}
}

