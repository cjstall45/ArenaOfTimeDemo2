using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace ArenaOfTimeDemo2.StateManagement
{
    public class TileMap
    {
        int _tileWidth, _tileHeight, _mapWidth, _mapHeight;
        public int player1Index = 0;
        public int player2Index = 0;

        Texture2D _tilesetTexture;
        Texture2D ninjaTile;

        Rectangle[] _tiles;

        Vector2[] _playerSelectTiles;

        int[] _map;

        string _filename;

        public TileMap(string filename)
        {
            _filename = filename;

        }

        public void LoadContent(ContentManager content)
        {
            string data = File.ReadAllText(Path.Join(content.RootDirectory, _filename));
            var lines = data.Split('\n');

            var tilesetFilename = lines[0].Trim();
            _tilesetTexture = content.Load<Texture2D>(tilesetFilename);
            ninjaTile = content.Load<Texture2D>("ninjaTile");

            var secondLine = lines[1].Split(',');
            _tileWidth = int.Parse(secondLine[0]);
            _tileHeight = int.Parse(secondLine[1]);

            int tilesetColumns = _tilesetTexture.Width / _tileWidth;
            int tilesetRows = _tilesetTexture.Height / _tileHeight;
            _tiles = new Rectangle[tilesetColumns * tilesetRows];

            for(int y = 0; y < tilesetRows; y++)
            {
                for(int x = 0; x < tilesetColumns; x++)
                {
                    int index = y * tilesetColumns + x;
                    _tiles[index] = new Rectangle(
                        x * _tileWidth, 
                        y * _tileHeight,
                        _tileWidth,
                        _tileHeight
                        );
                }
            }

            var thirdLine = lines[2].Split(',');
            _mapWidth = int.Parse(thirdLine[0]);
            _mapHeight = int.Parse(thirdLine[1]);

            var fourthLine = lines[3].Split(',');
            _map = new int[_mapWidth * _mapHeight];
            for(int i = 0; i < _mapWidth * _mapHeight; i++)
            {
                _map[i] = int.Parse(fourthLine[i]);
            }

            _playerSelectTiles = new Vector2[] { new Vector2(2 * _tileWidth, 4 * _tileHeight), new Vector2(5 * _tileWidth, 4 * _tileHeight), new Vector2(8 * _tileWidth, 4 * _tileHeight), new Vector2(11 * _tileWidth, 4 * _tileHeight) };
        }

        public void UpdatePosition(PlayerIndex player, int direction)
        {
            if(player == PlayerIndex.One && direction == 1)
            {
                if (player1Index < 3)
                {
                    player1Index++;
                }
                else
                {
                    player1Index = 0;
                }
            }
            if (player == PlayerIndex.One && direction == -1)
            {
                if (player1Index > 0)
                {
                    player1Index--;
                }
                else
                {
                    player1Index = 3;
                } 
            }
            if (player == PlayerIndex.Two && direction == 1)
            {
                if (player2Index < 3)
                {
                    player2Index++;
                }
                else
                {
                    player2Index = 0;
                }
            }
            if (player == PlayerIndex.Two && direction == -1)
            {
                if (player2Index > 0)
                {
                    player2Index--;
                }
                else
                {
                    player2Index = 3;
                }
            }
        }

        public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for(int y = 0; y < _mapHeight; y++)
            {
                for(int x = 0; x < _mapWidth; x++)
                {
                    int index = _map[y * _mapWidth + x] - 1;
                    if (index == -1) continue;
                    if (y == 4 && (x == 2 || x == 5 || x == 8 || x == 11))
                    {
                        if (player1Index == player2Index)
                        {
                            spriteBatch.Draw(
                                _tilesetTexture,
                                _playerSelectTiles[player1Index],
                                new Rectangle(0, 60, 60, 60),
                                Color.White
                            );
                        }
                        else
                        {
                            spriteBatch.Draw(
                                _tilesetTexture,
                                _playerSelectTiles[player1Index],
                                new Rectangle(120, 60, 60, 60),
                                Color.White
                            );
                            spriteBatch.Draw(
                                _tilesetTexture,
                                _playerSelectTiles[player2Index],
                                new Rectangle(60, 60, 60, 60),
                                Color.White
                            );
                        }
                    }
                    else if(y == 3 && x == 5)
                    {
                        spriteBatch.Draw(
                                ninjaTile, 
                                new Vector2(5 * _tileWidth, 3 * _tileHeight),
                                null,
                                Color.White
                            );
                    }
                    else
                    {
                        spriteBatch.Draw(
                            _tilesetTexture,
                            new Vector2(
                                x * _tileWidth,
                                y * _tileHeight
                                ),
                                _tiles[index],
                                Color.White
                            );
                    }
                }
            }
        }
    }
}
