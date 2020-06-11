using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadEditorScene()
    {
        SceneManager.LoadScene(SceneNames.EditorScene);
    }
}
