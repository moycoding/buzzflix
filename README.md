# buzzflix
Unity UI Toolkit app prototype


## Purpose
A movie theater seat reservation app to show my skills with Unity UI Toolkit and Flask.

## Prerequisites
To run this prototype you must have these apps/packages installed on your machine:
- python version >= 3.7
- flask `pip install flask`
- marshmallow `pip install flask`
- Unity 2021.3.9f1 or compatible

## Running
Start the server locally by running this command in the `/Server` folder:
`flask --app index run`

Then start the client from the Unity editor.

The client will show the current day, but I populated Sept. 22 and 23, so choose one of those dates.

## Design Decisions

### Client
I chose UI Toolkit because it felt more familiar with my React Native experience. However, I ran into plenty of snags along the way that I had to figure out/work around. But it did feel like I was able to dabble in a few areas/controls, such as `ListView`, `ScrollView`, a bit of flexbox flexing, navigation and event triggered fetches

I tried to make things as modular as possible in the time allotted. The code falls into the categories of controllers, models, and services.

### Server
I used Flask because of it's ease of setup and light weight. To save time I implemented the data store in memory.

Marshmallow seems to be a pretty popular schema package, so I went with that too.

The endpoints are:

`GET /movies`
Params:
`date`: ISO date to query

`GET /reservations`
Params:
`movie_id`: id of movie
`time`: ISO time of movie

`POST /reservations`
Body:
`movie_id`, `time`, `row`, `seat` and `name`

`GET /allreservations`

Use the `/allreservations` endpoint to see all reservations, including names.

## Ideas / Improvements
Things obviously got a little messy as time drew short, so first I'd do some heavy re-factoring.

There are a few bugs in the client and server. There isn't much error checking, but the basic functionality works in most cases.

I would have liked to create some custom `VisualElement`s to put directly in the hierarchy.

It would have been overkill for this project, but I would have liked to pull in a library or two to make the code more reactive, functional and declarative, especially for an app like this. Some UnityRx streams would be nice.

I thought I would get a chance to add some theming, but it kept falling off the priority list, but it's something I want to play with.

I also wanted to add just a hint of juice.