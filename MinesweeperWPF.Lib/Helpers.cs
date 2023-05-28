using System.IO;
namespace MinesweeperWPF.Lib;

internal static class Helpers
{
    private static string GetResourceFullPath(string name) => Path.GetFullPath(Path.Combine("Resources", name));

    /// <summary>
    ///     Возвращает Uri ресурса
    /// </summary>
    /// <param name="name">Название ресурса</param>
    /// <returns>Uri ссылка</returns>
    internal static Uri GetResourceUri(string name) => new(GetResourceFullPath(name), UriKind.Absolute);
}