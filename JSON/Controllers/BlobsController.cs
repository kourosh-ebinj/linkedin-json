using JSONPath.Helpers;
using Microsoft.AspNetCore.Mvc;
using JSONPath.Services;

namespace JSONPath.Controllers;

[ApiController]
[Route("[controller]")]
public class BlobsController : ControllerBase
{
    private readonly IBlobDataFile _blobDataFile;
    private readonly IDataFile _dataFile;

    public BlobsController(IBlobDataFile blobDataService, IDataFile dataFile)
    {
        _blobDataFile = blobDataService;
        _dataFile = dataFile;
    }

    [HttpGet("lookup")]
    public async Task<IActionResult> Lookup(CancellationToken cancellationToken = default)
    {
        var jsonData = _dataFile.GetData();
        var age = JsonHelper.GetPrimitiveValueByPath<int>(jsonData, "$[?(@._id == '1')].age");
        var friends1 = JsonHelper.GetItemsByPath<string>(jsonData, "$[?(@._id == '1')].friends[*].name");
        var friends2 = JsonHelper.GetItemsByPath<string>(jsonData, "$[?(@._id == '2')].friends[*].name");
        Console.WriteLine(age);
        Console.WriteLine(string.Join(", ", friends1));
        Console.WriteLine(string.Join(", ", friends2));

        var blobJsonData = _blobDataFile.GetData();
        var width = JsonHelper.GetPrimitiveValueByPath<int>(blobJsonData, "$.imgs[?(@.id == 'a4ea732cd3d5948a')].width");
        var bboxes = JsonHelper.GetItemsByPath<decimal>(blobJsonData, "$.anns[?(@.id == 'a4ea732cd3d5948a_1')].bbox.*");
        Console.WriteLine(width);
        Console.WriteLine(string.Join(", ", bboxes));

        return Ok();
    }
}
