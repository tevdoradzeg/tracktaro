# TrackTaro - Personal Music Collection Manager & Gallery

TrackTaro (Trackთარო) is a database/website fullstack developed in `.NET`.
It is a showcase of my physical music collection to view, manage and maybe boast about.

The name TrackTaro consists of “track” - as in music track and “taro” - meaning shelf in Georgian. Think of this as a nice digital music shelf.

It is developed full stack in `C#`. The project is built using `.NET 8`, `Blazor`, and `Entity Framework Core`.

The project consists of three parts, and this README will get into each part individually to discuss
the functionality and design strategy of each:

1. [Shared Part](#shared-part)
2. [API Project](#api-project)
3. [Web Project](#web-project)

## Shared Part

The shared part of the project consists mostly of class definiton for the main database entities and
their information exchange formats

### Main Data Classes

Located directly in `TrackTaro.Shared/` there are 5 class files for the main entity types.
They contain type definitons for the EF SQLite database for the back-end to use, and also are available
for FE access for ease of use. They contain key relation and lazy-loading definitions for the EF
scheme and changing any of these classes would require a new migration and a database update.

### DTOs and Mappers

Located in `TrackTaro.Shared/Dtos` are data transfer objects associated with these classes. For each
class there are at least three DTOs: the default one, the create one and the update one.

To avoid circular dependencies and large load times, `Items` and `Artists` also have shortened DTO
versions which exclude the linked Item/Artists given on the endpoint which is called.

Mappers are implemented as extension methods on actual class objects and when called (for example `Artist.ToDto()`)
create a new object with the nessecary information. These classes should be extended and update with
any updates to the DTOs since the mappings are the main way the BE converts the objects.

## API Project

The API project is the BE part of the application and is responsible for the database, APIs, file handling
and simple authorization checks.

### Authentication

To make the developer's life easier the BE implements authorization using `Attribute, IAsyncAuthorizationFilter`,
given that this is a single (or at most a bit more than single) user application in terms of admin actions,
this suffices fully and makes it so that a secure API endpoint can just be added by adding the
`ApiKey` attribute at the top of its implementation.

The class checks for a specific authorization header and compares its value to the real key stored in config.

Currently the authorization from frontend is acquired through a password, which is sent to the AuthenticationController,
however that is expanded in the main section.

**It is important to note that both the password and the API key operate from the appsettings for now
and do not rely on a secrets manager.**

### Controllers

The controllers are designed to be versitale but mostly for the FE use of this application.

#### Artists Controller

The artists controller has its straightforward `queried GET`, `GET by id`, `POST`, `DELETE` and `PUT`
endpoints designed for searching, getting specific artist info, creating a new artists, and deleting / modifying
a select artist. The implementations of these methods are very straightforward and use EF abstraction
to work with the Artist collection in the database.

The more complicated endpoint here is `GET: api/artists/search-external`, which is used to search the
external [MusicBrainz](https://musicbrainz.org/doc/MusicBrainz_API) database, for artist names and countries. The 
use case for this endpoint is to autofill names and info while creating a new artist and to make sure
the personal collection at least somewhat matches the wider one at MusicBrainz

#### Authentication Controller

As mentioned before the API relies on header authentication, however for the FE to acquire the correct
API Token, the user must enter a password. This class contains a single endpoint which checks the validty
of the password and returns the correct token if it is validated.

#### Files Controller

The files controller manages the images required to keep for items. The images are located under
`TrackTaro.Api/wwwroot` and are sorted nicely into categories in case a manual review is needed.

The file upload endpoint uses the class in `TrackTaro.Api/Helpers/UploadHandler.cs` to validate the
incoming file, mostly for format and size checks. After this the file is distributed to the correct
folder based on the query parameter and give a new GUID to be stored. Importantly, this endpoint also
returns the newly established file path with the OK.

#### Items Controller

This is the most complicated controller on the BE. Given that the project mainly functions around
item entities and they're the "center-piece" a lot of relational changes are also handled here.

* Straightforward endpoints
The `GET id`, `POST` and `DELETE id` endpoints all work based on DTOs corresponding to their operations
and after some basic checks modify the database as per the request

* Linking endpoints
The `POST: api/items/{itemId}/artists` and `DELETE: api/items/{itemId}/artists/` are endpoint to manage
the many-to-many relationship between items and artists and their junction table. The POST endpoint 
links the entities and the DELETE endpoint unlinks them

* Search endpoint
The `GET query` endpoint is used as a search endpoint and gives the user the ability to query parameters,
and filter the dataset by them. It also implements a custom period of time filter.

* PUT endpoint
The `PUT id` endpoint is used to update items, however it also accepts a list of artists **unlike
the POST endpoint**. This decision came about after realizing it would be a lot more convinient for
FE editing use. So this endpoint **also updates the Artists <-> Items relationship**.

### Database

The database is mostly set up in `Data/MusicDbContext.cs` it declares all the shared classes and the
intended relationships between them. Of course the main part of its configuration also happens in `Program.cs`
which contains straighforward setup steps. The setup specifies the API point and also helps the
developer by using Swagger.

## Web Project

The FE of the project is managed with Blazor, and tries to utilize JS as little as possible. The CSS
configuration is mostly plain, but the default bootstrap config that comes with Blazor is also used.

### Authentication

The Authentication Service is the only custom service the FE project uses (besides this there's also
the HTTP and JSRuntime). The service is built to manage admin permissions.

It uses the `sessionStorage` of the JSRuntime to store the API key retrieved from BE to include it in
the API calls which need authorization. Doing it this way ensures that unless the tab is closed (the
session ends) the user can stay signed in and do multiple admin operations. **No API Key or Passowrd
is stored or managed on the FE side of the repo.**

### wwwroot

Contains some default css and minimal image files for the web header and icon. Mostly similar
to the default configuraiton with bootstrap

### Validation

This implements a custom validator (`: ValidationAttribute`), since the adding and editing components
on FE use `EditForm`s and validation attributes can be used to integrate the checking and error message process
nicely.

The `TimeValidationAttribute.cs` checks that the string of time entered by the user can be correctly
parse into a `TimeSpan`, which is used to save and view the length of `Track`s.

### Components

#### ItemDisplays
* 