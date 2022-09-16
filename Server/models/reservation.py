from marshmallow import Schema, fields

class Reservation():
    def __init__(self, id, time, row, seat, name):
        self.id = id
        self.time = time
        self.row = row
        self.seat = seat
        self.name = name

class ReservationSchema(Schema):
    id = fields.Int()
    time = fields.Time()
    row = fields.Int()
    seat = fields.Int()
    name = fields.Str()