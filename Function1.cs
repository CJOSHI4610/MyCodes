using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Kafka;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;

namespace kafkatriggerconsumer
{
    public class Function1
    {
        // KafkaTrigger sample 
        // Consume the message from "topic" on the LocalBroker.
        // Add `BrokerList` and `KafkaPassword` to the local.settings.json
        // For EventHubs
        // "BrokerList": "{EVENT_HUBS_NAMESPACE}.servicebus.windows.net:9093"
        // "KafkaPassword":"{EVENT_HUBS_CONNECTION_STRING}
        //kafka flow nuget
        [FunctionName("Function1")]
        public void Run(
            [KafkaTrigger("pkc-7prvp.centralindia.azure.confluent.cloud:9092",
                          "CJVN",
                          Username = "WYXG2LN425GX27GM",
                          Password = "tslT1bRLJKAL5244Pxqpgp3KBcFJ0rQ/+aAgKXFXXUWM8F/p18dRfvL/QvlNIeqn",
                          Protocol = BrokerProtocol.SaslSsl,
                          AuthenticationMode = BrokerAuthenticationMode.Plain,
                         
                          ConsumerGroup = "$Default")] KafkaEventData<string>[] events,
            ILogger log)
        {

            var serviceBusConnectionString = "Endpoint=sb://sb-training-blob.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=a5+gTtrHcnJSldxfAi6aZ9ROhXtDEf8Fz+ASbNHBf4U=";
            var serviceBusTopicName = "kafkacjvn";

            foreach (KafkaEventData<string> eventData in events)
            {
                log.LogInformation($"C# Kafka trigger function processed a message: {eventData.Value}");

                var order = JsonConvert.DeserializeObject<Order>(eventData.Value);


                if (order.orderid % 2 == 0)
                {
                    var topicClient = new TopicClient(serviceBusConnectionString, serviceBusTopicName);
                    var messageBody = new Message(Encoding.UTF8.GetBytes(eventData.Value));

                    messageBody.UserProperties.Add("Source", "Client1");

                    topicClient.SendAsync(messageBody);
                }

               
            }
           
        }

        public class Order
        {
            public long ordertime { get; set; }
            public int orderid { get; set; }
            public string itemid { get; set; }
            public double orderunits { get; set; }
            public Address address { get; set; }
        }

        
        public class Address
        {
            public string city { get; set; }
            public string state { get; set; }
            public int zipcode { get; set; }
        }


    }
}
