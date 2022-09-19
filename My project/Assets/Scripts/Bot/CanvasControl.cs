using UnityEngine;
using TMPro;

public class CanvasControl : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI health;
    [SerializeField]
    TextMeshProUGUI score;
    [SerializeField]
    Bot bot;

    Transform canvasTransform;

    private void Awake()
    {
        canvasTransform = transform;
    }

    void LateUpdate()
    {
        health.text = ((int)bot.data.healph).ToString();
        score.text = bot.data.score.ToString();
        canvasTransform.LookAt(Camera.main.transform.position);
    }
}
