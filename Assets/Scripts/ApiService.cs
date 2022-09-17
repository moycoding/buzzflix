using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class ApiService : MonoBehaviour
{
    public string URL;

    public Action onMoviesUpdated;

    public List<MovieData> Movies { get; private set; }

    private SelectionController m_SelectionController;

    private void Awake()
    {
        //GetData();
    }

    public void SetSelectionController(SelectionController selectionController)
    {
        m_SelectionController = selectionController;
        GetData(selectionController.Date);
        Debug.Log(selectionController.Date.ToString());

        selectionController.onDateChanged += (date) =>
        {
            GetData(date);
        };
    }

    public void GetData(DateTime date)
    {
        Movies = null;
        StartCoroutine(FetchData(date));
    }
    public IEnumerator FetchData(DateTime date)
    {
        var uriBuilder = new UriBuilder("http", "localhost", 5000);
        uriBuilder.Path = "/movies";
        uriBuilder.Query = $"date={date.ToString("yyyy-MM-dd")}";
        var uri = uriBuilder.Uri;
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();
            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
                var movies = JsonUtility.FromJson<Movies>(request.downloadHandler.text);
                Debug.Log(movies);
                Debug.Log(movies.data.Count);
                Movies = movies.data.ConvertAll(m =>
                {
                    Debug.Log(m.times.Count);
                    var times = m.times.ConvertAll(t => {
                        var time = DateTime.Parse(t);
                        return time.ToString("H:mm");
                    });
                    
                    Debug.Log(times.Count);
                    return new MovieData(m.id, m.name, times);
                });

                onMoviesUpdated?.Invoke();
                Debug.Log(Movies.Count);
            }
        }
    }
}