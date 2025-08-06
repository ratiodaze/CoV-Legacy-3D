using System;
using UnityEngine;

[Serializable]
public class MenuElement
{
    public string menuName;
    public GameObject menuRoot;
    public KeyCode toggleKey = KeyCode.None;
    public bool startHidden = true;
}
