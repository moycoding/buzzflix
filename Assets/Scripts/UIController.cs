using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [SerializeField]
    VisualTreeAsset movieListEntryTemplate;

    private const string navContentName = "NavContent";
    private const string movieListName = "MovieList";

    private LinearNavigationController m_NavController;
    private MovieListController m_MovieListController;

    void OnEnable()
    {
        UIDocument doc = GetComponent<UIDocument>();
        VisualElement root = doc.rootVisualElement;

        var contentContainer = root.Q<VisualElement>(navContentName);
        m_NavController = new(root, contentContainer);

        var movieList = root.Q<ListView>(movieListName);
        m_MovieListController = new(movieList, movieListEntryTemplate);
    }
}
