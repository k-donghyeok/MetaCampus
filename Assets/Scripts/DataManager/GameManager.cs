using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    private int savedRespawnPointID; // Saved respawn point ID
    private int savedSceneID; // Saved scene ID

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void UpdateRespawnPoint(int respawnPointID, int sceneID)
    {
        savedRespawnPointID = respawnPointID;
        savedSceneID = sceneID;

        SaveGame(); // Call the save method whenever the respawn point updates
    }

    private void SaveGame()
    {
        // Use PlayerPrefs, JSON serialization, or other methods to save the data
        PlayerPrefs.SetInt("RespawnPointID", savedRespawnPointID);
        PlayerPrefs.SetInt("SceneID", savedSceneID);
        PlayerPrefs.Save();
    }

    private void LoadGame()
    {
        // Load the saved respawn point and scene IDs
        savedRespawnPointID = PlayerPrefs.GetInt("RespawnPointID");
        savedSceneID = PlayerPrefs.GetInt("SceneID");

        // Load the corresponding scene and set player position to the respawn point
        // Replace "LoadSceneByID" and "SetPlayerPosition" with your own implementation
        LoadSceneByID(savedSceneID);
        SetPlayerPosition(savedRespawnPointID);
    }

    private void LoadSceneByID(int sceneID)
    {
        // Implement your own logic to load a scene based on its ID
    }

    private void SetPlayerPosition(int respawnPointID)
    {
        // Implement your own logic to set the player position based on the respawn point ID
    }

    private void Start()
    {
        // Check if there is saved data and load the game accordingly
        if (PlayerPrefs.HasKey("RespawnPointID") && PlayerPrefs.HasKey("SceneID"))
        {
            LoadGame();
        }
        else
        {
            // Start the game at the default location
        }
    }
}
