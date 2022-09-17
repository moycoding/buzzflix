using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
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
                var movies = JsonUtility.FromJson<Movies>(request.downloadHandler.text);
                Movies = movies.data.ConvertAll(m =>
                {
                    var times = m.times.ConvertAll(t => {
                        var time = DateTime.Parse(t);
                        return time.ToString("H:mm");
                    });
                    
                    return new MovieData(m.id, m.name, times);
                });

                onMoviesUpdated?.Invoke();
            }
        }
    }

    public void GetReservations(int movieId, DateTime time)
    {
        Reservations = null;
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
                var reservations = JsonUtility.FromJson<ReservationWrapper>(request.downloadHandler.text);
                Reservations = reservations.data;

                onReservationsUpdated?.Invoke();
            }
        }
    }

    public void PostReservation()
    {
        if (Movies == null || m_SelectionController.MovieIndex == -1) return;
        if (m_SelectionController.TimeIndex == -1) return;

        var movie = Movies[m_SelectionController.MovieIndex];
        var id = movie.id;
        var timeString = movie.times[m_SelectionController.TimeIndex];
        var time = DateTime.Parse(timeString);
        StartCoroutine(PostSeatReservation(id, time, m_SelectionController.Row, m_SelectionController.Seat, m_SelectionController.ReservationName));
    }
    private string m_RequestBody;
    public IEnumerator PostSeatReservation(int movieId, DateTime time, int row, int seat, string name)
    {
        var uriBuilder = new UriBuilder("http", "localhost", 5000);
        uriBuilder.Path = "/reservations";
        var uri = uriBuilder.Uri;
        var body = $"{{ \"movie_id\": {movieId}, \"time\": \"{time.ToString("HH:mm:ss")}\", \"row\": {row}, \"seat\": {seat}, \"name\": \"{name}\" }}";

        var request = new UnityWebRequest(uri, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(body);
        request.uploadHandler = (UploadHandler) new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler) new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("Accept", "application/json");
        yield return request.SendWebRequest();
        if (request.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log(request.error);
        }
        else
        {
            Debug.Log(request.responseCode);
            Debug.Log(request.downloadHandler.text);
        }
    }
}