using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine
{
	public class TRef
	{
		public int TLocX;
		public int TLocY;
		public int tVal;

        // for the tiles position on the tilemap
        // usesd as a reference
		public TRef( int x, int y, int val)
		{            
			TLocX = x;
			TLocY = y;
			tVal = val;
		}

	}
}
