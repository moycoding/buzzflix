using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [SerializeField]
    VisualTreeAsset movieListEntryTemplate;

    private const string navContentName = "NavContent";
    private const string nameDatePanelName = "NameDatePanel";
    private const string movieListName = "MovieList";
    private const string seatSelectionPanelName = "SeatSelectionPanel";

    private LinearNavigationController m_NavController;
    private NameDateController m_NameDateController;
    private MovieListController m_MovieListController;
    private SeatSelectionController m_SeatSelectionController;

    void OnEnable()
    {
        UIDocument doc = GetComponent<UIDocument>();
        VisualElement root = doc.rootVisualElement;

        var contentContainer = root.Q<VisualElement>(navContentName);
        m_NavController = new(root, contentContainer);

        var nameDatePanel = root.Q<VisualElement>(nameDatePanelName);
        m_NameDateController = new(nameDatePanel);

        var movieList = root.Q<ListView>(movieListName);
        m_MovieListController = new(movieList, movieListEntryTemplate);

        var seatSelectionPanel = root.Q<VisualElement>(seatSelectionPanelName);
        m_SeatSelectionController = new(seatSelectionPanel);
    }
}
