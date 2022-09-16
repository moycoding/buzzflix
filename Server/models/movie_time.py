from marshmallow import Schema, fields

from .theater import TheaterSchema

class MovieTime():
    def __init__(self, time, theater):
        self.time = time
        self.theater = theater

class MovieTimeSchema(Schema):
    time = fields.Time()
    theater = fields.Nested(TheaterSchema)