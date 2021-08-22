# CrewDemandService

## Technologies Used:

### Api:

1. .Net 5.0 Framework
2. In-Memory Sql
3. Swagger

### Unit Testing:
1. xUnit
2. Moq
3. AutoFixture

## How to run?

Use Visual Studio 2019 or Rider IDE and clone the master branch of this repository.


## Database

During the Startup, data from [pilot.json](https://github.com/mkiruba/CrewDemandService/blob/master/CrewDemandService/Infrastructure/Data/Pilot.json) is inserted in to in-memory database.

![db diagram](image.jpg)

## Api

Followed CQRS to segregate Command and Queries.

![pilots sequence diagram](image.jpg)

![flights sequence diagram](image.jpg)
