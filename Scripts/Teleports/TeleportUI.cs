using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TeleportUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform buttonParent;
    [SerializeField] private TextMeshProUGUI descriptionText; // Optional

    private void Start()
    {
        GenerateWaypointButtons();
    }

    private void GenerateWaypointButtons()
    {
        var waypoints = TeleportManager.Instance.GetWaypoint();

        foreach (var waypoint in waypoints)
        {
            GameObject buttonObj = Instantiate(buttonPrefab, buttonParent);
            buttonObj.SetActive(true); // Always activate prefab before accessing children

            // Set button label to the waypoint's name
            var label = buttonObj.GetComponentInChildren<TextMeshProUGUI>();
            if (label != null)
                label.text = waypoint.waypointName;
            else
                Debug.LogWarning($"[TeleportUI] Label not found on button: {buttonObj.name}");

            var button = buttonObj.GetComponent<Button>();

            // ✅ Capture this specific waypoint in a local variable so it's not lost in the loop
            var capturedWaypoint = waypoint;

            // Add listener that references the correct waypoint (thanks to capturedWaypoint)
            button.onClick.AddListener(() =>
            {
                Debug.Log($"[TeleportUI] Clicked: {capturedWaypoint.waypointName}");
                TeleportManager.Instance.TeleportToWaypoint(capturedWaypoint);
                if (descriptionText != null)
                    descriptionText.text = capturedWaypoint.description;
            });
        }
    }

}
