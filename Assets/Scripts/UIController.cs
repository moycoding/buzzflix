using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    private const string navContentName = "NavContent";

    private LinearNavigationController m_NavController;

    void OnEnable()
    {
        UIDocument doc = GetComponent<UIDocument>();
        VisualElement root = doc.rootVisualElement;

        var contentContainer = root.Q<VisualElement>(navContentName);
        m_NavController = new(root, contentContainer);
    }
}
