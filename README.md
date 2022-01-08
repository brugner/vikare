<p align="center">
  <a href="" rel="noopener">
 <img width=200px height=200px src="https://www.pinclipart.com/picdir/big/524-5244832_building-videoscribe-clip-art-airport-clipart-png-transparent.png" alt="Project logo"></a>
</p>

<h3 align="center">Vikare</h3>

<div align="center">

  [![Status](https://img.shields.io/badge/status-active-success.svg)]() 
  [![GitHub Issues](https://img.shields.io/github/issues/brugner/vikare.svg)](https://github.com/brugner/vikare/issues)
  [![GitHub Pull Requests](https://img.shields.io/github/issues-pr/brugner/vikare.svg)](https://github.com/brugner/vikare/pulls)
  [![License](https://img.shields.io/badge/license-MIT-blue.svg)](/LICENSE)

</div>

---

## Table of Contents
- [About](#about)
- [Getting started](#getting_started)
- [Usage](#usage)
- [Built using](#built_using)
- [Contributing](#contributing)
- [Authors](#authors)
- [Acknowledgments](#acknowledgement)

## About <a name = "about"></a>
This project is a simple API that serves data about airports like name, coordinates and international codes. It was meant to be a quick look into the new features of .NET 6 and C# 10, mainly the Minimal API approach. It was also a pet project to check that my new dev environment was working correctly.

## Getting Started <a name = "getting_started"></a>
These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Prerequisites
Before you begin, make sure you have git and .NET 6 installed on your system.

### Installing
Clone the repo

```
git clone https://github.com/brugner/vikare.git
```

Go to the API project folder
```
cd vikare/src/Vikare.API/
```

Ran the API project

```
dotnet run
```

Open a browser, Postman or any other HTTP client and run one of the [requests](#usage).

## Running the tests <a name = "tests"></a>
Use a console to go to ./tests/Vikare.Tests/ and run this command

```
dotnet test
```

## Usage <a name="usage"></a>
### Get all airports
```
GET /api/airports?page=1&pageSize=10&search=greece&excludeMetadata=false
```
- Returns a list of airports.
- page: 1 if not specified.
- pageSize: range between 1 and 50.
- search: optional, it searches by name, city and country.
- excludeMetadata: false by default.

### Get by Id
```
GET /api/airports/{id}
```
- Returns an airport, null if not found.

### Get airports by distance
```
GET /api/airports/[closest | farthest]?lat=50&lng=-50&count=10&unit=k
```
- Returns a list of the closest or farthest airports relative to a point.
- lat: latitude of the point being compared to.
- lng: longitude of the point being compared to.
- count: amount of entries asked ranged between 1 and 50.
- unit: unit of measure. k: kilometers, m: miles, n: nautical miles.

## Built Using <a name = "built_using"></a>
- [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) - Rest API
- [xUnit](https://xunit.net/) - Unit testing

## Contributing
Feel free to submit a pull request or open an issue.
## Authors <a name = "authors"></a>
- [@brugner](https://github.com/brugner) - Idea & initial work

## Acknowledgements <a name = "acknowledgement"></a>
- The dataset was compiled by [Jonatan Cisneros](https://www.kaggle.com/jonatancr) and published on [Kaggle](https://www.kaggle.com/jonatancr/airports).
- Project image from [Pinclipart](https://www.pinclipart.com/maxpin/iTxoiiR/).