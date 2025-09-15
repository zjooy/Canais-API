using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;

namespace Canais.Application.Service;

public class SqsService : ISqsService
{
    private readonly IAmazonSQS _sqsClient;
    private readonly string _queueUrl;

    public SqsService(IAmazonSQS sqsClient, IConfiguration config)
    {
        _sqsClient = sqsClient;
        _queueUrl = config["AWS:QueueUrl"];
    }

    public async Task EnviarReclamacaoAsync(string reclamacao)
    {
        var request = new SendMessageRequest
        {
            QueueUrl = _queueUrl,  
            MessageBody = reclamacao
        };

        await _sqsClient.SendMessageAsync(request);
    }
}
