using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovieListEntryController
{
    private const string movieLabelName = "MovieLabel";
    private const string MovieTimeContainerName = "MovieTimes";
    private const string MovieTimeButtonClass = "movie-time-button";
    private const string SelectedMovieTimeClass = "selected-movie-time";

    private Label m_MovieLabel;
    private VisualElement m_MovieTimesContainer;
    private List<Button> m_MovieTimeButtons;

    public void SetVisualElement(VisualElement visualElement)
    {
        m_MovieLabel = visualElement.Q<Label>(movieLabelName);
        m_MovieTimesContainer = visualElement.Q<VisualElement>(MovieTimeContainerName);
    }

    public void SetMovieData(MovieData movieData, int selectedIndex, Action<int> onSelect)
    {
        m_MovieLabel.text = movieData.Name;
        m_MovieTimeButtons = new();
        m_MovieTimesContainer.Clear();

        for (int i = 0; i < movieData.Times.Count; i++)
        {
            var button = new Button();
            button.text = movieData.Times[i];
            button.AddToClassList(MovieTimeButtonClass);

            if (i == selectedIndex)
            {
                button.AddToClassList(SelectedMovieTimeClass);
            }

            var index = i;
            button.clicked += () =>
            {
                onSelect(index);
                button.AddToClassList(SelectedMovieTimeClass);
            };

            m_MovieTimesContainer.Add(button);
            m_MovieTimeButtons.Add(button);
        }
    }

    public void Unselect(int index)
    {
        if (index >= 0 && index < m_MovieTimeButtons.Count)
        {
            m_MovieTimeButtons[index].RemoveFromClassList(SelectedMovieTimeClass);
        }
    }
}
