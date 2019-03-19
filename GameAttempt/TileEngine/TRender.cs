
using GameAttempt.Components;
using Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TileEngine;

namespace GameAttempt
{

    public class TRender : DrawableGameComponent
    {
        #region Properties
        public TManager tileManager;
        public Texture2D tSheet;

        //References for Components
        public TRef Collectable;
        public TRef OpenPortal;
        public TRef ClosedPortal;
        public TRef SlugEnemy;
        public TRef BombBoy;
        public TRef Alien;

        Texture2D LevelOneBkGrnd;
        Texture2D LevelTwoBkGrnd;
        Texture2D LevelThreeBkGrnd;
        Texture2D LevelFourBkGrnd;
        public SpriteEffects effect;

        //List of Collectables
        public List<CollectableComponent> Collectables;
        //Portal
        public List<PortalComponent> Portal;

        //List of Enemies
        public List<EnemyComponent> enemies;

        //Audio
        public Song BackgroundMusic;
        Song Lvl1Song;
        Song Lvl2Song;
        Song Lvl3Song;
        Song Lvl4Song;
        public bool hasLevelChanged = false;

        //Level States
        public enum LevelStates { LevelOne, LevelTwo, LevelThree, LevelFour };
        public LevelStates _current;

        Vector2 ViewportCentre
        {
            get
            {
                return new Vector2(GraphicsDevice.Viewport.Width / 2,
                GraphicsDevice.Viewport.Height / 2);
            }
        }

        public List<Collider> collisons = new List<Collider>();
        List<TRef> tRefs = new List<TRef>();

        public int tsWidth;                     // gets the width of tSheet
        public int tsHeight;                       // gets teh height of tSheet

        public int tsRows = 15;                 // how many sprites in a column
        public int tsColumns = 11;                  // how many Sprites in a Row

        public int scale = 2;

        public string[] impassableTiles;

        public int[,] tileMap;
        public int[,] tileMap2;
        public int[,] tileMap3;
        public int[,] tileMap4;


        #endregion

