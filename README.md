# BlazorMud
An _experimental_ [MUD](https://en.wikipedia.org/wiki/MUD) made with [.NET Core 3.0](https://docs.microsoft.com/dotnet/core/) and [Blazor](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor).

This project is a playground for experimenting, in very early stages and extremely work in progress. Expect stuff to change a lot. There is no guarantee that this project will evolve into something useful or end up being complete.

# Requirements
## For running
- .NET Core 3.0
- [docker](https://www.docker.com/) and up-to-date version of [docker-compose](https://docs.docker.com/compose/)
## For development
- .NET Core 3.0 SDK
- docker and up-to-date version of docker-compose
- [Visual Studio 2019](https://visualstudio.microsoft.com/vs) _or_ [Visual Studio Code](https://code.visualstudio.com/) _or_ [JetBrains Rider](https://www.jetbrains.com/rider/)

# Installation for Linux
1. Clone the repository
2. Open a terminal in the cloned folder
3. Use `docker build . -t blazormud` to build the BlazorMud docker image
4. Use `docker-compose up` to run the image and its dependencies
5. Open `localhost:8080` in your webbrowser to access Adminer
    1. System: `PostgreSQL`
    2. Server: `db`
    3. Username: `blazormud`
    4. Password: `blazormudpwd`
    5. Database: `blazormud`
6. Click Import, click file select, select the `database.sql` file inside the cloned folder and click execute
7. Open `localhost:8000` in your webbrowser to access BlazorMud
