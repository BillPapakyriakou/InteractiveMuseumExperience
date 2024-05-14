using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LinkOpener : MonoBehaviour, IPointerClickHandler
{

    public void OnPointerClick(PointerEventData eventData)
    {
        TMP_Text textMeshPro = GetComponent<TMP_Text>();

        int linkIndex = TMP_TextUtilities.FindIntersectingLink(textMeshPro, Input.mousePosition, null);
        if (linkIndex != -1)
        {
            TMP_LinkInfo linkInfo = textMeshPro.textInfo.linkInfo[linkIndex];
            string url = linkInfo.GetLinkID();
            // Open the URL (or handle the link click) here
            Debug.Log("Link Clicked: " + url);
        }
    }
}
