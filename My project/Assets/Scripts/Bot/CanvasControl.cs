using UnityEngine;
using TMPro;

public class CanvasControl : MonoBehaviour
{
    [SerializeField]
    Transform canvasTransform;
    [SerializeField]
    TextMeshProUGUI health;
    [SerializeField]
    TextMeshProUGUI score;

    Bot bot;

    private void Awake()
    {
        bot = this.GetComponent<Bot>();
    }

    void LateUpdate()
    {
        health.text = ((int)bot.healph).ToString();
        score.text = bot.score.ToString();
        canvasTransform.LookAt(Camera.main.transform.position);
    }
}
