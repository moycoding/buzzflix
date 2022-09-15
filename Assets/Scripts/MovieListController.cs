using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovieListController
{
    private VisualTreeAsset m_ListEntryTemplate;
    private ListView m_ListView;
    private List<MovieData> m_MovieData;

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
            (item.userData as MovieListEntryController).SetMovieData(m_MovieData[index]);
        };

        m_ListView.itemsSource = m_MovieData;
    }
}
