using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    public int sceneID;
    public Slider loadSlider;

    private void Start()
    {
        StartCoroutine(LoadNextScene(sceneID));
    }

    /// <summary>
    /// ����� ��������� ��������� ����� � ���������� ������� ��������
    /// </summary>
    /// <param name="sceneNumber">����� �����</param>
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