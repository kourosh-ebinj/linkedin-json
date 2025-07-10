using JSONPath.Helpers;

namespace JSONPath.Services
{
    public interface IBlobDataFile : IDataFile
    {

    }

    public class BlobDataFile : IBlobDataFile
    {
        // As the file 266 MB, it is not included in the repository.
        // File Url: https://storage.googleapis.com/kaggle-data-sets/2327240/3919937/compressed/TextOCR_0.1_train.json.zip?X-Goog-Algorithm=GOOG4-RSA-SHA256&X-Goog-Credential=gcp-kaggle-com%40kaggle-161607.iam.gserviceaccount.com%2F20250703%2Fauto%2Fstorage%2Fgoog4_request&X-Goog-Date=20250703T071040Z&X-Goog-Expires=259200&X-Goog-SignedHeaders=host&X-Goog-Signature=55eb8ee4ca74e493b1e6a402095f4ef39cb25b0494a4294d2169f0ca4e5cee0827f2a0d338b16d5341c1ce4aa46f1246f314886862577fcf31896bcf77401909a166e327abbebe7b5c5f2312571ff05da727a1cc13ec6520416e6dd4201b899fe6c9f1d06c678d36ece9e8e9cd540869aa3b8a10322402b76452b2b8bb2d63e739ca3f732708c176095fd6cf0c9322cd0a0aa300d818913b1d49dbfe3e6b15debb10730ba5a9d7beca0a836f1fce5c137155d8aec220f535cb90be73a454ea8ae05e445023653179b61a4d650c906673179a47dace08bdc1c99d98dfa19a1a2e00b50cc4caa5322e908813a573a176151d15d4581960f012384d316ddcd8e799
        public string FilePath => "Data\\TextOCR_0.1_train.json";
        public string GetData()
        {
            return FileHelper.LoadJsonFile(FilePath);
        }
    }
}
