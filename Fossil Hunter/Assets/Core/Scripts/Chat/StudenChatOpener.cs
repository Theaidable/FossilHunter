using UnityEngine;

/// <summary>
/// Scriptet bruges til at åbne chatten
/// </summary>
/// <author> David Gudmund Danielsen </author>
public class StudenChatOpener : MonoBehaviour
{
    [SerializeField] private GameObject chatPanel;

    private void OnMouseDown()
    {
        if(chatPanel != null)
        {
            chatPanel.SetActive(true);
        }
    }
}
