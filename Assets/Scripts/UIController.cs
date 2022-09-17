using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIController : MonoBehaviour
{
    [SerializeField]
    VisualTreeAsset movieListEntryTemplate;

    [SerializeField]
    ApiService apiService;

    private const string navContentName = "NavContent";
    private const string nameDatePanelName = "NameDatePanel";
    private const string movieListName = "MovieList";
    private const string seatSelectionPanelName = "SeatSelectionPanel";

    private LinearNavigationController m_NavController;
    private SelectionController m_SelectionController;
    private NameDateController m_NameDateController;
    private MovieListController m_MovieListController;
    private SeatSelectionController m_SeatSelectionController;

    void OnEnable()
    {
        UIDocument doc = GetComponent<UIDocument>();
        VisualElement root = doc.rootVisualElement;

        var contentContainer = root.Q<VisualElement>(navContentName);
        m_NavController = new(root, contentContainer);

        m_SelectionController = new();
        apiService.SetSelectionController(m_SelectionController);

        var nameDatePanel = root.Q<VisualElement>(nameDatePanelName);
        m_NameDateController = new(nameDatePanel, m_SelectionController);

        var movieList = root.Q<ListView>(movieListName);
        m_MovieListController = new(movieList, movieListEntryTemplate, m_SelectionController, apiService);

        var seatSelectionPanel = root.Q<VisualElement>(seatSelectionPanelName);
        m_SeatSelectionController = new(seatSelectionPanel, m_SelectionController, apiService);
    }
}
