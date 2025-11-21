using UnityEngine;

/// <summary>
/// Scriptet bruges til at åbne chatten
/// </summary>
/// <author> David Gudmund Danielsen </author>
public class StudenChatOpener : MonoBehaviour
{
    [SerializeField] private GameObject chatPanel;

    private BoxCollider2D _collider;

    private void Awake()
    {
        _collider = GetComponent<BoxCollider2D>();
    }

    private void OnMouseDown()
    {
        if(chatPanel != null)
        {
            chatPanel.SetActive(true);

            if(_collider != null)
            {
                _collider.enabled = false;
            }
        }
    }
    
    public void EnableCollider()
    {
        if (_collider != null)
        {
            _collider.enabled = true;
        }
    }
}
