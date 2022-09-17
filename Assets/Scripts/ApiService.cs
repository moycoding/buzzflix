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
    public Action onReservationsUpdated;

    public List<MovieData> Movies { get; private set; }
    public Reservations Reservations { get; private set; }

    private SelectionController m_SelectionController;

    private void Awake()
    {
        //GetMovies();
    }

    public void SetSelectionController(SelectionController selectionController)
    {
        m_SelectionController = selectionController;
        GetMovies(selectionController.Date);
        Debug.Log(selectionController.Date.ToString());

        selectionController.onDateChanged += (date) =>
        {
            GetMovies(date);
        };

        selectionController.onTimeIndexChanged += (timeIndex) =>
        {
            Reservations = null;
            if (timeIndex != -1)
            {
                GetReservations(
                    Movies[m_SelectionController.MovieIndex].id,
                    DateTime.Parse(Movies[m_SelectionController.MovieIndex].times[timeIndex])
                );
            }
        };
    }

    public void GetMovies(DateTime date)
    {
        Movies = null;
        StartCoroutine(FetchMovies(date));
    }
    public IEnumerator FetchMovies(DateTime date)
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

    public void GetReservations(int movieId, DateTime time)
    {
        Movies = null;
        StartCoroutine(FetchReservations(movieId, time));
    }
    public IEnumerator FetchReservations(int movieId, DateTime time)
    {
        var uriBuilder = new UriBuilder("http", "localhost", 5000);
        uriBuilder.Path = "/reservations";
        uriBuilder.Query = $"movie_id={movieId}&time={time.ToString("HH:mm:ss")}";
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
                var reservations = JsonUtility.FromJson<ReservationWrapper>(request.downloadHandler.text);
                Debug.Log(reservations);
                Debug.Log(reservations.data);
                Reservations = reservations.data;

                onReservationsUpdated?.Invoke();
            }
        }
    }
}