        public TRender(Game game) : base(game)
        {
            tSheet = Game.Content.Load<Texture2D>
                  ("Sprites/TileSheet3");    // get TileSheet

            tsWidth = tSheet.Width / tsColumns;                 // gets Width of tiles
            tsHeight = tSheet.Height / tsRows;                  // gets Height of tiles

            _current = LevelStates.LevelOne;

            #region Tile Maps
            // Level 1
            tileMap = new int[,]
                {
                    {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
                    {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
                    {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
                    {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  2,  2,  2,  2,  2,  2,  2,  2, },
                    {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  14, 2,  2,  2,  2,  2,  2,  2,  2, },
                    {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  16, 1,  2,  2,  2,  2,  2,  2,  2,  2,  2, },
                    {   0, 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2, },
                    {   1,  1,  1,  1,  17, 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  9,  10, 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2, },
                    {   0,  0,  0,  0,  0,  0,  16, 1,  1,  17, 0,  0,  0,  0,  1,  1,  0,  0,  0,  0,  0,  13,  1,  0,  74, 0,  0,  0,  0,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2, },
                    {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  2,  1,  1,  12, 0,  0,  74, 2,  2,  2,  1,  1,  2,  2,  2,  2,  2, },
                    {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  2,  0,  2,  0,  0,  13, 1,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2, },
                    {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  2,  0,  2,  0,  0,  0,  2,  2,  2,  2,  2,  2,  1,  1,  2,  2,  2, },
                    {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  2,  0,  2,  2,  0,  0,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2, },
                    {   74, 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  2,  2,  0,  2,  2,  0,  0,  0,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2, },
                    {   77, 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  2,  2,  0,  2,  2,  0,  0,  0,  2,  2,  2,  2,  2,  2,  2,  2,  1,  1, },
                    {   80, 80, 0,  0,  0,  0,  0,  74, 74, 0,  0,  0,  0,  0,  0,  0,  74, 0,  0,  0,  0,  2,  2,  1,  1,  1,  1,  1,  1,  12,  2,  2,  2,  2,  2,  2,  2,  2,  2, },
                    {   2, 70,  0,  0,  0,  0,  0,  77, 77, 0,  0,  0,  0,  0,  0,  0,  77, 0,  0,  0,  0,  2,  1,  2,  2,  2,  2,  2,  2,  0,  2,  2,  2,  2,  1,  1,  1,  2,  2, },
                    {   1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  1,  2,  2,  2,  2,  2,  2,  2,  1,  1,  1,  1,  1,  2,  2,  2,  2,  2, },
                    {   2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2,  2, },

                };
            
            // level 2
            tileMap2 = new int[,]
                {
                    {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  },
                    {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  },
                    {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  3,  3,  3,  3,  3,  },
                    {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  },
                    {   0,  0,  0,  0,  0,  34, 31, 31, 33, 0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  },
                    {   0,  0,  0,  0,  0,  0,  0,  4,  0,  0,  0,  75, 0,  0,  0,  0,  0,  3,  3,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  },
                    {   0, 0,  0,  0,  0,  0,  0,  4,  0,  0,  26, 3,  25, 0,  0,  0,  26, 4,  4,  25, 0,  0,  0,  0,  0,  0,  29, 3,  3,  3,  0,  0,  0,  0,  0,  0,  0,  0,  0,  },
                    {   67, 0,  0,  0,  0,  0,  0,  4,  4,  0,  0,  4,  0,  0,  0,  0,  0,  4,  0,  0,  0,  0,  0,  0,  0,  0,  0,  4,  4,  4,  3,  3,  30,  0,  0,  0,  0,  29, 3,  },
                    {   3,  3,  30, 0,  0,  0,  0,  0,  4,  4,  0,  4,  0,  0,  0,  0,  0,  4,  0,  0,  0,  0,  0,  0,  0,  0,  0,  4,  4,  4,  4,  4,  0,  0,  0,  0,  0,  0,  0,  },
                    {   4,  4,  0,  0,  0,  0,  0,  0,  0,  4,  4,  4,  0,  0,  0,  0,  0,  4,  0,  0,  0,  0,  0,  0,  0,  0,  4,  4,  0,  0,  0,  4,  0,  0,  0,  0,  0,  75,  0,  },
                    {   4,  4,  0,  0,  0,  0,  0,  0,  0,  0,  4,  4,  0,  0,  0,  0,  0,  4,  0,  0,  0,  0,  0,  0,  0,  4,  4,  0,  0,  0,  0,  4,  0,  0,  0,  0,  0,  78,  0,  },
                    {   4,  4,  0,  0,  0,  0,  0,  0,  0,  0,  0,  4,  0,  0,  0,  0,  0,  4,  0,  0,  0,  0,  0,  0,  0,  4,  0,  0,  0,  0,  0,  4,  75,  0,  0,  0,  3,  3,  3,  },
                    {   4,  0,  75, 0,  0,  0,  0,  0,  0,  0,  0,  4,  0,  75, 0,  0,  0,  4,  0,  0,  0,  0,  0,  0,  0,  4,  0,  0,  0,  0,  0,  4,  3,  3,  4,  4,  4,  4,  4,  },
                    {   68, 0,  78, 78, 3,  3,  3,  0,  0,  0,  0,  4,  3,  25, 0,  0,  29, 3,  30, 0,  0,  0,  0,  0,  0,  3,  3,  3,  3,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  },
                    {   3,  3,  3,  3,  0,  0,  0,  0,  0,  0,  26, 3,  4,  0,  0,  0,  0,  4,  0,  0,  0,  0,  26, 3,  3,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  },
                    {   4,  4,  0,  0,  0,  0,  0,  0,  0,  0,  0,  4,  4,  0,  0,  0,  0,  4,  0,  0,  0,  0,  0,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  },
                    {   4,  4,  75, 0,  0,  0,  0,  0,  0,  0,  0,  4,  0,  0,  0,  0,  0,  4,  0,  0,  0,  0,  0,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  },
                    {   4,  4,  78, 75, 0,  0,  0,  0,  0,  0,  4,  4,  0,  0,  0,  0,  0,  4,  0,  0,  0,  0,  0,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  4,  },
                    {   3,  3,  3,  3,  3,  3,  3,  0,  0,  0,  3,  3,  3,  0,  0,  0,  29, 3, 30,  0,  0,  0,  0,  4,  4,  4,  4,  3,  3,  3,  3,  3,  3,  3,  3,  3,  3,  3,  3,  },

                };

            // level 3
            tileMap3 = new int[,]
                {
                    {	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,	0,	0,	0,	0,	0,	},
                    {	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,  0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	},
                    {	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,  0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	},
                    {	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	73,	0,	0,	0,	0,	0,  0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	},
                    {	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	76,	0,	0,	0,	0,	0,  0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	},
                    {	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,  35, 5,  36,  0,	0,	0,	0,  0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	},
                    {	0,	0,	0,	0,	0,	35,	5,	36,	0,	0,	0,	35,	5,	36,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,  0,	0,	0,	0,	0,	0,	0,	73,	0,	0,	0,	0,	0,	0,	0,	},
                    {	0,	73,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,  0,  0,	0,	0,	0,	0,	0,	0,	76,  73,  0,  0,  0,  0,  0,  0,	},
                    {	0,	76,	73,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,  0,  0,  0,  0,	0,	0,  0,  0,	0,	0,	0,	0,	0,	35,	5,  5,  5,  5,  5,  5,  5,  5,  },
                    {	0,	35,	5,	36,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	39,	5,	5,  5,  5,  0,	0,	0,	0,	0,	37,	7,  7,  7,  7,  7,  7,  7,  7,  7,  },
                    {	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	6,	6,	6,  6,  0,	0,	0,	39,	5,	7,	6,	6,  6,  6,  6,  6,  6,  6,  6,  },
                    {	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	35,	5,	36,	0,	0,	0,	0,	0,	6,	6,	6,  6,  0,	0,	0,	0,	7,	6,	6,	6,  6,  6,  6,  6,  6,  6,  6,  },
                    {	0,	0,	0,	0,	0,	35,	5,	36,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	6,	6,	6,  6,  0,	0,	0,	0,	6,	6,	6,	6,  6,  6,  6,  6,  6,  6,  6,  },
                    {	73,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	6,	6,	6,  6,  0,	0,	0,	0,	6,	6,	6,	6,  6,  6,  6,  6,  6,  6,  6,  },
                    {	76,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	0,	78,	6,  6,  6,  0,	0,	0,	0,	6,	6,	6,  6,  6,  6,  6,  6,  6,  6, 	6,	},
                    {	5,	5,	5,	0,	0,	0,	0,	73,	0,	0,	0,	0,	73,	0,	0,	0,	0,	0,	0,	39,	5,	5,	5,	5,  0,	0,	0,	0,	7,	7,	7,	7,	7,	7,	7,	7,	7,	7,	7,	},
                    {	6,	6,	6,	5,	5,	0,	0,	76,	0,	0,	0,	0,	76,	0,	0,	0,	0,	0,	0,	0,  6,	6,	6,	6,  0,	0,	0,	0,	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	},
                    {	6,	6,	6,	6,	6,	5,	5,	5,	5,	5,	5,	5,	5,	0,	0,	0,	0,	0,	0,	0,	6,	6,	6,	6,  0,	0,	0,	0,	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	},
                    {	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,  6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	6,	},
                };

            // level 4
            tileMap4 = new int[,]
                {
                        {   7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7,  7, },
                        {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
                        {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
                        {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
                        {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
                        {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
                        {   0,  0,  0,  0,  0, 52,  7, 51,  0,  0,  0,  0,  0,  52,  7, 51,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
                        {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
                        {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
                        {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
                        {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, 52,  7, 51,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
                        {   0,  0,  0,  0,  0,  52,  7, 51, 0,  0,  0,  0,  0, 52,  7, 51,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
                        {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
                        {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
                        {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  52,  7,  7, 51,  0,  0,  0,  0,  0,  0,  0,  0, 0,  0,  0,  0,  0,  0,  0, },
                        {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
                        {   0,  0,  0,  0,  0, 52,  7, 51,  0,  0,  0,  0,  0, 52,  7, 51,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
                        {   0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, },
                        {   7,  7,  7,  7,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0, 52,  7, 51,  0,  0,  0,  0,  0,  0,  0,  0,  0,  0,  7,  7,  7,  7,  7, },
                };

            #endregion

            game.Components.Add(this);

            tileManager = new TManager();

            Collectable = new TRef(128 * 8, 128, 64);
            ClosedPortal = new TRef(128 * 8, 128 * 2, 67);
            OpenPortal = new TRef(128 * 9, 128 * 2, 68);

            //Enemies
            SlugEnemy = new TRef(8 * 128, 0, 61);   // Slug Enemy
            Alien = new TRef(9 * 128, 0, 62);   // Alien Shooter
            BombBoy = new TRef(10 * 128, 0, 63);  // BombBoy
        }

        public override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            LevelOneBkGrnd = Game.Content.Load<Texture2D>("Sprites/DinoParkBackgroundFinal");
            LevelTwoBkGrnd = Game.Content.Load<Texture2D>("Sprites/IceAgeLevel2Final");
            LevelThreeBkGrnd = Game.Content.Load<Texture2D>("Sprites/FalloutLevel3Final");
            LevelFourBkGrnd = Game.Content.Load<Texture2D>("Sprites/MelvinsShipLevel4Final");
            Lvl1Song = Game.Content.Load<Song>("Audio/dinoParkLvl1Final");
            Lvl2Song = Game.Content.Load<Song>("Audio/IceAgeLvl2Final");
            Lvl3Song = Game.Content.Load<Song>("Audio/FalloutLvl3Final");
            Lvl4Song = Game.Content.Load<Song>("Audio/MelvinsMotherShipLvl4Final");

            #region Adding Tile References
            // create a new tile from the TileSheet in list (locX, locY, IndexNum)
            //Reused tile refs
            tRefs.Add(new TRef(0, 15, 0));   // blank space

            // Level 1 TRefs
            tRefs.Add(new TRef(0, 0, 1));    // Ground with grass
            tRefs.Add(new TRef(0, 1, 2));    // Ground no grass
            //Level 2 TRefs
            tRefs.Add(new TRef(0, 2, 3));    // Ground with ice
            tRefs.Add(new TRef(0, 3, 4));    // Ground no ice
            //Level 3 TRefs
            tRefs.Add(new TRef(0, 4, 5));    // Ground with yellow
            tRefs.Add(new TRef(0, 5, 6));    // Ground
            //Level 4 TRefs
            tRefs.Add(new TRef(0, 6, 7));    // Ground but in space
            tRefs.Add(new TRef(0, 15, 8));   // blank space

            tRefs.Add(new TRef(1, 0, 9));    // Rounded Ground Left
            tRefs.Add(new TRef(2, 0, 10));   // Rounded Ground Right 
            tRefs.Add(new TRef(3, 0, 11));   // Rounded Ground Single
            tRefs.Add(new TRef(4, 0, 12));   // Cliff edge Right
            tRefs.Add(new TRef(5, 0, 13));   // Cliff edge Left
            tRefs.Add(new TRef(6, 0, 14));   // Slant Left 
            tRefs.Add(new TRef(7, 0, 15));   // Slant Right
            tRefs.Add(new TRef(1, 1, 16));   // Curved Left 
            tRefs.Add(new TRef(2, 1, 17));   // Curved Right
            tRefs.Add(new TRef(3, 1, 18));   // Thin Square 
            tRefs.Add(new TRef(4, 1, 19));   // Thin Rounded
            tRefs.Add(new TRef(5, 1, 20));   // Thin Rounded Right 
            tRefs.Add(new TRef(6, 1, 21));   // Thin Rounded left          

           
            tRefs.Add(new TRef(1, 2, 22));   // Rounded Ground Left
            tRefs.Add(new TRef(2, 2, 23));   // Rounded Ground Right 
            tRefs.Add(new TRef(3, 2, 24));   // Rounded Ground Single
            tRefs.Add(new TRef(4, 2, 25));   // Cliff edge Right
            tRefs.Add(new TRef(5, 2, 26));   // Cliff edge Left
            tRefs.Add(new TRef(6, 2, 27));   // Slant Left 
            tRefs.Add(new TRef(7, 2, 28));   // Slant Right
            tRefs.Add(new TRef(1, 3, 29));   // Curved Left 
            tRefs.Add(new TRef(2, 3, 30));   // Curved Right
            tRefs.Add(new TRef(3, 3, 31));   // Thin Square 
            tRefs.Add(new TRef(4, 3, 32));   // Thin Rounded
            tRefs.Add(new TRef(5, 3, 33));   // Thin Rounded Right 
            tRefs.Add(new TRef(6, 3, 34));   // Thin Rounded left

          
            tRefs.Add(new TRef(1, 4, 35));   // Rounded Ground Left
            tRefs.Add(new TRef(2, 4, 36));   // Rounded Ground Right 
            tRefs.Add(new TRef(3, 4, 37));   // Rounded Ground Single
            tRefs.Add(new TRef(4, 4, 38));   // Cliff edge Right
            tRefs.Add(new TRef(5, 4, 39));   // Cliff edge Left
            tRefs.Add(new TRef(6, 4, 40));   // Slant Left 
            tRefs.Add(new TRef(7, 4, 41));   // Slant Right
            tRefs.Add(new TRef(1, 5, 42));   // Curved Left 
            tRefs.Add(new TRef(2, 5, 43));   // Curved Right
            tRefs.Add(new TRef(3, 5, 44));   // Thin Square 
            tRefs.Add(new TRef(4, 5, 45));   // Thin Rounded
            tRefs.Add(new TRef(5, 5, 46));   // Thin Rounded Right 
            tRefs.Add(new TRef(6, 5, 47));   // Thin Rounded left


            
            tRefs.Add(new TRef(1, 6, 48));   // Rounded Ground Left
            tRefs.Add(new TRef(2, 6, 49));   // Rounded Ground Right 
            tRefs.Add(new TRef(3, 6, 50));   // Rounded Ground Single
            tRefs.Add(new TRef(4, 6, 51));   // Cliff edge Right
            tRefs.Add(new TRef(5, 6, 52));   // Cliff edge Left
            tRefs.Add(new TRef(6, 6, 53));   // Slant Left 
            tRefs.Add(new TRef(7, 6, 54));   // Slant Right
            tRefs.Add(new TRef(1, 7, 55));   // Curved Left 
            tRefs.Add(new TRef(2, 7, 56));   // Curved Right
            tRefs.Add(new TRef(3, 7, 57));   // Thin Square 
            tRefs.Add(new TRef(4, 7, 58));   // Thin Rounded
            tRefs.Add(new TRef(5, 7, 59));   // Thin Rounded Right 
            tRefs.Add(new TRef(6, 7, 60));   // Thin Rounded left

            // Other Tiles
            tRefs.Add(SlugEnemy);   // Slug Enemy
            tRefs.Add(Alien);   // Alien Shooter
            tRefs.Add(BombBoy);  // BombBoy
            tRefs.Add(Collectable);   // Portal Gem
            tRefs.Add(new TRef(9, 1, 65));   // Buzz Saw
            tRefs.Add(new TRef(10, 1, 66));  // Spikes
            tRefs.Add(ClosedPortal);   // Portal Closed
            tRefs.Add(OpenPortal);   // Portal Opened
            tRefs.Add(new TRef(10, 2, 69));  // Heart
            tRefs.Add(new TRef(8, 3,  70));   // Car
            tRefs.Add(new TRef(9, 3,  71));   // Lamp Post
            tRefs.Add(new TRef(10, 3, 72));  // Ammo
            tRefs.Add(new TRef(8, 4,  73));   // Tree Top lvl3
            tRefs.Add(new TRef(9, 4,  74));   // Tree Top lvl1
            tRefs.Add(new TRef(10, 4, 75));  // Tree Top lvl2
            tRefs.Add(new TRef(8, 5,  76));   // Tree Bottom lvl3
            tRefs.Add(new TRef(9, 5,  77));   // Tree Bottom lvl1
            tRefs.Add(new TRef(10, 5, 78));  // Tree Bottom lvl2

            tRefs.Add(new TRef(0, 2, 79));  // Impassible Ground
            tRefs.Add(new TRef(0, 0, 80));  // Ground that can be walked through
            tRefs.Add(new TRef(8, 1, 81));



            #endregion
            // Names of all 78 Tiles
            string[] tNames = { "Empty",
                                "GroundLv1Top",
                                "GroundLv1",                               
                                "GroundLv2Top",
                                "GroundLv2",
                                "GroundLv3Top",
                                "GroundLv3",
                                "GroundLv4Top",
                                "GroundLv4",
                                "RoundedGroundLeft1",
                                "RoundedGroundRight1", 
                                "RoundedGroundSingle1",
                                "CliffedgeRight1",
                                "CliffedgeLeft1",
                                "SlantLeft1",
                                "SlantRight1",
                                "CurvedLeft1", 
                                "CurvedRight1",
                                "ThinSquare1",
                                "ThinRounded1",                               
                                "ThinRoundedRight1",
                                "ThinRoundedleft1",                                
                                "RoundedGroundLeft2",
                                "RoundedGroundRight2",
                                "RoundedGroundSingle2",
                                "CliffedgeRight2",
                                "CliffedgeLeft2",
                                "SlantLeft2",
                                "SlantRight2",
                                "CurvedLeft2",
                                "CurvedRight2",
                                "ThinSquare2",
                                "ThinRounded2",
                                "ThinRoundedRight2",
                                "ThinRoundedleft2",                                
                                "RoundedGroundLeft3",
                                "RoundedGroundRight3",
                                "RoundedGroundSingle3",
                                "CliffedgeRight3",
                                "CliffedgeLeft3",
                                "SlantLeft3",
                                "SlantRight3",
                                "CurvedLeft3",
                                "CurvedRight3",
                                "ThinSquare3",
                                "ThinRounded3",
                                "ThinRoundedRight3",
                                "ThinRoundedleft3",                                
                                "RoundedGroundLeft4",
                                "RoundedGroundRight4",
                                "RoundedGroundSingle4",
                                "CliffedgeRight4",
                                "CliffedgeLeft4",
                                "SlantLeft4",
                                "SlantRight4",
                                "CurvedLeft4",
                                "CurvedRight4",
                                "ThinSquare4",
                                "ThinRounded4",
                                "ThinRoundedRight4",
                                "ThinRoundedleft4",
                                "SlugEnemy" ,
                                "AlienShooter" ,
                                "BombBoy" ,
                                "PortalGem" ,
                                "BuzzSaw" ,
                                "Spikes" ,
                                "PortalClosed" ,
                                "PortalOpened" ,
                                "Heart" ,
                                "Car" ,
                                "LampPost" ,
                                "Ammo" ,
                                "TreeToplvl1" ,
                                "TreeToplvl2" ,
                                "TreeToplvl3" ,
                                "TreeBottomlvl1" ,
                                "TreeBottomlvl2" ,
                                "TreeBottomlvl3" ,
                                "ImpassableGround",
                                "WalkThroughGround",
                                "Collectable"
                                }; 

            impassableTiles = new string[] { "GroundLv1Top",
                                "GroundLv2Top",
                                "GroundLv3Top",
                                "GroundLv4Top",
                                "RoundedGroundLeft1",
                                "RoundedGroundRight1",
                                "RoundedGroundSingle1",
                                "CliffedgeRight1",
                                "CliffedgeLeft1",
                                "SlantLeft1",
                                "SlantRight1",
                                "CurvedLeft1",
                                "CurvedRight1",
                                "ThinSquare1",
                                "ThinRounded1",
                                "ThinRoundedRight1",
                                "ThinRoundedleft1",                                
                                "RoundedGroundLeft2",
                                "RoundedGroundRight2",
                                "RoundedGroundSingle2",
                                "CliffedgeRight2",
                                "CliffedgeLeft2",
                                "SlantLeft2",
                                "SlantRight2",
                                "CurvedLeft2",
                                "CurvedRight2",
                                "ThinSquare2",
                                "ThinRounded2",
                                "ThinRoundedRight2",
                                "ThinRoundedleft2",                               
                                "RoundedGroundLeft3",
                                "RoundedGroundRight3",
                                "RoundedGroundSingle3",
                                "CliffedgeRight3",
                                "CliffedgeLeft3",
                                "SlantLeft3",
                                "SlantRight3",
                                "CurvedLeft3",
                                "CurvedRight3",
                                "ThinSquare3",
                                "ThinRounded3",
                                "ThinRoundedRight3",
                                "ThinRoundedleft3",                                
                                "RoundedGroundLeft4",
                                "RoundedGroundRight4",
                                "RoundedGroundSingle4",
                                "CliffedgeRight4",
                                "CliffedgeLeft4",
                                "SlantLeft4",
                                "SlantRight4",
                                "CurvedLeft4",
                                "CurvedRight4",
                                "ThinSquare4",
                                "ThinRounded4",
                                "ThinRoundedRight4",
                                "ThinRoundedleft4",
                                "ImpassableGround" };


            // creates Layer for the first level
            tileManager.addLayer("LevelOne", tNames,
                                 tileMap, tRefs, tsWidth, tsHeight);

            tileManager.addLayer("LevelTwo", tNames,
                                 tileMap2, tRefs, tsWidth, tsHeight);

            tileManager.addLayer("LevelThree", tNames,
                                 tileMap3, tRefs, tsWidth, tsHeight);

            tileManager.addLayer("LevelFour", tNames,
                                 tileMap4, tRefs, tsWidth, tsHeight);

            // sets Ground as Active Layer
            tileManager.ActiveLayer = tileManager.GetLayer("LevelOne");

            // Creates a set of impassable tiles
            tileManager.ActiveLayer.makeImpassable(impassableTiles);

            // sets the current tile
            tileManager.CurrentTile = tileManager.ActiveLayer.Tiles[0, 0];

            //Sets Collison tiles
            SetupCollison();

            SetupComponents();

            base.LoadContent();
        }

        public void SetupCollison()
        {
            // Adds a collider to each tile allowing it to be collidable
            foreach (Tile t in tileManager.ActiveLayer.Impassable)
            {
                collisons.Add(new Collider(Game.Content.Load<Texture2D>("Sprites/Collison"),
                              new Vector2(t.X * t.TileWidth / 2, t.Y * t.TileHeight / 2),
                              new Vector2(t.TileWidth / 2, t.TileHeight / 2)));
            }

        }

        public void SetupComponents()
        {
            switch (_current)
            {
                case LevelStates.LevelOne:
                    Collectables = new List<CollectableComponent> {
                        new CollectableComponent (Game, tSheet, Collectable, tileManager, new Vector2(400, 800)),
                        new CollectableComponent (Game, tSheet, Collectable, tileManager, new Vector2(950, 780)),
                        new CollectableComponent (Game, tSheet, Collectable, tileManager, new Vector2(1250, 750)),
                        new CollectableComponent (Game, tSheet, Collectable, tileManager, new Vector2(1400, 120)),
                        new CollectableComponent (Game, tSheet, Collectable, tileManager, new Vector2(150, 300))
                        };

                    Portal = new List<PortalComponent> { new PortalComponent(Game, tSheet, ClosedPortal, tileManager, new Vector2(50, 320)) };

                    //enemies = new List<EnemyComponent>
                    //{
                    //    new EnemyComponent (Game, tSheet, SlugEnemy, tileManager, new Vector2(900, 960)),
                    //    new EnemyComponent (Game, tSheet, Alien, tileManager, new Vector2(15, 320)),
                    //    new EnemyComponent (Game, tSheet, BombBoy, tileManager, new Vector2(820, 960))
                    //};
                    break;

                case LevelStates.LevelTwo:
                    Collectables = new List<CollectableComponent> {
                        new CollectableComponent (Game, tSheet, Collectable, tileManager, new Vector2(400, 800)),
                        new CollectableComponent (Game, tSheet, Collectable, tileManager, new Vector2(950, 780)),
                        new CollectableComponent (Game, tSheet, Collectable, tileManager, new Vector2(1250, 750)),
                        new CollectableComponent (Game, tSheet, Collectable, tileManager, new Vector2(1400, 120)),
                        new CollectableComponent (Game, tSheet, Collectable, tileManager, new Vector2(1200, 300))
                        };

                    Portal = new List<PortalComponent> { new PortalComponent(Game, tSheet, ClosedPortal, tileManager, new Vector2(25, 370)) };
                    break;

                case LevelStates.LevelThree:
                    Collectables = new List<CollectableComponent> {
                        new CollectableComponent (Game, tSheet, Collectable, tileManager, new Vector2(400, 200)),
                        new CollectableComponent (Game, tSheet, Collectable, tileManager, new Vector2(950, 120)),
                        new CollectableComponent (Game, tSheet, Collectable, tileManager, new Vector2(1250, 150)),
                        new CollectableComponent (Game, tSheet, Collectable, tileManager, new Vector2(1400, 120)),
                        new CollectableComponent (Game, tSheet, Collectable, tileManager, new Vector2(350, 1200))
                        };

                    Portal = new List<PortalComponent> { new PortalComponent(Game, tSheet, ClosedPortal, tileManager, new Vector2(50, 320)) };
                    break;

                case LevelStates.LevelFour:
                    Collectables = new List<CollectableComponent> {
                        new CollectableComponent (Game, tSheet, Collectable, tileManager, new Vector2(400, 800)),
                        new CollectableComponent (Game, tSheet, Collectable, tileManager, new Vector2(950, 780)),
                        new CollectableComponent (Game, tSheet, Collectable, tileManager, new Vector2(1250, 750)),
                        new CollectableComponent (Game, tSheet, Collectable, tileManager, new Vector2(1400, 120)),
                        new CollectableComponent (Game, tSheet, Collectable, tileManager, new Vector2(150, 300))
                        };

                    Portal = new List<PortalComponent> { new PortalComponent(Game, tSheet, ClosedPortal, tileManager, new Vector2(50, 320)) };
                    break;
            }
        }

        public override void Update(GameTime gameTime)
        {
            PlayerComponent player = Game.Services.GetService<PlayerComponent>();

            // when the level is switched
            switch (_current)
            {
                // set up for level one
                case LevelStates.LevelOne:

                    player.Position = new Vector2(100, 700);
                    PlayAudio(Lvl1Song);
                    hasLevelChanged = false;

                    break;
                // set up for level 2
                case LevelStates.LevelTwo:

                    player.Position = new Vector2(100, 700);
                    PlayAudio(Lvl2Song);

                    hasLevelChanged = false;

                    break;
                // set up for level 3
                case LevelStates.LevelThree:


                    PlayAudio(Lvl3Song);

                    hasLevelChanged = false;

                    break;
                // set up for level 4
                case LevelStates.LevelFour:

                    PlayAudio(Lvl4Song);

                    hasLevelChanged = false;

                    break;
            }
            base.Update(gameTime);
        }
        //Audio code
        public void PlayAudio(Song levelSong)
        {
            if (MediaPlayer.State != MediaState.Playing /*&& !hasLevelChanged*/)
            {
                MediaPlayer.Play(levelSong);
                MediaPlayer.Volume = .5f;
                MediaPlayer.IsRepeating = true;
            }
            else if (MediaPlayer.State == MediaState.Playing && hasLevelChanged)
            {
                MediaPlayer.Stop();
            }
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch spriteBatch = Game.Services.GetService<SpriteBatch>();
            Camera Cam = Game.Services.GetService<Camera>();

            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, null, null, null, null, Cam.CurrentCamTranslation);

            #region Switch for Level States

            switch (_current)
            {
                // Draws the Level to the screen 
                case LevelStates.LevelOne:
                    spriteBatch.Draw(LevelOneBkGrnd, new Rectangle(Cam.View.X, Cam.View.Y, tsWidth * tileMap.GetLength(1) / 2,tileMap.GetLength(0) * 64), Color.White);
                    foreach (Tile t in tileManager.ActiveLayer.Tiles)
                    {
                        Vector2 position = new Vector2(t.X * t.TileWidth / 2,
                                                       t.Y * t.TileHeight / 2);

                        spriteBatch.Draw(tSheet, new Rectangle(position.ToPoint(),
                                                               new Point(t.TileWidth / 2,
                                                               t.TileHeight / 2)),


                                                 new Rectangle((t.TRefs.TLocX * t.TileWidth),
                                                               (t.TRefs.TLocY * t.TileHeight),
                                                               t.TileWidth,
                                                               t.TileHeight),
                                                 Color.White);

                    }

                    // Draws the collision image to the screen 
                    foreach (var item in collisons)
                        item.draw(spriteBatch);

                    break;
                 // Draws the 2th level to the screen
                case LevelStates.LevelTwo:
                    spriteBatch.Draw(LevelTwoBkGrnd, new Rectangle(Cam.View.X, Cam.View.Y, tsWidth * tileMap.GetLength(1) / 2, 64 * tileMap.GetLength(0)), Color.White);
                    foreach (Tile t in tileManager.ActiveLayer.Tiles)
                    {
                        Vector2 position = new Vector2(t.X * t.TileWidth / 2,
                                                       t.Y * t.TileHeight / 2);

                        spriteBatch.Draw(tSheet, new Rectangle(position.ToPoint(),
                                                               new Point(t.TileWidth / 2,
                                                               t.TileHeight / 2)),


                                                 new Rectangle((t.TRefs.TLocX * t.TileWidth),
                                                               (t.TRefs.TLocY * t.TileHeight),
                                                               t.TileWidth,
                                                               t.TileHeight),
                                                 Color.White);

                    }

                    // Draws the collision image to the screen 
                    foreach (var item in collisons)
                        item.draw(spriteBatch);

                    break;
                    // draws the third level to the screen 
                case LevelStates.LevelThree:
                    spriteBatch.Draw(LevelThreeBkGrnd, new Rectangle(Cam.View.X, Cam.View.Y, tsWidth * tileMap.GetLength(1) / 2, 64 * tileMap.GetLength(0)), Color.White);
                    foreach (Tile t in tileManager.ActiveLayer.Tiles)
                    {
                        Vector2 position = new Vector2(t.X * t.TileWidth / 2,
                                                       t.Y * t.TileHeight / 2);

                        spriteBatch.Draw(tSheet, new Rectangle(position.ToPoint(),
                                                               new Point(t.TileWidth / 2,
                                                               t.TileHeight / 2)),


                                                 new Rectangle((t.TRefs.TLocX * t.TileWidth),
                                                               (t.TRefs.TLocY * t.TileHeight),
                                                               t.TileWidth,
                                                               t.TileHeight),
                                                 Color.White);

                    }

                    // Draws the collision image to the screen 
                    foreach (var item in collisons)
                        item.draw(spriteBatch);

                    break;
                // Draws the 2th level to the screen
                case LevelStates.LevelFour:
                    spriteBatch.Draw(LevelFourBkGrnd, new Rectangle(Cam.View.X, Cam.View.Y, tsWidth * tileMap.GetLength(1) / 2, 64 * tileMap.GetLength(0)), Color.White);
                    foreach (Tile t in tileManager.ActiveLayer.Tiles)
                    {
                        Vector2 position = new Vector2(t.X * t.TileWidth / 2,
                                                       t.Y * t.TileHeight / 2);

                        spriteBatch.Draw(tSheet, new Rectangle(position.ToPoint(),
                                                               new Point(t.TileWidth / 2,
                                                               t.TileHeight / 2)),


                                                 new Rectangle((t.TRefs.TLocX * t.TileWidth),
                                                               (t.TRefs.TLocY * t.TileHeight),
                                                               t.TileWidth,
                                                               t.TileHeight),
                                                 Color.White);

                    }

                    // Draws the collision image to the screen 
                    foreach (var item in collisons)
                        item.draw(spriteBatch);

                    break;
            }

            #endregion

            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}
