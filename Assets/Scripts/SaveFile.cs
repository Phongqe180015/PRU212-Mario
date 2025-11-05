using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class GameScore
{
    public List<LevelScore> levelScores = new List<LevelScore>();  // ✅ Khởi tạo mặc định
}
[System.Serializable]
public class LevelScore
{
    public string levelName;  // ✅ Đổi từ int sang string để lưu tên scene
    public int score;
    public float VectorX;
    public float VectorY;
    public float VectorZ;
}
public class SaveFile : MonoBehaviour
{
    public GameScore gameScore;

    private void Start()
    {
        LoadData();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            // ✅ Lưu TÊN SCENE trực tiếp, không dùng GetHashCode()
            string currentLevelName = SceneManager.GetActiveScene().name;
            int currentScore = GameManager.instance != null ? GameManager.instance.player1Score : 0;
            Vector3 currentPosition = transform.position;
            
            Debug.Log($"Saving - Scene: {currentLevelName}, Position: {currentPosition}");
            Save(currentLevelName, currentScore, currentPosition);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Debug.Log("Load");
            string currentLevelName = SceneManager.GetActiveScene().name;
            Load(currentLevelName);
        }
    }
    
    // ✅ HÀM MỚI: Để gọi từ UI Button (không có tham số)
    public void SaveGame()
    {
        string currentLevelName = SceneManager.GetActiveScene().name;
        int currentScore = GameManager.instance != null ? GameManager.instance.player1Score : 0;
        Vector3 currentPosition = transform.position;
        
        Debug.Log($"[SaveGame Button] Saving - Scene: {currentLevelName}, Position: {currentPosition}");
        Save(currentLevelName, currentScore, currentPosition);
    }
    
    // ✅ HÀM MỚI: Để load từ UI Button
    public void LoadGame()
    {
        string currentLevelName = SceneManager.GetActiveScene().name;
        Debug.Log($"[LoadGame Button] Loading scene: {currentLevelName}");
        Load(currentLevelName);
    }
    public void Save(string levelName, int score, Vector3 position)
    {
        // ✅ Kiểm tra gameScore đã được khởi tạo chưa
        if (gameScore == null)
        {
            gameScore = new GameScore();
        }
        
        foreach (var levelScore in gameScore.levelScores)
        {
            if (levelScore.levelName == levelName)
            {
                levelScore.score = score;
                levelScore.VectorX = position.x;
                levelScore.VectorY = position.y;
                levelScore.VectorZ = position.z;
                SaveData();
                Debug.Log($"Updated level '{levelName}' - Score: {score}, Position: {position}");
                return;
            }
        }
        
        // ✅ Thêm level mới nếu chưa tồn tại
        gameScore.levelScores.Add(new LevelScore
        {
            levelName = levelName,
            score = score,
            VectorX = position.x,
            VectorY = position.y,
            VectorZ = position.z
        });
        SaveData();
        Debug.Log($"Saved new level '{levelName}' - Score: {score}, Position: {position}");
    }
    public void Load(string levelName)
    {
        if (gameScore == null || gameScore.levelScores == null)
        {
            Debug.LogWarning("No save data available to load.");
            return;
        }
        
        foreach (var levelScore in gameScore.levelScores)
        {
            if (levelScore.levelName == levelName)
            {
                // ✅ ÁP DỤNG vị trí đã load cho nhân vật
                Vector3 loadedPosition = new Vector3(levelScore.VectorX, levelScore.VectorY, levelScore.VectorZ);
                transform.position = loadedPosition;
                
                // ✅ Cập nhật score nếu có GameManager
                if (GameManager.instance != null)
                {
                    GameManager.instance.player1Score = levelScore.score;
                }
                
                Debug.Log($"Loaded level '{levelName}' - Score: {levelScore.score}, Position: {loadedPosition}");
                Debug.Log($"Player moved to position: {transform.position}");
                return;
            }
        }

        Debug.Log($"No save data found for level '{levelName}'");
    }
    public void LoadData()
    {
        string file = "savefile.json";
        string filePath = Path.Combine(Application.persistentDataPath, file);
        
        // ✅ Nếu file không tồn tại, khởi tạo gameScore mới
        if (!File.Exists(filePath)) 
        {
            gameScore = new GameScore();
            Debug.Log("No save file found. Created new GameScore.");
            return;
        }

        try
        {
            // ✅ Đọc và parse JSON
            string jsonContent = File.ReadAllText(filePath);
            
            // ✅ Kiểm tra nội dung file có rỗng không
            if (string.IsNullOrEmpty(jsonContent))
            {
                gameScore = new GameScore();
                Debug.Log("Save file is empty. Created new GameScore.");
                return;
            }
            
            gameScore = JsonUtility.FromJson<GameScore>(jsonContent);
            
            // ✅ Kiểm tra parse có thành công không
            if (gameScore == null)
            {
                gameScore = new GameScore();
                Debug.LogWarning("Failed to parse save file. Created new GameScore.");
            }
            else
            {
                Debug.Log($"Data Loaded successfully. Found {gameScore.levelScores.Count} level(s).");
            }
        }
        catch (System.Exception e)
        {
            // ✅ Bắt lỗi khi parse JSON
            Debug.LogError($"Error loading save file: {e.Message}");
            gameScore = new GameScore();
        }
    }
    public void SaveData()
    {
        string file = "savefile.json";
        string filePath = Path.Combine(Application.persistentDataPath, file);

        string json = JsonUtility.ToJson(gameScore, true);

        File.WriteAllText(filePath, json);
        Debug.Log("Data Saved, at path: " + filePath);
    }
}
