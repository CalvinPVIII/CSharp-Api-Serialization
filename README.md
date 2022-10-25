## C# Serialization

    - A few examples of how to get around the issue of infinite serialization when dealing with a one to many relationship in a C# .net Api

## JSON Ignore

To prevent JSON from including certain properties of an object when serializing, use the `[JsonIgnore]` decorator above any property you want to exclude.

You will need to have `using System.Text.Json.Serialization;` in your file.

For example: if there is a class that looks like this:

```c#
public class Person
{
    public int Age {get;set;}
    public string Name {get;set;}
    [JsonIgnore]
    public string HomeAddress {get;set;}
}

```

The Json serialized version of this object would be

```c#
    {
        "age": "45",
        "name": "Jimmy"
    }
```

## Data Transfer Objects

Data transfer objects are "utility" objects that are created to restructure and reshape our data however we see fit.

For example lets say we have these two classes

```c#

public class SuperHero
{
    public string Name {get;set;}
    public string Origin {get;set;}
    public string Team {get;set;}
}


public class SuperPower
{
    public string Description {get;set;}
    public int PowerRating {get;set;}
}

```

If we want to take pieces of data from each of these two classes, we could create a Data Transfer Object that might look something like this:

```c#
public class SuperDto
{
    public string HeroName {get;set;}
    public string PowerDescription {get;set;}
}

```

We could then do something like this:

```c#
SuperHero spiderMan = new SuperHero("Spider-Man", "Queens, New York", "Avengers")
SuperPower strength = new SuperPower("Super Strength", 100)

SuperDTO result = new SuperDto(){HeroName = spiderMan.Name, PowerDescription = strength.Description}
```

And our JSON result would be:

```json
{
  "HeroName": "Spider-Man",
  "PowerDescription": "Super Strength"
}
```

How does this relate back to APIs? Lets look at these two classes:

```c#

public class Album
{
    public int AlbumId {get;set;}
    public string Name {get; set;}
    public virtual ICollection<Song> Songs {get;set;}

    public Album()
    {
        Songs = new HashSet<Song>() { };
    }
}

public class Song
{
    public int SongId {get;set;}
    public string Name {get;set;}
    public int AlbumId {get;set;}
    public virtual Album Album {get;set;}

}


```

Here we have a one to many with Albums and Songs. An Album has many Songs, and Songs have one Album.

If we try to serialize a Song or and Album, we would get an infinite loop, as the song has the album, and the album contains that song, and that song contains the album, and so on infinitely.

We can get around this using a Data Transfer Object, or a DTO.

If we wanted to return our album without having an infinite loop, we could create an AlbumDTO that could look something like this:

```c#
public class AlbumDto
{
    public int AlbumId {get;set;}
    public string Name {get;set;}
    public List<string> Songs {get;set;}
}

```

In this DTO, we have all the relevant data from our initial Album class, as well as the only data that really matter from our Songs class, which is the song name, that we are now storing as a list of strings rather than a list of Song objects.

In order to use this DTO in our controller we would want to do something like this:

```c#
public ActionResult<List<AlbumDto>> Get()
{
    // Getting our list of albums from the database, including the Songs so we can access it later
    var albums = _db.Albums.Include(entity=> entity.Songs).ToList()
    // Creating our list that we will eventually return at the end of this function
    List<AlbumDto> result = new List<AlbumDto>(){};

    // Looping through the albums list from the database
    foreach(Album album in albums)
    {
        // Creating our album DTO, and converting the data from our original album model into our DTO
        AlbumDto newAlbumDto = new AlbumDto(){AlbumId=album.AlbumId, Name=album.Name};

        // There is one piece of data missing so far, which is the list of song names. So lets get that.

        // Creating an empty list, so we can push our song names to them. It is a list of strings because that is that datatype we defined in our initial AlbumDto class
        List<string> songNames = new List<string>(){};
        // Looping through all the songs in an album
        foreach(Song song in album.Songs)
        {
            // Pushing the song name to our songNames list
            songNames.Add(song.Name);
        }

        // Now that we have our list of song names, we want to add that to our AlbumDto
        newAlbumDto.Songs = songNames;

        // Our AlbumDto has now been assembled with all the data we need, so we add it to our result list
        result.Add(newAlbumDto);
    }

    // After the loop completes, all our albums should now be converted, so we can return our initial result list that we created
    return result

}
```

The result of this should look something like this:

```json
[
    {
        "albumName": "Illmatic",
        "albumId": 1,
        "songs": [
            "NY State of Mind", "The World Is Yours", "Memory Lane"...
        ]
    },
    "albumName": "Age Of Winters",
        "albumId": 2,
        "songs": [
            "Celestial Crown", "Bareal's Blade", "Freya"...
        ]
    },
]

```

## Why use one over the other?

Both JSON Ignore and Data Transfer Objects are ways to get around infinite serialization. If you want a simple solution to just ignore nested data that may not be relevant, then JSON Ignore is the way to go. Data Transfer Objects are a bit more complex, but they offer way more flexibility than using JSON Ignore. If you want to keep parts of data from your nested objects, than use a Data Transfer Object.

These are two ways to solve the same issue, and there are many other approaches you can take. Ultimately it's up to you to decide what works best in the context of your app.

## Key Parts in Each Example:

### JSON Ignore example:

    - Models/Animal.cs => Where JSON ignore is used
    - Controllers/LocationsController => Including Animals in the Locations query result

### Data Transfer Object Example:

    -  DataTransferObjects/AnimalDto => Reshaping our Animal data into an object with relevant data
    - DataTransferObject/LocationResultDto => Reshaping our Location data into an object with relevant data, and converting all Animals of a location into AnimalDtos
    - Controller/LocationsController => Converting Locations into LocationResultDtos
