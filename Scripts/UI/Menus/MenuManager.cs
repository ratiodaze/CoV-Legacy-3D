using UnityEngine;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    [Header("Menus to Manage")]
    [SerializeField] private List<MenuElement> menus = new();

    private Dictionary<KeyCode, MenuElement> keyToMenu = new();

    private void Awake()
    {
        foreach (var menu in menus)
        {
            if (menu.menuRoot == null)
            {
                Debug.LogWarning($"[MenuManager] Menu '{menu.menuName}' has no root assigned.");
                continue;
            }

            // Start hidden if desired
            if (menu.startHidden)
                menu.menuRoot.SetActive(false);

            // Register key if valid
            if (menu.toggleKey != KeyCode.None)
            {
                if (!keyToMenu.ContainsKey(menu.toggleKey))
                    keyToMenu.Add(menu.toggleKey, menu);
                else
                    Debug.LogWarning($"[MenuManager] Duplicate toggle key '{menu.toggleKey}' assigned to multiple menus.");
            }
        }
    }

    private void Update()
    {
        foreach (var kvp in keyToMenu)
        {
            if (Input.GetKeyDown(kvp.Key))
            {
                ToggleMenu(kvp.Value);
                break; // One menu per frame
            }
        }
    }

    private void ToggleMenu(MenuElement menu)
    {
        if (menu.menuRoot == null) return;

        bool newState = !menu.menuRoot.activeSelf;
        menu.menuRoot.SetActive(newState);

        Debug.Log($"[MenuManager] {(newState ? "Opened" : "Closed")} '{menu.menuName}'");
    }
}
