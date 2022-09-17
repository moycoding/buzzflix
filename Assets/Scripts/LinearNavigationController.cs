using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

// TODO: make this an instance of VisualElement, if possible
public class LinearNavigationController
{
    public Action onAction;

    private const string navPanelClassName = "nav-panel";
    private const string selectedContentClassName = "selected-content";
    private const string unselectedContentClassName = "unselected-content";
    private const string backButtonName = "BackButton";
    private const string nextButtonName = "NextButton";

    private Button m_BackButton;
    private Button m_NextButton;
    private int m_CurrentPanelIndex = -1;
    private readonly VisualElement m_Root;
    private List<VisualElement> m_Panels = new();

    public LinearNavigationController(VisualElement root, VisualElement panelsParent)
    {
        m_Root = root;

        var panels = panelsParent.hierarchy;
        for (var i = 0; i < panels.childCount; i++)
        {
            m_Panels.Add(panels[i]);
        }

        m_BackButton = root.Q<Button>(backButtonName);
        m_NextButton = root.Q<Button>(nextButtonName);
        m_CurrentPanelIndex = 0;
        // TODO: make this more robust

        m_BackButton.clicked += () => Back();
        m_NextButton.clicked += () => Next();
        
        UpdateButtons();
    }

    public void SetCurrentPanel(int index)
    {
        // TODO: assert?
        if (index < 0 || index >= m_Panels.Count) return;

        var prevIndex = m_CurrentPanelIndex;
        m_CurrentPanelIndex = index;

        DisablePanel(prevIndex);
        EnablePanel(m_CurrentPanelIndex);
        UpdateButtons();
    }

    private void Back()
    {
        if (m_CurrentPanelIndex > 0)
        {
            SetCurrentPanel(m_CurrentPanelIndex - 1);
        }
    }

    private void Next()
    {
        if (m_CurrentPanelIndex < m_Panels.Count - 1)
        {
            SetCurrentPanel(m_CurrentPanelIndex + 1);
        }
        else
        {
            onAction?.Invoke();
            SetCurrentPanel(0);
        }
    }

    private void UpdateButtons()
    {
        m_BackButton.visible = m_CurrentPanelIndex > 0;
        m_NextButton.text = m_CurrentPanelIndex < m_Panels.Count - 1 ? "Next" : "Reserve";
    }

    private void DisablePanel(int index)
    {
        if (index < 0 || index >= m_Panels.Count) return;
        m_Panels[index].RemoveFromClassList(selectedContentClassName);
        m_Panels[index].AddToClassList(unselectedContentClassName);
    }

    private void EnablePanel(int index)
    {
        if (index < 0 || index >= m_Panels.Count) return;
        m_Panels[index].RemoveFromClassList(unselectedContentClassName);
        m_Panels[index].AddToClassList(selectedContentClassName);
    }
}
