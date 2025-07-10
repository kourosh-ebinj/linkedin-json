
using JSONPath.Services;

namespace JSONPath;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddHttpContextAccessor();

        builder.Services.AddControllers();
        builder.Services.AddSingleton<IDataFile,DataFile>();
        builder.Services.AddSingleton<IBlobDataFile, BlobDataFile>();

        var app = builder.Build();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();

    }
}
