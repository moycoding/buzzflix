from flask import Flask, jsonify, request
from datetime import date, time, datetime
from pprint import pprint

from .models.movie import Movie, MovieSchema
from .models.movie_time import MovieTime
from .models.theater import Theater
from .models.reservations import Reservations, ReservationsSchema
from .models.reservation import Reservation, ReservationSchema

app = Flask(__name__)

theaters = [
    Theater([8, 8, 8, 9, 9, 9, 10, 10, 10, 10, 10, 10]),
    Theater([10, 10, 10, 10, 10, 10, 10, 10, 12, 12, 12, 12, 12, 12, 12, 12])
]

movies = [
    Movie(1, 'The First Movie', date(2022, 9, 22), [MovieTime(time(11, 0, 0), theaters[0]), MovieTime(time(14, 30, 0), theaters[0])]),
    Movie(2, 'The Not So Good Sequel', date(2022, 9, 22), [MovieTime(time(11, 30, 0), theaters[1]), MovieTime(time(15, 45, 0), theaters[1])]),
    Movie(3, 'The First Movie', date(2022, 9, 23), [MovieTime(time(10, 45, 0), theaters[0]), MovieTime(time(14, 30, 0), theaters[0])]),
    Movie(4, 'The Not So Good Sequel', date(2022, 9, 23), [MovieTime(time(11, 15, 0), theaters[1]), MovieTime(time(15, 45, 0), theaters[1])])
]

reserved_seats = [
    Reservation(1, time(11, 0, 0), 4, 4, 'Chris'),
    Reservation(1, time(11, 0, 0), 4, 5, 'Freddie'),
    Reservation(2, time(11, 30, 0), 7, 3, 'Roger')
]

def toDate(dateString): 
    return date.fromisoformat(dateString)

@app.route('/movies')
def get_movies():
    date = request.args.get('date', default = datetime.today, type = toDate)
    schema = MovieSchema(many=True)
    output = schema.dump(
        filter(lambda t: t.date == date, movies)
    )
    return jsonify(output)

def toTime(timeString): 
    return time.fromisoformat(timeString)

@app.route('/reservations')
def get_reservations():
    id = request.args.get('movie_id', type=int)
    time = request.args.get('time', type = toTime)

    selectedMovies = list(filter(lambda m: m.id == id, movies))
    if len(selectedMovies) == 0:
        return '', 404
    selectedMovie = selectedMovies[0]

    times = list(filter(lambda t: t.time == time, selectedMovie.times))
    if len(times) == 0:
        return '', 404
    selectedTime = times[0]

    selected_seats = list(filter(lambda s: s.id == id and s.time == time, reserved_seats))
    reservations = Reservations(selectedTime.theater, selected_seats)

    schema = ReservationsSchema()
    output = schema.dump(reservations)
    return jsonify(output)

@app.route('/allreservations')
def get_all_reservations():
    schema = ReservationSchema(many=True)
    output = schema.dump(reserved_seats)
    return jsonify(output)

    
@app.route('/reservations', methods=['POST'])
def add_reservation():
    reservationData = request.get_json()
    pprint(reservationData)
    reservation = Reservation(reservationData['movie_id'], toTime(reservationData['time']), int(reservationData['row']), int(reservationData['seat']), reservationData['name'])
    pprint(reservation)
    reserved_seats.append(reservation)
    pprint(reserved_seats)
    return '', 204


if __name__ == "__main__":
    app.run()
