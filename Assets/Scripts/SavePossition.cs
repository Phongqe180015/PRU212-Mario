using UnityEngine;
using UnityEngine.SceneManagement;

public class SavePossition : MonoBehaviour
{
    private bool isLoadingFromSave = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        LoadGame();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            DeleteData();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SavePosition();
        }
    }

    
    
    public void SavePosition()
    {
        Vector3 pos = transform.position;
        PlayerPrefs.SetFloat("PlayerX", pos.x);
        PlayerPrefs.SetFloat("PlayerY", pos.y);
        PlayerPrefs.SetFloat("PlayerZ", pos.z);

        string currentScene = SceneManager.GetActiveScene().name;
        PlayerPrefs.SetString("SavedScene", currentScene);
        PlayerPrefs.Save();
        Debug.Log("Game Saved in " + currentScene + " with Position: " + pos);
    }
    public void LoadGame()
    {
        if (PlayerPrefs.HasKey("SavedScene"))
        {
            string savedSceneName = PlayerPrefs.GetString("SavedScene");
            string currentSceneName = SceneManager.GetActiveScene().name;
            
            
            if (savedSceneName != currentSceneName && !isLoadingFromSave)
            {
                isLoadingFromSave = true;
                SceneManager.LoadScene(savedSceneName);
                return;
            }

            else if (savedSceneName == currentSceneName)
            {
                LoadPosition();
            }
        }
    }
    public void LoadPosition()
    {
        if (PlayerPrefs.HasKey("PlayerX") && PlayerPrefs.HasKey("PlayerY") && PlayerPrefs.HasKey("PlayerZ"))
        {
            float x = PlayerPrefs.GetFloat("PlayerX", transform.position.x);
            float y = PlayerPrefs.GetFloat("PlayerY", transform.position.y);
            float z = PlayerPrefs.GetFloat("PlayerZ", transform.position.z);
            transform.position = new Vector3(x, y, z);
            Debug.Log("Position Loaded: " + transform.position);
        }
    }   
    public void DeleteData()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Position Delete: ");
    }
}
