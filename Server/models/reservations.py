from marshmallow import Schema, fields

from .theater import TheaterSchema
from .reservation import ReservationSchema

class Reservations():
    def __init__(self, theater, reserved_seats):
        self.theater = theater
        self.reserved_seats = reserved_seats

class ReservationsSchema(Schema):
    theater = fields.Nested(TheaterSchema)
    reserved_seats = fields.List(fields.Nested(ReservationSchema))
