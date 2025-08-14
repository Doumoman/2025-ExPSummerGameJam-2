using UnityEngine;

public class CheckScoreManager : MonoBehaviour
{
    int[,] _tileMap = new int[5, 6];
    int[,] _bugMap = new int [5, 6];
    
    // 육각형 타일맵의 6개 방향 (Flat-top hexagon 기준)
    Vector2Int[] hexDirections = {
        new Vector2Int(1, 0),   // 동쪽
        new Vector2Int(1, -1),  // 남동쪽
        new Vector2Int(0, -1),  // 남서쪽
        new Vector2Int(-1, 0),  // 서쪽
        new Vector2Int(-1, 1),  // 북서쪽
        new Vector2Int(0, 1)    // 북동쪽
    };
    
    void Start()
    {
        // 테스트용 타일맵 초기화
        InitializeTestTileMap();
        
        // 직선 탐지 및 bugMap 설정
        DetectAndMarkLines();
        
        // 결과 출력
        PrintResults();
    }
    
    void InitializeTestTileMap()
    {
        // 예시: 대각선으로 같은 타입의 타일 배치
        _tileMap[0, 0] = 1;
        _tileMap[1, 0] = 1;
        _tileMap[2, 0] = 1;
        _tileMap[3, 0] = 1;
        
        // 다른 직선 예시
        _tileMap[1, 2] = 2;
        _tileMap[1, 3] = 2;
        _tileMap[1, 4] = 2;
    }
    
    void DetectAndMarkLines()
    {
        // bugMap 초기화
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                _bugMap[x, y] = 0;
            }
        }
        
        // 각 위치에서 6개 방향으로 직선 탐지
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                CheckLinesFromPosition(x, y);
            }
        }
    }
    
    void CheckLinesFromPosition(int startX, int startY)
    {
        int currentTileType = _tileMap[startX, startY];
        
        // 각 방향으로 직선 탐지
        for (int dir = 0; dir < 6; dir++)
        {
            var line = GetLineInDirection(startX, startY, dir, currentTileType);
            
            // 3개 이상 연결된 직선이면 bugMap에 -1 설정
            if (line.Count >= 3)
            {
                foreach (var pos in line)
                {
                    _bugMap[pos.x, pos.y] = -1;
                }
            }
        }
    }
    
    System.Collections.Generic.List<Vector2Int> GetLineInDirection(int startX, int startY, int direction, int tileType)
    {
        var line = new System.Collections.Generic.List<Vector2Int>();
        Vector2Int currentPos = new Vector2Int(startX, startY);
        Vector2Int dir = hexDirections[direction];
        
        // 시작 위치부터 해당 방향으로 같은 타입의 타일을 찾아나감
        while (IsValidPosition(currentPos.x, currentPos.y) && 
               _tileMap[currentPos.x, currentPos.y] == tileType)
        {
            line.Add(currentPos);
            currentPos += dir;
            
            // 육각형 좌표계에서 y좌표 조정 (홀수/짝수 행에 따라)
            currentPos = AdjustHexCoordinate(currentPos, dir);
        }
        
        return line;
    }
    
    Vector2Int AdjustHexCoordinate(Vector2Int pos, Vector2Int direction)
    {
        // Flat-top 육각형에서 홀수/짝수 행에 따른 좌표 조정
        // 이 부분은 사용하는 육각형 좌표계에 따라 조정이 필요할 수 있습니다
        return pos;
    }
    
    bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < 5 && y >= 0 && y < 6;
    }
    
    // 더 정확한 육각형 직선 탐지 (모든 방향 고려)
    public void DetectAllHexLines(int minLineLength = 3)
    {
        // bugMap 초기화
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                _bugMap[x, y] = 0;
            }
        }
        
        bool[,] visited = new bool[5, 6];
        
        // 모든 위치에서 직선 탐지
        for (int x = 0; x < 5; x++)
        {
            for (int y = 0; y < 6; y++)
            {
                if (!visited[x, y])
                {
                    CheckAllDirectionsFromPosition(x, y, visited, minLineLength);
                }
            }
        }
    }
    
    void CheckAllDirectionsFromPosition(int startX, int startY, bool[,] visited, int minLineLength)
    {
        int tileType = _tileMap[startX, startY];
        
        // 6개 방향 모두 검사
        for (int dir = 0; dir < 6; dir++)
        {
            var linePositions = new System.Collections.Generic.List<Vector2Int>();
            
            // 해당 방향으로 연속된 같은 타입 타일 찾기
            Vector2Int currentPos = new Vector2Int(startX, startY);
            Vector2Int direction = hexDirections[dir];
            
            while (IsValidPosition(currentPos.x, currentPos.y) && 
                   _tileMap[currentPos.x, currentPos.y] == tileType)
            {
                linePositions.Add(currentPos);
                currentPos += direction;
            }
            
            // 최소 길이 이상이면 bugMap에 표시
            if (linePositions.Count >= minLineLength)
            {
                foreach (var pos in linePositions)
                {
                    _bugMap[pos.x, pos.y] = -1;
                    visited[pos.x, pos.y] = true;
                }
            }
        }
    }
    
    void PrintResults()
    {
        Debug.Log("=== TileMap ===");
        for (int y = 5; y >= 0; y--)
        {
            string row = "";
            for (int x = 0; x < 5; x++)
            {
                row += _tileMap[x, y] + " ";
            }
            Debug.Log(row);
        }
        
        Debug.Log("=== BugMap ===");
        for (int y = 5; y >= 0; y--)
        {
            string row = "";
            for (int x = 0; x < 5; x++)
            {
                row += _bugMap[x, y].ToString().PadLeft(2) + " ";
            }
            Debug.Log(row);
        }
    }
    
    // 외부에서 호출할 수 있는 메인 함수
    public void ProcessTileMapLines()
    {
        DetectAllHexLines(3); // 3개 이상 연결된 직선 탐지
    }
}
