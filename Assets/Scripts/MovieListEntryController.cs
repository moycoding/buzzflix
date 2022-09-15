using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MovieListEntryController
{
    private const string movieLabelName = "MovieLabel";
    private const string MovieTimeContainerName = "MovieTimes";
    private const string MovieTimeButtonClass = "movie-time-button";

    private Label m_MovieLabel;
    private VisualElement m_MovieTimesContainer;

    public void SetVisualElement(VisualElement visualElement)
    {
        m_MovieLabel = visualElement.Q<Label>(movieLabelName);
        m_MovieTimesContainer = visualElement.Q<VisualElement>(MovieTimeContainerName);
    }

    public void SetMovieData(MovieData movieData)
    {
        m_MovieLabel.text = movieData.Name;

        for (int i = 0; i < movieData.Times.Count; i++)
        {
            var button = new Button();
            button.text = movieData.Times[i];
            button.AddToClassList(MovieTimeButtonClass);
            m_MovieTimesContainer.Add(button);
        }
    }
}
