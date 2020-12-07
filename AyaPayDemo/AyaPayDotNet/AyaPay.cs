using Newtonsoft.Json;
using RestSharp;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace AyaPayDotNet
{
    public class AyaPay
    {
        //TODO: "dev" or "pro" to switch between UAT vs. Production environment
        public const AYAPAY_ENV CURRENT_ENV = AYAPAY_ENV.DEV;

        public enum AYAPAY_ENV { DEV, PRO}

        //TODO: Base URL - update with live URL endpoints to go live
        public const string BASE_URL_DEV = "https://opensandbox.ayainnovation.com";
        public const string BASE_URL_PRO = "https://api.ayapay.com";

        //TODO: update with API key and secret
        public const string CONSUMER_KEY = "xxxxxxx";
        public const string CONSUMER_SECRET = "xxxxxx";

        //TODO: update with merchant account credentials
        public const string MERCHANT_MOBILE = "xxxxxx";
        public const string MERCHANT_PIN = "xxxxxx";

        //TODOD: update key to decrypt the response received via callback
        public const string DECRYPTION_KEY = "xxxxxx";

        public const string BASE_URL_RES = "/merchant/1.0.0";

        public const string REQ_ACCESS_TOKEN = "/token";
        public const string REQ_USER_TOKEN = "/thirdparty/merchant/login";
        public const string REQ_PUSH_PAYMENT = "/thirdparty/merchant/requestPushPayment";

        public static AccessToken AccessToken;
        public static string latestAccessToken;

        public static string DecryptPaymentResponse(string paymentResult)
        {
            byte[] iv = new byte[16];
            byte[] buffer = Convert.FromBase64String(paymentResult);

            using (Aes aes = Aes.Create())
            {
                aes.Padding = PaddingMode.PKCS7;
                aes.Mode = CipherMode.ECB;
                aes.Key = Encoding.UTF8.GetBytes(DECRYPTION_KEY);
                aes.IV = iv;
                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (MemoryStream memoryStream = new MemoryStream(buffer))
                {
                    using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                        {
                            return streamReader.ReadToEnd();
                        }
                    }
                }
            }
        }

        public static void RefreshAccessToken()
        {
            string baseUrl = CheckEnvironment();

            var keyBytes = Encoding.UTF8.GetBytes(CONSUMER_KEY + ":" + CONSUMER_SECRET);
            string base64KeySecret = Convert.ToBase64String(keyBytes); 

            var client = new RestClient(baseUrl + REQ_ACCESS_TOKEN);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);

            request.AddHeader("Authorization", "Basic " + base64KeySecret);
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("grant_type", "client_credentials");
            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                AccessToken = JsonConvert.DeserializeObject<AccessToken>(response.Content);
                latestAccessToken = AccessToken.access_token;
            }
        }

        public static UserTokenResponse GetUserToken()
        {
            string baseUrl = CheckEnvironment();

            var client = new RestClient();
            client.BaseUrl = new Uri(baseUrl + BASE_URL_RES + REQ_USER_TOKEN);
            client.Timeout = -1;
            var request = new RestRequest(Method.POST);

            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddHeader("Token", "Bearer " + latestAccessToken);
            request.AddParameter("phone", MERCHANT_MOBILE);
            request.AddParameter("password", MERCHANT_PIN);

            IRestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                UserTokenResponse userTokenResponse = JsonConvert.DeserializeObject<UserTokenResponse>(response.Content);
                return userTokenResponse;
            }
            else
            {
                return null;
            }
        }

        private static string CheckEnvironment()
        {
            if (CURRENT_ENV == AYAPAY_ENV.PRO)
            {
                return BASE_URL_PRO;
            }
            else
            {
                return BASE_URL_DEV;
            }
        }

        public static PaymentResponse RequestPushPayment(string customerPhone, int amount, string externalTransactionId, string externalAdditionalData)
        {
            UserTokenResponse userTokenResponse = GetUserToken();

            if (userTokenResponse == null)
            {
                //access token could be expired, try requesting access token again
                RefreshAccessToken();

                //request user token again
                userTokenResponse = GetUserToken();
            }

            string baseUrl = CheckEnvironment();

            var client = new RestClient();
            client.BaseUrl = new Uri(baseUrl + BASE_URL_RES + REQ_PUSH_PAYMENT);
            var requestPay = new RestRequest(Method.POST);

            requestPay.AddHeader("Token", "Bearer " + latestAccessToken);
            requestPay.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            requestPay.AddHeader("Authorization", "Bearer " + userTokenResponse.Token.Token);
            requestPay.AddParameter("customerPhone", customerPhone);
            requestPay.AddParameter("amount", amount);
            requestPay.AddParameter("currency", "MMK");
            requestPay.AddParameter("externalTransactionId", externalTransactionId);
            requestPay.AddParameter("externalAdditionalData", externalAdditionalData);

            IRestResponse responsePay = client.Execute(requestPay);

            PaymentResponse paymentResponse = JsonConvert.DeserializeObject<PaymentResponse>(responsePay.Content);

            return paymentResponse;
        }
    }
}
