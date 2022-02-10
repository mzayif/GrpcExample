using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace grpcServer
{
    public class MessageService : Message.MessageBase
    {
        private readonly ILogger<GreeterService> _logger;
        private List<GetMessageModel> getMessages = new();

        public MessageService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }


        public override Task<MessageResponse> SendMessage(MessageRequest request, ServerCallContext context)
        {
            System.Console.WriteLine("Request Name:" + request.UserName);
            System.Console.WriteLine("Request Message:" + request.Message);
            getMessages.Add(new GetMessageModel { UserName = request.UserName, Message = request.Message });

            return Task.FromResult(new MessageResponse
            {
                Message = "Hello " + request.UserName + ". Message is accept"
            });
        }


        public override Task<MessageCountResponse> GetUserMessageCount(MessageCountRequest request, ServerCallContext context)
        {
            System.Console.WriteLine("Start " + nameof(GetUserMessageCount) + " methot");
            System.Console.WriteLine("Request Name:" + request.UserName);
            System.Console.WriteLine(getMessages.Count());

            return Task.FromResult(new MessageCountResponse
            {
                SendMessageCount = getMessages.Where(x => x.UserName == request.UserName).Count()
            });
        }

        public override async Task GetUserMessages(UserMessagesRequest request, IServerStreamWriter<UserMessagesResponse> responseStream, ServerCallContext context)
        {
            System.Console.WriteLine("Start " + nameof(GetUserMessages) + " methot");
            System.Console.WriteLine("Request Name:" + request.UserName);

            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(1000);
                await responseStream.WriteAsync(new UserMessagesResponse{
                    Message = i+". Sıradaki Mesaj"
                });
            }
            //return base.GetUserMessages(request, responseStream, context);
        }

        public override async Task<MessageResponse> SendMessages(IAsyncStreamReader<MessageRequest> requestStream, ServerCallContext context)
        {

            while (await requestStream.MoveNext(context.CancellationToken))
            {
                System.Console.WriteLine($"Message: {requestStream.Current.Message} | User Name: {requestStream.Current.UserName}");
            }

            return new MessageResponse
            {
                Message = "Mesajların tamamı alınmıştır."
            };
        }
    }
}
