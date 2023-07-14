using Microsoft.AspNetCore.Mvc;
using Amazon;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace MVCFlowerShop.Controllers
{
    public class SNSExampleController : Controller
    {
        private const string topicARN = "arn:aws:sns:us-east-1:575031580801:SNSExample_TP055396";

        // Func 1: Learn how to get the keys from the appsettings json file
        private List<string> getKeysInformation()
        {
            List<string> keys = new List<string>();

            var builder = new ConfigurationBuilder().
                SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");   // you can also specify other json file here in the future

            IConfiguration configure = builder.Build();
            keys.Add(configure["Values:Key1"]);
            keys.Add(configure["Values:Key2"]);
            keys.Add(configure["Values:Key3"]);

            return keys;
        }

        // Func 2: Subscribe Newsletter page for customer
        public IActionResult Index()
        {
            return View();
        }

        // Func 3: To process newsletter subscription
        public async Task<IActionResult> processSubscription(string email) {
            // 1 - Add on credentials to attach to the account
            List<string> keys = getKeysInformation();

            // 2- Setup an agent to help communicate with sns
            AmazonSimpleNotificationServiceClient snsAgent = new AmazonSimpleNotificationServiceClient(keys[0], keys[1], keys[2], RegionEndpoint.USEast1);

            // 3- Create subscription request, check subscription response, handle error
            try
            {
                SubscribeRequest request = new SubscribeRequest
                {
                    TopicArn = topicARN,
                    Protocol = "Email",
                    Endpoint = email
                };
                SubscribeResponse response = await snsAgent.SubscribeAsync(request);
                ViewBag.subscriptionSuccessID = response.ResponseMetadata.RequestId;
                return View();
            }
            catch (AmazonSimpleNotificationServiceException ex)
            { 
                return BadRequest(ex.Message);
            }
        }

        // Func 4: Provide a form for admin broadcast message
        public IActionResult AdminBroadcastMessage() {
            return View();
        }

        // Func 5: Send broadcast to subscriber email
        public async Task<IActionResult> processBroadcast(string subjectTitle, string messageBody) {
            // 1 - Add on credentials to attach to the account
            List<string> keys = getKeysInformation();

            // 2- Setup an agent to help communicate with sns
            AmazonSimpleNotificationServiceClient snsAgent = new AmazonSimpleNotificationServiceClient(keys[0], keys[1], keys[2], RegionEndpoint.USEast1);

            try
            {
                PublishRequest request = new PublishRequest
                {
                    TopicArn = topicARN,
                    Subject = subjectTitle,
                    Message = messageBody,
                };
                await snsAgent.PublishAsync(request);
                return Content("Email sent to all your customers");
            }
            catch (AmazonSimpleNotificationServiceException ex) { 
                return BadRequest(ex.Message);
            }
        }


    }
}
