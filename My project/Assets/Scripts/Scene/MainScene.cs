using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    /// <summary>
    /// Метод загружает указанную сцену после нажатия кнопки
    /// </summary>
    /// <param name="sceneID">Номер сцены</param>
    public void OnClickLoadScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}