from marshmallow import Schema, fields

from .movie_time import MovieTimeSchema

class Movie():
    def __init__(self, id, name, date, times):
        self.id = id
        self.name = name
        self.date = date
        self.times = times

class MovieSchema(Schema):
    id = fields.Int()
    name = fields.Str()
    date = fields.Date()
    times = fields.Pluck(MovieTimeSchema, "time", many=True)
