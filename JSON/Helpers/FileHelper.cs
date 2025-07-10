using System.Text;

namespace JSONPath.Helpers;

public static class FileHelper
{

    public static string LoadJsonFile(string filepath)
    {

        return File.ReadAllText(filepath, Encoding.UTF8);
    }


}
