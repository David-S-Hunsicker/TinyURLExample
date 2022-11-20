# TinyURLExample


## Design

Generally we want our program to have the appearance of a rest controller which passes requests to the business logic layer, then persistence / data etc. I stubbed out validation for input and left the trace idea of userId in the data that stores our entities but didn't really cover them

Program.cs isn't used because there are unit tests to cover most use cases but it does have functions in it that take a form similar to a rest controller.

The resolver has caching because we definitely don't want our db getting hit on every request. Because this is just an example and not a real service we don't have any kind of real message broker that the cache can subscribe to all deletes. The side effect of this is the caches can store entries that have been deleted and cause undesirable behavior.


## Hashing / Mapping

If we had more time instead of the string and ulong implementation where we get incremented by one we'd use something like [0-9, a-z, A-Z]. All of those combined are 62^n where n is the length of the short url. At length 7 we're into the trillions of URL which should be enough for a while.


## Data
We went with a very simple schema here. Ideally we would not store the click metrics on the same table or maybe not on the same db as the actual entry itself. updating metrics would happen so much more than Read/Delete/Create operations it's worth looking into how to intelligently isolate those functions.

Any time we're hashing as a mechanism for storage it's worth talking about sharding. A fast approach might be 11 or so shards with the first 10 being hash % 10 and the last being a pure custom string shard. Maybe if the custom URL becomes the most desired used case or the most profitable one we expand the shards from 1 custom url to many. There are databases that are built with these kind of problems in mind.