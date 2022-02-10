using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;
using grpcServer;

namespace grpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var chanel = GrpcChannel.ForAddress("https://localhost:5001");
            //var greetClient = new Greeter.GreeterClient(chanel);
            var messageClient = new Message.MessageClient(chanel);

            //   var response = greetClient.SayHello(new HelloRequest{
            //        Name="Mahmut"
            //    }) ;

            //    System.Console.WriteLine(response.Message);

            //   var messageresponse = messageClient.SendMessage(new MessageRequest{
            //        UserName="Father",
            //        Message="Okey. Good by"
            //    }) ;

            //    System.Console.WriteLine(messageresponse.Message);

            //     var messageCountResponse = messageClient.GetUserMessageCount(new MessageCountRequest{
            //        UserName="Father"
            //    }) ;
            //    System.Console.WriteLine("Father Message Count : "+messageCountResponse.SendMessageCount);


            ///Server Streaming 

            // var response = messageClient.GetUserMessages(new UserMessagesRequest { UserName = "Elif" });
            // CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            // while (await response.ResponseStream.MoveNext(cancellationTokenSource.Token))
            // {
            //     System.Console.WriteLine($"Message: {response.ResponseStream.Current.Message}");
            // }


            ///Client Streaming
            System.Console.WriteLine("Mesaj gönderimi başladı. " + DateTime.Now);
            var request = messageClient.SendMessages();
            for (int i = 0; i < 10; i++)
            {
                await Task.Delay(1000);
                await request.RequestStream.WriteAsync(new MessageRequest
                {
                    Message = (i+1) + ". Mesaj gönderildi",
                    UserName = "Elif"
                });
            }
            System.Console.WriteLine("Mesaj gönderimi tamamlandı. " + DateTime.Now);            
            System.Console.WriteLine("===========================================");
            await request.RequestStream.CompleteAsync(); // İşlemim bitti demek
            System.Console.WriteLine("Server Cevabı : " + (await request.ResponseAsync).Message);
        }
    }
}
