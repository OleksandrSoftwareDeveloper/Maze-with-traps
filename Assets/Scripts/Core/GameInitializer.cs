using UnityEngine;
using System.Collections.Generic;
using MazeAlgorithms;

public class GameInitializer : MonoBehaviour
{
    [SerializeField] private int m_Width;
    [SerializeField] private int m_Length;
    [SerializeField] private float m_OneBlockWidthInTheScene = 1;
    [SerializeField] private GameObject m_SampleBlock;
    [SerializeField] private GameObject m_ParentObjectForMapSymbols;
    [SerializeField] private GameObject m_SampleBlockSymbolForMap;
    [SerializeField] private int m_QuantityOfEnemiesOnThePathToTarget = 1;
    [SerializeField] private int m_QuantityOfEnemiesInRandomPlaces = 1;
    [SerializeField] private GameObject m_SampleEnemy;
    [SerializeField] private Vector3 m_EnemyOffsetPosition;
    [SerializeField] private GameObject m_SampleDangerSymbol;
    [SerializeField] private GameObject m_PlayerSymbolForMap;
    [SerializeField] private GameObject m_SamplePathFieldSymbol;
    [SerializeField] private GameObject m_SampleTargetSymbolForMap;
    [SerializeField] private GameObject m_SamplePlayer;
    [SerializeField] private GameObject m_SampleTarget;
    [SerializeField] private Vector3 m_LeftBackCornerPositionInTheScene;

    private List<List<bool>> m_TheMaze;
    private Vector2 m_TargetPositionInTheMaze = Vector2.zero;
    private Vector2 m_LeftBackCornerPositionOnTheMap;
    private float m_OneSymbolWidthOnTheMap = 10;
    private Vector3 m_LeftBackCornerOfLeftBackCornerBlockPosition;
    private Vector3 m_RightForwardCornerOfRightForwardCornerBlockPosition;
    private Vector2 m_LeftBackCornerOfLeftBackCornerBlockSymbolPosition;
    private Vector2 m_RightForwardCornerOfRightForwardCornerBlockSymbolPosition;
    private Transform m_PlayerTransform;
    private List<GameObject> m_AllPathSymbolsOnTheMap = new();

