from marshmallow import Schema, fields

class Theater():
    def __init__(self, seats_per_row):
        self.seats_per_row = seats_per_row

class TheaterSchema(Schema):
    seats_per_row = fields.List(fields.Int)