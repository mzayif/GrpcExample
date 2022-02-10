using System;
using Grpc.Net.Client;
using grpcServer;

namespace grpcClient
{
    class Program
    {
        static void Main(string[] args)
        {
           var chanel = GrpcChannel.ForAddress("https://localhost:5001");
           //var greetClient = new Greeter.GreeterClient(chanel);
           var messageClient = new Message.MessageClient(chanel);

        //   var response = greetClient.SayHello(new HelloRequest{
        //        Name="Mahmut"
        //    }) ;

        //    System.Console.WriteLine(response.Message);
    
          var messageresponse = messageClient.SendMessage(new MessageRequest{
               UserName="Father",
               Message="Okey. Good by"
           }) ;

           System.Console.WriteLine(messageresponse.Message);
        
            var messageCountResponse = messageClient.GetUserMessageCount(new MessageCountRequest{
               UserName="Father"
           }) ;

           System.Console.WriteLine("Father Message Count : "+messageCountResponse.SendMessageCount);
        }
    }
}
