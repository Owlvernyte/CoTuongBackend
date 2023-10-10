namespace CoTuongBackend.Domain.Helpers.Games;

public static class RoomCodeGenerator
{
    const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
    const int _codeLength = 6;
    public static string Generate()
        => RandomStringGenerator.Generate(_chars, _codeLength);
}
