using System.Collections.Generic;
using UnityEngine;

namespace MazeAlgorithms
{
    public class RandomMazeGenerator
    {
        private static T RemoveRandomElement<T>(ref HashSet<T> data)
        {
            int ElementIndex = UnityEngine.Random.Range(0, data.Count);
            int i = 0;
            foreach (T element in data)
            {
                if (i == ElementIndex)
                {
                    data.Remove(element);
                    return element;
                }
                i++;
            }
            return default;
        }

        private static void Shuffle<T>(List<List<T>> data)
        {
            for (int i = 0; i < data.Count; i++)
            {
                int Index = UnityEngine.Random.Range(0, data.Count);
                if (Index == i)
                    continue;

                T X_temporary = data[Index][0];
                T Y_temporary = data[Index][1];

                data[Index][0] = data[i][0];
                data[Index][1] = data[i][1];

                data[i][0] = X_temporary;
                data[i][1] = Y_temporary;
            }
        }

        public List<List<bool>> GenerateMaze(int width, int length)
        {        
            List<List<bool>> Maze = new();
            for(int i = 0; i < length; i++)
            {
                Maze.Add(new());
                for(int j = 0; j < width; j++)
                {
                    Maze[i].Add(false);
                }
            }

            int X = UnityEngine.Random.Range(0, width / 2) * 2;
            int Y = UnityEngine.Random.Range(0, length / 2) * 2;

            HashSet<Vector2Int> ConnectPoints = new HashSet<Vector2Int>
            {
                new Vector2Int(X, Y)
            };

            while (ConnectPoints.Count > 0)
            {
                Vector2Int Point = RemoveRandomElement(ref ConnectPoints);

                X = Point.x;
                Y = Point.y;

                Maze[Y][X] = true;

                Connect(Maze, X, Y);
                AddVisitedPoints(Maze, ConnectPoints, X, Y);
            }

            return Maze;
        }

        private void Connect(List<List<bool>> maze, int x, int y)
        {
            List<List<int>> Directions = new ()
            {
                new() {-1, 0 },
                new() { 1, 0 },
                new() { 0, -1 },
                new() { 0, 1 }
            };

            Shuffle(Directions);

            for (int i = 0; i < Directions.Count; i++)
            {
                int NeighboringX = x + Directions[i][0] * 2;
                int NeighboringY = y + Directions[i][1] * 2;

                if (RoadExists(maze, NeighboringX, NeighboringY))
                {
                    int ConnectorX = x + Directions[i][0];
                    int ConnectorY = y + Directions[i][1];
                    maze[ConnectorY][ConnectorX] = true;

                    return;
                }
            }
        }

        private void AddVisitedPointIfItExists(List<List<bool>> maze, HashSet<Vector2Int> points, int x, int y)
        {
            if (IsPointInMaze(maze, x, y) && !RoadExists(maze, x, y))
            {
                points.Add(new(x, y));
            }
        }

        private void AddVisitedPoints(List<List<bool>> maze, HashSet<Vector2Int> points, int x, int y)
        {
            AddVisitedPointIfItExists(maze, points, x - 2, y);
            AddVisitedPointIfItExists(maze, points, x + 2, y);
            AddVisitedPointIfItExists(maze, points, x, y - 2);
            AddVisitedPointIfItExists(maze, points, x, y + 2);
        }


        private bool RoadExists(List<List<bool>> maze, int x, int y)
        {
            return IsPointInMaze(maze, x, y) && maze[y][x];
        }

        private bool IsPointInMaze(List<List<bool>> maze, int x, int y)
        {
            int length = maze.Count;
            int width = maze[0].Count;
            return x >= 0 && x < width && y >= 0 && y < length;
        }
    }
}