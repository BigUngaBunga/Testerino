using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace BelowUs
{
    public class MapGenerator : MonoBehaviour
    {
        [Min(50)]
        [SerializeField] protected int mapHeight;

        [Min(50)]
        [SerializeField] protected int mapWidth;

        [SerializeField] protected string seed;
        [SerializeReference] protected bool useRandomSeed, generateExit;

        [Range(3, 10)]
        [SerializeField] protected byte borderThickness;

        [Range(0, 5)]
        [SerializeField] protected byte passagewayRadius;

        protected int openWaterPercentage;
        protected int enclaveRemovalSize;
        protected const int waterTile = 1;
        protected const int wallTile = 0;
        protected int[,] noiseMap;
        protected Random random;
        public Vector2 ExitLocation { get; protected set; }
        public Vector2 MapSize => new Vector2(mapWidth, mapHeight);

        protected struct Coordinate
        {
            public int tileX;
            public int tileY;

            public Coordinate(int tileX, int tileY)
            {
                this.tileX = tileX;
                this.tileY = tileY;
            }
        }

        protected class Room : IComparable<Room>
        {
            public List<Coordinate> tiles;
            public List<Coordinate> edgeTiles;
            public List<Room> connectedRooms;
            public int tilesInRoom;
            public bool isMainRoom, isAccesibleFromMainRoom;

            public Room(List<Coordinate> tiles, int[,] map, int waterTile)
            {
                this.tiles = tiles;
                tilesInRoom = tiles.Count;
                connectedRooms = new List<Room>();
                edgeTiles = new List<Coordinate>();

                foreach (Coordinate tile in tiles)
                    for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
                        for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                            if (x == tile.tileX || y == tile.tileY && IsInmapRange(x, y, map) && map[x, y] == waterTile)
                                edgeTiles.Add(tile);
            }

            public static void ConnectRooms(Room roomA, Room roomB)
            {
                if (roomA.isAccesibleFromMainRoom)
                    roomB.SetAccesibleFromMainRoom();
                else if (roomB.isAccesibleFromMainRoom)
                    roomA.SetAccesibleFromMainRoom();

                roomA.connectedRooms.Add(roomB);
                roomB.connectedRooms.Add(roomA);
            }

            public bool IsConnected(Room otherRoom) => connectedRooms.Contains(otherRoom);

            public int CompareTo(Room other) => other.tilesInRoom.CompareTo(tilesInRoom);

            public void SetAccesibleFromMainRoom()
            {
                if (!isAccesibleFromMainRoom)
                {
                    isAccesibleFromMainRoom = true;
                    foreach (Room connectedRoom in connectedRooms)
                        connectedRoom.SetAccesibleFromMainRoom();
                }
            }

            private bool IsInmapRange(int tileX, int tileY, int[,] map) => tileX >= 0 && tileX < map.GetLength(0) && tileY >= 0 && tileY < map.GetLength(1);
        }

        public bool IsInMapRange(int tileX, int tileY) => tileX >= 0 && tileX < mapWidth && tileY >= 0 && tileY < mapHeight;

        protected WaitForSeconds Wait(string text = "") => CorutineUtilities.Wait(0.005f, text);

        protected void InitiateMap(Vector2 mapSize)
        {
            mapWidth = (int)mapSize.x;
            mapHeight = (int)mapSize.y;
            noiseMap = new int[mapWidth, mapHeight];
            RandomizeMapVariables();
            FillMapWithNoise();
        }

        protected virtual void RandomizeMapVariables()
        {
            if (useRandomSeed)
                seed = Environment.TickCount.ToString();
            random = new Random(seed.GetHashCode());
        }

        protected virtual void FillMapWithNoise()
        {
            for (int x = 0; x < mapWidth; x++)
                for (int y = 0; y < mapHeight; y++)
                    noiseMap[x, y] = (random.Next(0, 100) >= openWaterPercentage) ? wallTile : waterTile;
        }

        protected void CreateEntranceAndExit(bool randomExitPlacement = true)
        {
            int entranceSize = borderThickness - 2;
            int exitDistanceFromCorners = 2 + passagewayRadius;

            Vector2 entranceLocation = new Vector2(noiseMap.GetLength(0) / 2, noiseMap.GetLength(1) - 1);
            DrawCircle(entranceLocation, entranceSize);

            if (generateExit)
            {
                ExitLocation = randomExitPlacement
                    ? new Vector2(random.Next(exitDistanceFromCorners, noiseMap.GetLength(0) - exitDistanceFromCorners), 0)
                    : new Vector2(noiseMap.GetLength(0) / 2, 0);

                DrawCircle(ExitLocation, entranceSize);
            }

        }

        protected IEnumerator ClearPathways()
        {
            List<List<Coordinate>> waterTileRegions = GetRegion(waterTile);
            List<Room> rooms = new List<Room>();

            foreach (List<Coordinate> region in waterTileRegions)
                rooms.Add(new Room(region, noiseMap, waterTile));

            rooms.Sort();
            rooms[0].isMainRoom = true;
            rooms[0].isAccesibleFromMainRoom = true;

            yield return StartCoroutine(ConnectAllRooms(rooms));
        }

        protected List<List<Coordinate>> GetRegion(int tileType)
        {
            List<List<Coordinate>> regions = new List<List<Coordinate>>();
            int[,] flaggedTiles = new int[mapWidth, mapHeight];

            for (int x = 0; x < mapWidth; x++)
                for (int y = 0; y < mapHeight; y++)
                    if (flaggedTiles[x, y] == 0 && noiseMap[x, y] == tileType)
                    {
                        List<Coordinate> newRegion = GetRegionTiles(x, y);
                        regions.Add(newRegion);

                        foreach (Coordinate tile in newRegion)
                            flaggedTiles[tile.tileX, tile.tileY] = 1;
                    }

            return regions;
        }

        protected List<Coordinate> GetRegionTiles(int startX, int startY)
        {
            List<Coordinate> tiles = new List<Coordinate>();
            int[,] flaggedTiles = new int[mapWidth, mapHeight];
            int tileType = noiseMap[startX, startY];

            Queue<Coordinate> queue = new Queue<Coordinate>();
            queue.Enqueue(new Coordinate(startX, startY));
            flaggedTiles[startX, startY] = 1;

            while (queue.Count > 0)
            {
                Coordinate tile = queue.Dequeue();
                tiles.Add(tile);

                for (int x = tile.tileX - 1; x <= tile.tileX + 1; x++)
                    for (int y = tile.tileY - 1; y <= tile.tileY + 1; y++)
                        if (IsInMapRange(x, y) && (x == tile.tileX || y == tile.tileY) && flaggedTiles[x, y] == 0 && noiseMap[x, y] == tileType)
                        {
                            flaggedTiles[x, y] = 1;
                            queue.Enqueue(new Coordinate(x, y));
                        }
            }

            return tiles;
        }

        protected IEnumerator ConnectAllRooms(List<Room> rooms, bool forceAccessibilityFromMainRoom = false) //IEnumerator
        {
            List<Room> unconnectedRooms = new List<Room>();
            List<Room> connectedRooms = new List<Room>();

            if (forceAccessibilityFromMainRoom)
            {
                foreach (Room room in rooms)
                {
                    if (room.isAccesibleFromMainRoom)
                        connectedRooms.Add(room);
                    else
                        unconnectedRooms.Add(room);
                }
                yield return StartCoroutine(ConnectRooms(unconnectedRooms, connectedRooms));
            }
            else
                yield return StartCoroutine(ConnectRooms(rooms, rooms));

            if (!forceAccessibilityFromMainRoom)
                yield return StartCoroutine(ConnectAllRooms(rooms, true));
        }

        protected IEnumerator ConnectRooms(List<Room> roomsA, List<Room> roomsB)
        {
            bool possibleConnectionEstablished = false;
            bool isForcingStartAccesibility = !roomsA.Equals(roomsB);
            Room bestRoomA = null, bestRoomB = null;
            Coordinate bestTileA = new Coordinate(), bestTileB = new Coordinate();
            int closestDistance = 0;

            foreach (Room roomA in roomsA)
            {
                if (!isForcingStartAccesibility)
                {
                    possibleConnectionEstablished = false;
                    if (roomA.connectedRooms.Count > 0)
                        continue;
                }

                foreach (Room roomB in roomsB)
                {
                    if (roomA == roomB || roomA.IsConnected(roomB))
                        continue;

                    for (int tileIndexA = 0; tileIndexA < roomA.edgeTiles.Count; tileIndexA++)
                    {
                        for (int tileIndexB = 0; tileIndexB < roomB.edgeTiles.Count; tileIndexB++)
                        {
                            Coordinate tileA = roomA.edgeTiles[tileIndexA];
                            Coordinate tileB = roomB.edgeTiles[tileIndexB];
                            int distanceBetweenRooms = (int)(Mathf.Pow(tileA.tileX - tileB.tileX, 2) + Mathf.Pow(tileA.tileY - tileB.tileY, 2));

                            if (distanceBetweenRooms < closestDistance || !possibleConnectionEstablished)
                            {
                                closestDistance = distanceBetweenRooms;
                                possibleConnectionEstablished = true;
                                bestTileA = tileA;
                                bestTileB = tileB;
                                bestRoomB = roomA;
                                bestRoomA = roomB;
                            }
                        }

                        if (CorutineUtilities.WaitAmountOfTimes(tileIndexA, roomA.edgeTiles.Count, 10))
                            yield return Wait("Finding closest tiles");
                    }
                }
                if (possibleConnectionEstablished && !isForcingStartAccesibility)
                    CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
            }

            if (possibleConnectionEstablished && isForcingStartAccesibility)
            {
                CreatePassage(bestRoomA, bestRoomB, bestTileA, bestTileB);
                List<Room> rooms = roomsA.Union(roomsB).ToList();
                yield return StartCoroutine(ConnectAllRooms(rooms, true));
            }
        }

        protected void CreatePassage(Room roomA, Room roomB, Coordinate tileA, Coordinate tileB)
        {
            Room.ConnectRooms(roomA, roomB);
            List<Coordinate> line = GetLine(tileA, tileB);
            foreach (Coordinate point in line)
                DrawCircle(point, passagewayRadius);
        }

        protected void DrawCircle(Coordinate centre, int radius)
        {
            for (int x = -radius; x < radius; x++)
                for (int y = -radius; y < radius; y++)
                    if (x * x + y * y <= radius * radius)
                    {
                        int drawX = centre.tileX + x;
                        int drawY = centre.tileY + y;

                        if (IsInMapRange(drawX, drawY))
                            noiseMap[drawX, drawY] = waterTile;
                    }
        }

        protected void DrawCircle(Vector2 centre, int radius)
        {
            for (int x = -radius; x < radius; x++)
                for (int y = -radius; y < radius; y++)
                    if (x * x + y * y <= radius * radius)
                    {
                        int drawX = (int)centre.x + x;
                        int drawY = (int)centre.y + y;

                        if (IsInMapRange(drawX, drawY))
                            noiseMap[drawX, drawY] = waterTile;
                    }
        }

        protected List<Coordinate> GetLine(Coordinate from, Coordinate to)
        {
            List<Coordinate> line = new List<Coordinate>();
            int x = from.tileX;
            int y = from.tileY;

            int deltaX = to.tileX - from.tileX;
            int deltaY = to.tileY - from.tileY;

            int step = Math.Sign(deltaX);
            int gradientStep = Math.Sign(deltaY);

            bool inverted = false;
            int longest = Mathf.Abs(deltaX);
            int shortest = Mathf.Abs(deltaY);

            if (longest < shortest)
            {
                inverted = true;
                longest = Mathf.Abs(deltaY);
                shortest = Mathf.Abs(deltaX);
                step = Math.Sign(deltaY);
                gradientStep = Math.Sign(deltaX);
            }

            int gradientAccumulation = longest / 2;
            for (int i = 0; i < longest; i++)
            {
                line.Add(new Coordinate(x, y));

                if (inverted)
                    y += step;

                else
                    x += step;

                gradientAccumulation += shortest;
                if (gradientAccumulation >= longest)
                {
                    if (inverted)
                        x += gradientStep;
                    else
                        y += gradientStep;

                    gradientAccumulation -= longest;
                }
            }

            return line;
        }
    }
}
