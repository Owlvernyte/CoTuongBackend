﻿using CoTuongBackend.Domain.Entities.Games;

namespace CoTuongBackend.API.Hubs;

public interface IGameHubClient
{
    Task Joined(string message);
    Task Left(string message);
    Task Moved(Coordinate source, Coordinate destination, bool turn);
}
