using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class VictoryLevel2 : MonoBehaviour
{
    public TMP_Text messageText;      // Kéo UI Text vào đây (ví dụ SaveMessage)
    public string victoryMessage = "VICTORY ACHIEVED!";
    public Color messageColor = Color.yellow;
    public float displayDuration = 3f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Kiểm tra nếu người chạm là Player
        if (other.CompareTag("Player1"))
        {
            Debug.Log("Player reached victory zone!");
            StartCoroutine(ShowVictoryMessage());
        }
    }

    private IEnumerator ShowVictoryMessage()
    {
        messageText.text = victoryMessage;
        messageText.color = messageColor;
        messageText.gameObject.SetActive(true);

        yield return new WaitForSeconds(displayDuration);

        messageText.gameObject.SetActive(false);
    }
}