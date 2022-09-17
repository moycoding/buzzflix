using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovieListController
{
    private VisualTreeAsset m_ListEntryTemplate;
    private ListView m_ListView;

    private SelectionController m_SelectionController;
    private ApiService m_ApiService;
    private WeakReference<MovieListEntryController> m_SelectedTimeController = new(null);

    public MovieListController(
        ListView listView,
        VisualTreeAsset listEntryTemplate,
        SelectionController selectionController,
        ApiService apiService)
    {
        m_SelectionController = selectionController;
        m_ApiService = apiService;

        LoadMovieData();

        m_ListView = listView;
        m_ListEntryTemplate = listEntryTemplate;

        PopulateMovieList();

        selectionController.onDateChanged += (date) =>
        {
            SelectMovieTime(-1, -1, null);
            LoadMovieData();
        };

        apiService.onMoviesUpdated += () =>
        {
            PopulateMovieList();
        };
    }

    private void LoadMovieData()
    {
        // call api service
    }

    private void PopulateMovieList()
    {
        Debug.Log("Populating movie list");
        m_ListView.makeItem = () =>
        {
            var newListEntry = m_ListEntryTemplate.Instantiate();
            var newListEntryLogic = new MovieListEntryController();
            newListEntry.userData = newListEntryLogic;
            newListEntryLogic.SetVisualElement(newListEntry);

            return newListEntry;
        };

        m_ListView.bindItem = (item, index) =>
        {
            var controller = item.userData as MovieListEntryController;
            int selectedIndex = (index == m_SelectionController.MovieIndex) ? m_SelectionController.TimeIndex : -1;
            Debug.Log(m_ApiService.Movies);
            controller.SetMovieData(m_ApiService.Movies[index], selectedIndex, (timeIndex) =>
                {
                    SelectMovieTime(index, timeIndex, controller);
                }
            );
        };

        m_ListView.itemsSource = m_ApiService.Movies;
    }

    private void SelectMovieTime(int movieIndex, int timeIndex, MovieListEntryController controller)
    {
        MovieListEntryController prevSelectionController;
        if (m_SelectedTimeController.TryGetTarget(out prevSelectionController))
        {
            prevSelectionController.Unselect(m_SelectionController.TimeIndex);
        }

        m_SelectedTimeController.SetTarget(controller);
        m_SelectionController.MovieIndex = movieIndex;
        m_SelectionController.TimeIndex = timeIndex;
    }
}
