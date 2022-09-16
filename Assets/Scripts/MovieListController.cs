using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovieListController
{
    private VisualTreeAsset m_ListEntryTemplate;
    private ListView m_ListView;
    private List<MovieData> m_MovieData;

    private int m_SelectedMovieIndex = -1;
    private int m_SelectedTimeIndex = -1;
    private WeakReference<MovieListEntryController> m_SelectedTimeController = new(null);

    public MovieListController(ListView listView, VisualTreeAsset listEntryTemplate)
    {
        LoadMovieData();

        m_ListView = listView;
        m_ListEntryTemplate = listEntryTemplate;

        PopulateMovieList();
    }

    private void LoadMovieData()
    {
        m_MovieData = new();
        var movie1Times = new List<string> {"10:30", "1:30", "4:30"};
        m_MovieData.Add(new("The Origin Story", movie1Times));
        var movie2Times = new List<string> {"11:00", "1:45", "5:30"};
        m_MovieData.Add(new("Action Blockbuster Sequel", movie2Times));
    }

    private void PopulateMovieList()
    {
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
            int selectedIndex = (index == m_SelectedMovieIndex) ? m_SelectedTimeIndex : -1;
            controller.SetMovieData(m_MovieData[index], selectedIndex, (timeIndex) =>
                {
                    SelectMovieTime(index, timeIndex, controller);
                }
            );
        };

        m_ListView.itemsSource = m_MovieData;
    }

    private void SelectMovieTime(int movieIndex, int timeIndex, MovieListEntryController controller)
    {
        MovieListEntryController prevSelectionController;
        if (m_SelectedTimeController.TryGetTarget(out prevSelectionController))
        {
            prevSelectionController.Unselect(m_SelectedTimeIndex);
        }

        m_SelectedTimeController.SetTarget(controller);
        m_SelectedMovieIndex = movieIndex;
        m_SelectedTimeIndex = timeIndex;
    }
}
