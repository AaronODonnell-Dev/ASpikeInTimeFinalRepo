using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TileEngine
{
	public class TLayer
	{
		#region Parameters
		List<TRef> refs;
		List<Tile> impassable = new List<Tile>();
		List<Tile> passable = new List<Tile>();
		Tile[,] tiles;
		string layerName;
		public int TileWidth;
		public int TileHeight;
		#endregion 
		#region GetSets
		public Tile[,] Tiles
		{
			get { return tiles; }
			set { tiles = value; }
		}
		public string LayerName
		{
			get { return layerName; }
			set { layerName = value; }
		}
		public int MapWidth
		{
			get { return Tiles.GetLength(1); }
		}
		public int MapHeight
		{
			get { return Tiles.GetLength(0); }
		}
		public List<TRef> TRefs
		{
			get { return refs; }
			set { refs = value; }
		}
		public List<Tile> Impassable
		{
			get { return impassable; }
			set { impassable = value; }
		}
		public List<Tile> Passable
		{
			get { return passable; }
			set { passable = value; }
		}
		#endregion
        // Returns the tile position that are in the passable list
		public Tile GetPassableTileAt(int X, int Y)
		{
			return passable.Where(t => t.X == X && t.Y == Y).Single();
		}
        // Returns the tile positions that are in the impassable list
		public Tile GetImpassableTileAt(int X, int Y)
		{
			return impassable.Where(t => t.X == X && t.Y == Y).Single();
		}
        // Checks if the tile has an adjacent tile
		public List<Tile> adjacentTo(Tile t)
		{
			List<Tile> adjacentTiles = new List<Tile>();
			if (valid("above", t))
				adjacentTiles.Add(GetadjacentTile("above", t));
			if (valid("below", t))
				adjacentTiles.Add(GetadjacentTile("below", t));
			if (valid("left", t))
				adjacentTiles.Add(GetadjacentTile("left", t));
			if (valid("right", t))
				adjacentTiles.Add(GetadjacentTile("right", t));

			return adjacentTiles;

		}
        // checks to see if there is an adjacent impassable tile
		public List<Tile> adjacentImpassible(Tile t)
		{
			List<Tile> adjacentTilesImpassible = new List<Tile>();
			if (valid("above", t))
			{
				Tile tile = GetadjacentTile("above", t);
				if (!tile.Passable) adjacentTilesImpassible.Add(tile);
			}
			if (valid("below", t))
			{
				Tile tile = GetadjacentTile("below", t);
				if (!tile.Passable) adjacentTilesImpassible.Add(tile);
			}
			if (valid("left", t))
			{
				Tile tile = GetadjacentTile("left", t);
				if (!tile.Passable) adjacentTilesImpassible.Add(tile);
			}
			if (valid("right", t))
			{
				Tile tile = GetadjacentTile("right", t);
				if (!tile.Passable) adjacentTilesImpassible.Add(tile);
			}

			return adjacentTilesImpassible;

		}

        // returns all tiles that are passably
		internal List<Tile> getSurroundingPassableTiles(Tile tile)
		{
			return fullAdjacentPassable(tile);
		}
        
        // adds all diagonal tiles to the list
		public List<Tile> fullAdjacentPassable(Tile t)
		{
			List<Tile> adj = new List<Tile>();
			adj = adjacentPassable(t);
			List<Tile> diag = new List<Tile>();
			diag = getDiagPassable(t);
			adj.AddRange(diag);
			return adj;
		}

        // finds all valid diagonal passable tiles
		private List<Tile> getDiagPassable(Tile t)
		{
			List<Tile> diag = new List<Tile>();
			if (valid("above_left", t))
			{
				Tile tile = GetadjacentTile("above_left", t);
				if (tile.Passable) diag.Add(tile);
			}
			if (valid("above_right", t))
			{
				Tile tile = GetadjacentTile("above_right", t);
				if (tile.Passable) diag.Add(tile);
			}
			if (valid("below_left", t))
			{
				Tile tile = GetadjacentTile("below_left", t);
				if (tile.Passable) diag.Add(tile);
			}
			if (valid("below_right", t))
			{
				Tile tile = GetadjacentTile("below_right", t);
				if (tile.Passable) diag.Add(tile);
			}
			return diag;

		}

        // gets all adjacent tiles that passable added to the list
		public List<Tile> adjacentPassable(Tile t)
		{
			List<Tile> adjacentTilesPassible = new List<Tile>();
			if (valid("above", t))
			{
				Tile tile = GetadjacentTile("above", t);
				if (tile.Passable) adjacentTilesPassible.Add(tile);
			}
			if (valid("below", t))
			{
				Tile tile = GetadjacentTile("below", t);
				if (tile.Passable) adjacentTilesPassible.Add(tile);
			}
			if (valid("left", t))
			{
				Tile tile = GetadjacentTile("left", t);
				if (tile.Passable) adjacentTilesPassible.Add(tile);
			}
			if (valid("right", t))
			{
				Tile tile = GetadjacentTile("right", t);
				if (tile.Passable) adjacentTilesPassible.Add(tile);
			}

			return adjacentTilesPassible;

		}
        
        // gets the adjacent tiles from the tile map
		public Tile GetadjacentTile(string direction, Tile t)
		{
			switch (direction)
			{
				case "above":
					if (t.Y - 1 >= 0)
						return Tiles[t.Y - 1, t.X];
					break;
				case "above_left":
					if (t.Y - 1 >= 0 && t.X - 1 > 0)
						return Tiles[t.Y - 1, t.X - 1];
					break;

				case "above_right":
					if (t.Y - 1 >= 0 && t.X + 1 < this.MapWidth)
						return Tiles[t.Y - 1, t.X + 1];
					break;

				case "below":
					if (t.Y + 1 < this.MapHeight)
						return Tiles[t.Y + 1, t.X];
					break;
				case "below_left":
					if (t.Y + 1 < this.MapHeight && t.X - 1 > 0)
						return Tiles[t.Y + 1, t.X - 1];
					break;
				case "below_right":
					if (t.Y + 1 < this.MapHeight && t.X + 1 < this.MapWidth)
						return Tiles[t.Y + 1, t.X + 1];
					break;

				case "left":
					if (t.X - 1 >= 0)
						return Tiles[t.Y, t.X - 1];
					break;
				case "right":
					if (t.X + 1 < this.MapWidth)
						return Tiles[t.Y, t.X + 1];
					break;
				default: return t;
			}
			return t;
		}

        // checks if there is no tiles in this position
		public bool valid(string direction, Tile t)
		{
            // if there is no tiles in the position
			if (t == null) return false;
			switch (direction)
			{
				case "above":
					if (t.Y - 1 >= 0)
						return true;
					break;

				case "above_left":
					if (t.Y - 1 >= 0 && t.X - 1 > 0)
						return true;
					break;

				case "above_right":
					if (t.Y - 1 >= 0 && t.X + 1 < this.MapWidth)
						return true;
					break;

				case "below":
					if (t.Y + 1 < this.MapHeight)
						return true;
					break;

				case "below_left":
					if (t.Y + 1 < this.MapHeight && t.X - 1 > 0)
						return true;
					break;
				case "below_right":
					if (t.Y + 1 < this.MapHeight && t.X + 1 < this.MapWidth)
						return true;
					break;

				case "left":
					if (t.X - 1 >= 0)
						return true;
					break;
				case "right":
					if (t.X + 1 < this.MapWidth)
						return true;
					break;
				default: return false;
			}
			return false;
		}

        // adds tiles from the impassable tile array to impassale tiles list
		public void makeImpassable(string[] TileNames)
		{
			foreach (Tile t in this.Tiles)
				if (TileNames.Contains(t.TileName))
				{
					impassable.Add(t);
					t.Passable = false;
				}
				else passable.Add(t);
		}

	}
}
