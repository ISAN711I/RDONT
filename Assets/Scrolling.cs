using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Scrolling : MonoBehaviour
{
    public float speed = 100f;                          // Speed at which the text moves
    public RectTransform parentBounds;                  // The panel or container RectTransform
    public TMP_Text textElement;                        // The TextMeshPro UI text
    [TextArea]
    public string[] phrases;                            // List of messages to choose from

    private RectTransform rect;                         // RectTransform of the text element

    private bool isScrolling = false;
    private bool showingWaveMessage = false;

    void Start()
    {
        rect = textElement.GetComponent<RectTransform>();
        PickNewPhrase();
        ResetPosition();
    }

    void Update()
    {
        if (isScrolling)
        {
            rect.anchoredPosition += Vector2.left * speed * Time.deltaTime;

            if (rect.anchoredPosition.x < -rect.rect.width)
            {
                if (showingWaveMessage)
                {
                    showingWaveMessage = false;
                    PickNewPhrase(); // Choose a new random phrase instead of restoring old one
                }
                else
                {
                    PickNewPhrase();
                }

                ResetPosition();
            }
        }
    }

    void PickNewPhrase()
    {
        if (phrases.Length == 0) return;

        textElement.text = phrases[Random.Range(0, phrases.Length)];
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        isScrolling = true;
    }

    void ResetPosition()
    {
        float startX = Screen.width;
        rect.anchoredPosition = new Vector2(startX, rect.anchoredPosition.y);
    }

    public void DisplayWaveMessage(int waveNumber)
    {
        textElement.text = $"WAVE: {waveNumber}";
        LayoutRebuilder.ForceRebuildLayoutImmediate(rect);
        showingWaveMessage = true;
        isScrolling = true;
        ResetPosition();
    }
}
