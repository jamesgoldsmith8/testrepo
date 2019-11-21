using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Revature.Room.Lib;
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace ServiceBusMessaging
{
  /// <summary>
  /// This class' sole job is to serialize and send a mesesage to the queue to be verified
  /// </summary>
  public class ServiceBusSender
  {

    private readonly IConfiguration _configuration;
    private readonly QueueClient _queueClient;
    private readonly ILogger<ServiceBusSender> _logger;

    /// <summary>
    /// ServiceBusSender constructor injected with IConfiguration and ILogger
    /// </summary>
    /// <param name="configuration"></param>
    /// <param name="logger"></param>
    public ServiceBusSender(IConfiguration configuration, ILogger<ServiceBusSender> logger)
    {
      _configuration = configuration;
      _logger = logger;
      _queueClient = new QueueClient(_configuration.GetConnectionString("ServiceBus"), _configuration["Queues:TestQueue"]);
    }

    /// <summary>
    /// Method to generate and get Shared Access Signature which helps verify that you are authorized to interact with the queue
    /// </summary>
    /// <param name="resourceUri"></param>
    /// <param name="keyName"></param>
    /// <param name="key"></param>
    /// <param name="ttl"></param>
    /// <returns></returns>
    /// <remarks>Source url: https://stackoverflow.com/questions/50914924/send-msg-to-azure-service-bus-que-via-rest </remarks>
    public static string GetSasToken(string resourceUri, string keyName, string key, TimeSpan ttl)
    {
      var expiry = GetExpiry(ttl);
      string stringToSign = HttpUtility.UrlEncode(resourceUri) + "\n" + expiry;
      HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(key));
      var signature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(stringToSign)));
      var sasToken = String.Format(CultureInfo.InvariantCulture, "SharedAccessSignature sr={0}&sig={1}&se={2}&skn={3}",
      HttpUtility.UrlEncode(resourceUri), HttpUtility.UrlEncode(signature), expiry, keyName);
      return sasToken;
    }

    /// <summary>
    /// Get the time limit or expiration of the SAS token
    /// </summary>
    /// <param name="ttl"></param>
    /// <returns></returns>
    private static string GetExpiry(TimeSpan ttl)
    {
      TimeSpan expirySinceEpoch = DateTime.UtcNow - new DateTime(1970, 1, 1) + ttl;
      return Convert.ToString((int)expirySinceEpoch.TotalSeconds);
    }


    /// <summary>
    /// ServiceBus message for deleting a message
    /// </summary>
    /// <param name="roomToSend"></param>
    /// <returns></returns>
    public async Task SendDeleteMessage(Room roomToSend)
    {
      string data = JsonConvert.SerializeObject(roomToSend);

      Message message = new Message(Encoding.UTF8.GetBytes(data));

      _logger.LogInformation("ServiceBus sending delete message: ", data);
      await _queueClient.SendAsync(message);
    }
  }
}
