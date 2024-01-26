using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RestSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TestApp.Application.Services;
using TestApp.Domain.Adapters;
using TestApp.Domain.Interfaces.Adapters;
using TestApp.Domain.Interfaces.Repositories;
using TestApp.Domain.Models.Entities;
using TestApp.Domain.Models.Events;
using TestApp.Domain.Services;

namespace TestApp.Application.Anticorruption
{
    public class ExternalClientAdapter : IExternalClientAdapter
    {
        private readonly IPublishAdapter _publishAdapter;
        private readonly IExternalClientMessageService _externalClientMessageService;

        private string deadLetterQueue = string.Empty;

        public ExternalClientAdapter(IPublishAdapter publishAdapter, IExternalClientMessageService externalClientMessageService) =>
            (_publishAdapter, _externalClientMessageService) = (publishAdapter, externalClientMessageService);

        public async Task ProcessMessage(ExternalClientEvent externalClientEvent, string queue)
        {
            deadLetterQueue = $"{queue}_deadletter";

            switch (externalClientEvent.Type)
            {
                case 1:
                    await ProcessExternalClientOne(externalClientEvent);
                    break;

                case 2:
                    await ProcessExternalClientTwo(externalClientEvent);
                    break;

                default:
                    break;
            }
        }

        private async Task ProcessExternalClientOne(ExternalClientEvent externalClientEvent)
        {
            if (externalClientEvent.User.UserId > 0)
            {
                var statusCode = await SendToClient(externalClientEvent);

                if (statusCode == System.Net.HttpStatusCode.OK)
                {
                    externalClientEvent.Type = 2;
                    await _publishAdapter.SendMessage(JsonConvert.SerializeObject(externalClientEvent));
                }
                else
                {
                    await _publishAdapter.SendMessage(JsonConvert.SerializeObject(externalClientEvent), deadLetterQueue);

                    Log.Logger.Information("Error processing ProcessExternalClientOne {object}", JsonConvert.SerializeObject(externalClientEvent));
                }
            }
        }

        private async Task ProcessExternalClientTwo(ExternalClientEvent externalClientEvent)
        {
            if (externalClientEvent.User.UserId > 0)
            {
                var statusCode = await SendToClient(externalClientEvent);

                if (statusCode == System.Net.HttpStatusCode.OK)
                {
                    var externalClientMessage = new ExternalClientMessage(externalClientEvent.User.UserId, 200);
                    await _externalClientMessageService.SaveExternalClientMessage(externalClientMessage);
                }
                else
                {
                    await _publishAdapter.SendMessage(JsonConvert.SerializeObject(externalClientEvent), deadLetterQueue);

                    Log.Logger.Information("Error processing ProcessExternalClientOne {object}", JsonConvert.SerializeObject(externalClientEvent));
                }
            }
        }

        private async Task<System.Net.HttpStatusCode> SendToClient(ExternalClientEvent externalClientEvent)
        {
            var options = new RestClientOptions("https://webhook.site")
            {
                MaxTimeout = -1,
            };

            var client = new RestClient(options);
            var request = new RestRequest("/80dffe84-6c2e-45e9-9f4f-b26859595330", Method.Post);
            request.AddHeader("Content-Type", "application/json");
            var body = JsonConvert.SerializeObject(externalClientEvent.User);
            request.AddStringBody(body, DataFormat.Json);
            var response = await client.ExecuteAsync(request);

            return response.StatusCode;
        }
    }
}
