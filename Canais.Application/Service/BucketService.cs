using Amazon.Runtime.Internal.Util;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.SQS;
using Canais.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net;

namespace Canais.Application.Service;

public class BucketService : IBucketService
{
    private readonly IAmazonS3 _s3Client;
    private readonly string _bucketName;
    private readonly string _bucketReclamacoesFisica;
    private readonly ILogger<ReclamacaoService> _logger;

    public BucketService(IAmazonS3 s3Client, IConfiguration configuration, ILogger<ReclamacaoService> logger)
    {
        _s3Client = s3Client;
        _bucketName = configuration["AWS:BucketName"]!;
        _bucketReclamacoesFisica = configuration["AWS:BucketReclamacoesFisica"]!;
        _logger = logger;
    }

    public async Task<bool> EnviarArquivosFisicosAsync(IFormFile arquivo, string caminhoCompleto)
    {
        try
        {
            using var stream = arquivo.OpenReadStream();

            var request = new PutObjectRequest
            {
                BucketName = _bucketReclamacoesFisica,
                Key = caminhoCompleto,
                InputStream = stream,
                ContentType = arquivo.ContentType
            };

            var response = await _s3Client.PutObjectAsync(request);

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                _logger.LogError($"Erro ao salvar arquivo no S3: {caminhoCompleto}");
                return false;
            }

            _logger.LogInformation($"Anexos físicos enviados para S3 em canais-reclamacoes-fisica");
            return true;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Erro ao salvar arquivo no S3: {caminhoCompleto}");
            return false;
        }
    }

    public async Task<string> SalvarArquivoAsync(IFormFile arquivo, string nomeReclamante, int identificacaoReclamacao)
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
