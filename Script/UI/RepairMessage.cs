using TMPro;
using UnityEngine;
using System.Collections;

public class RepairMessage : MonoBehaviour
{
    public static RepairMessage Instance { get; private set; }
    public TextMeshProUGUI repairMessageText;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public void ShowRepairMessage(string message)
    {
        StartCoroutine(ShowMessageCoroutine(message));
    }

    private IEnumerator ShowMessageCoroutine(string message)
    {
        repairMessageText.text = message;
        yield return new WaitForSeconds(3f); // 메시지를 3초 동안 표시
        repairMessageText.text = "";
    }
}