    private void Start()
    {
        Time.timeScale = 1;

        RectTransform RectTransformOfBlockSymbol = m_SampleBlockSymbolForMap.GetComponent<RectTransform>();
        m_OneSymbolWidthOnTheMap = RectTransformOfBlockSymbol.sizeDelta.x / RectTransformOfBlockSymbol.localScale.x;
        m_LeftBackCornerPositionOnTheMap = RectTransformOfBlockSymbol.anchoredPosition;

        m_LeftBackCornerOfLeftBackCornerBlockPosition = m_LeftBackCornerPositionInTheScene - new Vector3(m_OneBlockWidthInTheScene, 0, m_OneBlockWidthInTheScene) / 2;
        m_RightForwardCornerOfRightForwardCornerBlockPosition = m_LeftBackCornerOfLeftBackCornerBlockPosition + new Vector3(m_OneBlockWidthInTheScene * m_Width, 0, m_OneBlockWidthInTheScene * m_Length);
        m_LeftBackCornerOfLeftBackCornerBlockSymbolPosition = m_LeftBackCornerPositionOnTheMap - (RectTransformOfBlockSymbol.sizeDelta.x / RectTransformOfBlockSymbol.localScale.x) * Vector2.one * 0.5f;
        m_RightForwardCornerOfRightForwardCornerBlockSymbolPosition = m_LeftBackCornerPositionOnTheMap + new Vector2((RectTransformOfBlockSymbol.sizeDelta.x / RectTransformOfBlockSymbol.localScale.x) * (m_Width - 0.5f), (RectTransformOfBlockSymbol.sizeDelta.x / RectTransformOfBlockSymbol.localScale.x) * (m_Length - 0.5f));

        m_TheMaze = new RandomMazeGenerator().GenerateMaze(m_Width - 2, m_Length - 2);

        m_TheMaze.Insert(0, new());
        m_TheMaze.Add(new());
        for(int i = 0; i < m_Width; i++)
        {
            m_TheMaze[0].Add(false);
            m_TheMaze[m_TheMaze.Count - 1].Add(false);
        }
        for(int i = 0; i < m_Length; i++)
        {
            m_TheMaze[i].Insert(0, false);
            m_TheMaze[i].Add(false);
        }

        m_TargetPositionInTheMaze = Vector2.zero;
        Vector2 PlayerPosition = Vector2.zero;
        bool WasPlayerPositionSet = false;
        for(int i = 0; i < m_Length; i++)
        {
            for(int j = 0; j < m_Width; j++)
            {
                if(m_TheMaze[i][j])
                {
                    if(!WasPlayerPositionSet)
                    {
                        PlayerPosition = new (i, j);
                        WasPlayerPositionSet = true;
                    }
                    m_TargetPositionInTheMaze = new (i, j);
                }
            }
        }

        KeyValuePair<int, int> PlayerPositionAsKeyValuePair = new((int)PlayerPosition.x, (int)PlayerPosition.y);
        KeyValuePair<int, int> TargetPositionAsKeyValuePair = new((int)m_TargetPositionInTheMaze.x, (int)m_TargetPositionInTheMaze.y);
        List<KeyValuePair<int, int>> PathFromPlayerToTarget = new PathFinder().FindPathBetweenTwoPoints(m_TheMaze, PlayerPositionAsKeyValuePair, TargetPositionAsKeyValuePair);

        List<KeyValuePair<int, int>> EnemyPositions = new();
        for(int i = 1; i <= m_QuantityOfEnemiesOnThePathToTarget; i++)
        {
            EnemyPositions.Add(PathFromPlayerToTarget[PathFromPlayerToTarget.Count / (m_QuantityOfEnemiesOnThePathToTarget + 1) * i]);
        }
        List<KeyValuePair<int, int>> AllEmptyPositions = new();
        for (int i = 0; i < m_Length; i++)
        {
            for (int j = 0; j < m_Width; j++)
            {
                if (m_TheMaze[i][j] && !(PlayerPosition.x == i && PlayerPosition.y == j) && !(m_TargetPositionInTheMaze.x == i && m_TargetPositionInTheMaze.y == j) && !EnemyPositions.Contains(new(i, j)))
                {
                    AllEmptyPositions.Add(new(i, j));
                }
            }
        }
        for (int i = 1; i <= m_QuantityOfEnemiesInRandomPlaces; i++)
        {
            KeyValuePair<int, int> NewEnemyPosition = AllEmptyPositions[new System.Random().Next(0, AllEmptyPositions.Count)];
            EnemyPositions.Add(NewEnemyPosition);
            AllEmptyPositions.Remove(NewEnemyPosition);
        }

        m_ParentObjectForMapSymbols.SetActive(true);
        Vector3 CurrentPositionInTheScene = m_LeftBackCornerPositionInTheScene;
        Vector3 CurrentPositionOnTheMap = m_LeftBackCornerPositionOnTheMap;
        for(int i = 0; i < m_Length; i++)
        {
            for(int j = 0; j < m_Width; j++)
            {
                if(!m_TheMaze[i][j])
                {
                    InstatiateObjectAtPosition(m_SampleBlock, CurrentPositionInTheScene);
                    PutSymbolToTheMap(m_SampleBlockSymbolForMap, CurrentPositionOnTheMap);
                }
                else if(PlayerPosition.x == i && PlayerPosition.y == j)
                {
                    m_PlayerTransform = InstatiateObjectAtPosition(m_SamplePlayer, CurrentPositionInTheScene).transform;
                }
                else if(m_TargetPositionInTheMaze.x == i && m_TargetPositionInTheMaze.y == j)
                {
                    InstatiateObjectAtPosition(m_SampleTarget, new(CurrentPositionInTheScene.x, m_SampleTarget.transform.position.y, CurrentPositionInTheScene.z));
                    PutSymbolToTheMap(m_SampleTargetSymbolForMap, CurrentPositionOnTheMap);
                }
                if(EnemyPositions.Contains(new(i, j)))
                {
                    InstatiateObjectAtPosition(m_SampleEnemy, CurrentPositionInTheScene + m_EnemyOffsetPosition);
                    PutSymbolToTheMap(m_SampleDangerSymbol, CurrentPositionOnTheMap);
                }
                CurrentPositionInTheScene.z += m_OneBlockWidthInTheScene;
                CurrentPositionOnTheMap.y += m_OneSymbolWidthOnTheMap;
            }
            CurrentPositionInTheScene.x += m_OneBlockWidthInTheScene;
            CurrentPositionOnTheMap.x += m_OneSymbolWidthOnTheMap;
            CurrentPositionInTheScene.z = m_LeftBackCornerPositionInTheScene.z;
            CurrentPositionOnTheMap.y = m_LeftBackCornerPositionOnTheMap.y;
        }
        m_ParentObjectForMapSymbols.SetActive(false);
    }

    private GameObject InstatiateObjectAtPosition(GameObject sample, Vector3 position)
    {
        GameObject NewGameObject = Instantiate(sample);
        NewGameObject.transform.position = position;
        return NewGameObject;
    }

