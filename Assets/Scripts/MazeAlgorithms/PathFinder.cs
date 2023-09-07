using System.Collections.Generic;

namespace MazeAlgorithms
{
    public class PathFinder
    {
        public List<KeyValuePair<int, int>> FindPathBetweenTwoPoints(List<List<bool>> maze, KeyValuePair<int, int> startPoint, KeyValuePair<int, int> finishPoint)
        {
            List<List<KeyValuePair<int, int>>> PossiblePathsToFinish = new() { new() {startPoint} };
            while(true)
            {
                for(int i = PossiblePathsToFinish.Count - 1; i >= 0; i--)
                {
                    KeyValuePair<int, int> LastPoint = PossiblePathsToFinish[i][PossiblePathsToFinish[i].Count - 1];
                    List<KeyValuePair<int, int>> PossibleNewSteps = new()
                    {
                        new(LastPoint.Key + 1, LastPoint.Value),
                        new(LastPoint.Key - 1, LastPoint.Value),
                        new(LastPoint.Key, LastPoint.Value + 1),
                        new(LastPoint.Key, LastPoint.Value - 1)
                    };
                    List<List<KeyValuePair<int, int>>> NewPossiblePaths = new();
                    foreach(KeyValuePair<int, int> possibleNewStep in PossibleNewSteps)
                    {
                        if(maze[possibleNewStep.Key][possibleNewStep.Value] && !PossiblePathsToFinish[i].Contains(possibleNewStep))
                        {
                            List<KeyValuePair<int, int>> NewPossiblePath = new();
                            foreach(KeyValuePair<int, int> point in PossiblePathsToFinish[i])
                            {
                                NewPossiblePath.Add(point);
                            }
                            NewPossiblePath.Add(possibleNewStep);
                            if(possibleNewStep.Key == finishPoint.Key && possibleNewStep.Value == finishPoint.Value)
                            {
                                return NewPossiblePath;
                            }
                            NewPossiblePaths.Add(NewPossiblePath);
                        }
                    }
                    PossiblePathsToFinish.RemoveAt(i);
                    foreach(List<KeyValuePair<int, int>> newPossiblePath in NewPossiblePaths)
                    {
                        PossiblePathsToFinish.Add(newPossiblePath);
                    }
                }
            }
        }
    }
}