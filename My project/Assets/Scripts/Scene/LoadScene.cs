using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public int sceneID;
    public Slider loadSlider;

    void Start()
    {
        StartCoroutine(LoadNextScene(sceneID));
    }

    /// <summary>
    /// Метод загружает выбранную сцену и показывает прогрес загрузки
    /// </summary>
    /// <param name="sceneNumber">Номер сцены</param>
    IEnumerator LoadNextScene(int sceneNumber)
    {
        float progress;
        AsyncOperation oper = SceneManager.LoadSceneAsync(sceneNumber);

        while(!oper.isDone)
        {
            progress = oper.progress / 0.99f;
            loadSlider.value = progress;
            yield return null;
        }
    }
}