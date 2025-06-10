using UnityEngine;

public class Jesse : MonoBehaviour
{
    [System.Serializable]
    public class PanelInfo
    {
        public GameObject panel;           // The panel to show
        public float displayDuration = 5f; // How long to show this panel
    }

    [Tooltip("List of panels and how long each should be shown.")]
    public PanelInfo[] panels;

    private int currentPanelIndex = 0;

    private void Start()
    {
        if (panels.Length == 0)
        {
            Debug.LogWarning("PanelSwitcher: No panels assigned.");
            return;
        }

        ShowPanel(0);
    }

    void ShowPanel(int index)
    {
        // Deactivate all panels first
        for (int i = 0; i < panels.Length; i++)
        {
            if (panels[i].panel != null)
                panels[i].panel.SetActive(false);
        }

        // Activate the current panel
        if (panels[index].panel != null)
        {
            panels[index].panel.SetActive(true);
            Invoke(nameof(SwitchToNextPanel), panels[index].displayDuration);
        }
    }

    void SwitchToNextPanel()
    {
        currentPanelIndex++;

        if (currentPanelIndex < panels.Length)
        {
            ShowPanel(currentPanelIndex);
        }
        else
        {
            Debug.Log("PanelSwitcher: Finished showing all panels.");
        }
    }
}