    private GameObject PutSymbolToTheMap(GameObject sample, Vector2 anchoredPosition)
    {
        GameObject NewSymbol = Instantiate(sample);
        RectTransform RectTransformOfNewSymbol = NewSymbol.GetComponent<RectTransform>();
        RectTransformOfNewSymbol.SetParent(m_ParentObjectForMapSymbols.transform);
        RectTransformOfNewSymbol.anchoredPosition = anchoredPosition;
        RectTransformOfNewSymbol.sizeDelta /= RectTransformOfNewSymbol.localScale.x;
        return NewSymbol;
    }

    private void RecalculatePlayerPositionRelativeToMazeSize(out float ratioBetweenDistanceFromPlayerToCornerAndMazeWidth, out float ratioBetweenDistanceFromPlayerToCornerAndMazeLength)
    {
        ratioBetweenDistanceFromPlayerToCornerAndMazeWidth =
        (m_PlayerTransform.transform.position.x - m_LeftBackCornerOfLeftBackCornerBlockPosition.x)
        / (m_RightForwardCornerOfRightForwardCornerBlockPosition.x - m_LeftBackCornerOfLeftBackCornerBlockPosition.x);

        ratioBetweenDistanceFromPlayerToCornerAndMazeLength =
        (m_PlayerTransform.transform.position.z - m_LeftBackCornerOfLeftBackCornerBlockPosition.z)
        / (m_RightForwardCornerOfRightForwardCornerBlockPosition.z - m_LeftBackCornerOfLeftBackCornerBlockPosition.z);
    }

    public void PrintPlayerPositionOnTheMap()
    {
        RecalculatePlayerPositionRelativeToMazeSize(out float RatioBetweenDistanceFromPlayerToCornerAndMazeWidth, out float RatioBetweenDistanceFromPlayerToCornerAndMazeLength);

        Vector2 NewPositionOfPlayerSymbolOnTheMap =
        new((m_LeftBackCornerOfLeftBackCornerBlockSymbolPosition.x + (m_RightForwardCornerOfRightForwardCornerBlockSymbolPosition.x - m_LeftBackCornerOfLeftBackCornerBlockSymbolPosition.x) * RatioBetweenDistanceFromPlayerToCornerAndMazeWidth),
        m_LeftBackCornerOfLeftBackCornerBlockSymbolPosition.y + (m_RightForwardCornerOfRightForwardCornerBlockSymbolPosition.y - m_LeftBackCornerOfLeftBackCornerBlockSymbolPosition.y) * RatioBetweenDistanceFromPlayerToCornerAndMazeLength);

        m_PlayerSymbolForMap.GetComponent<RectTransform>().anchoredPosition = NewPositionOfPlayerSymbolOnTheMap;
    }

    public void FindAndShowPathFromPlayerToTarget()
    {
        if (m_AllPathSymbolsOnTheMap.Count == 0)
        {
            RecalculatePlayerPositionRelativeToMazeSize(out float RatioBetweenDistanceFromPlayerToCornerAndMazeWidth, out float RatioBetweenDistanceFromPlayerToCornerAndMazeLength);

            int AbscissaOfCurrentPlayerPositionInTheMaze = (int)(m_Width * RatioBetweenDistanceFromPlayerToCornerAndMazeWidth);
            int OrdinateOfCurrentPlayerPositionInTheMaze = (int)(m_Length * RatioBetweenDistanceFromPlayerToCornerAndMazeLength);

            List<KeyValuePair<int, int>> PathFromPlayerToTarget = new PathFinder().FindPathBetweenTwoPoints(m_TheMaze, new(AbscissaOfCurrentPlayerPositionInTheMaze, OrdinateOfCurrentPlayerPositionInTheMaze), new((int)m_TargetPositionInTheMaze.x, (int)m_TargetPositionInTheMaze.y));

            for (int i = 0; i < PathFromPlayerToTarget.Count; i++)
            {
                m_AllPathSymbolsOnTheMap.Add(PutSymbolToTheMap(m_SamplePathFieldSymbol, new Vector2(m_LeftBackCornerPositionOnTheMap.x + PathFromPlayerToTarget[i].Key * m_OneSymbolWidthOnTheMap, m_LeftBackCornerPositionOnTheMap.y + PathFromPlayerToTarget[i].Value * m_OneSymbolWidthOnTheMap)));
            }
        }
    }

    public void DeleteShowedPath()
    {
        for(int i = m_AllPathSymbolsOnTheMap.Count - 1; i >= 0; i--)
        {
            Destroy(m_AllPathSymbolsOnTheMap[i]);
        }
        m_AllPathSymbolsOnTheMap.Clear();
    }
}