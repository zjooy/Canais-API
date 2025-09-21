using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SQS;
using Canais.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Canais.Application.Service;

public class BucketService : IBucketService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;

    public BucketService(IAmazonS3 s3Client, IConfiguration configuration)
    {
        _s3Client = s3Client;
        _bucketName = configuration["AWS:BucketName"];
    }

    public async Task<string> SalvarArquivoAsync(IFormFile arquivo, string nomeReclamante, string identificacaoReclamacao)
    {
        var nomeArquivo = $"{identificacaoReclamacao}/{nomeReclamante} - {arquivo.FileName}";

        using var stream = arquivo.OpenReadStream();

        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = nomeArquivo,
            InputStream = stream,
            ContentType = arquivo.ContentType
        };

        await _s3Client.PutObjectAsync(request);

        return nomeArquivo;
    }
}
