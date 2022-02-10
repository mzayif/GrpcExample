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


        public override Task<UserMessagesResponse> GetUserMessages(UserMessagesRequest request, ServerCallContext context)
        {
            System.Console.WriteLine("Start " + nameof(GetUserMessages) + " methot");
            System.Console.WriteLine("Request Name:" + request.UserName);

            return Task.FromResult(new UserMessagesResponse
            {

                //SendMessageCount = getMessages.Where(x=>x.UserName== request.UserName).Count()
            });
        }
    }
}
