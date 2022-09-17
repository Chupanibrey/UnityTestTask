using UnityEngine;
using UnityEngine.SceneManagement;

public class MainScene : MonoBehaviour
{
    /// <summary>
    /// ����� ��������� ��������� ����� ����� ������� ������
    /// </summary>
    /// <param name="sceneID">����� �����</param>
    public void OnClickLoadScene(int sceneID)
    {
        SceneManager.LoadScene(sceneID);
    }
}