using JSONPath.Helpers;

namespace JSONPath.Services
{
    public interface IDataFile
    {
        string FilePath { get; }
        string GetData();

    }

    public class DataFile: IDataFile
    {
        public string FilePath => "Data\\jsonData.json";
        public string GetData()
        {
            return FileHelper.LoadJsonFile(FilePath);
        }
    }
}
