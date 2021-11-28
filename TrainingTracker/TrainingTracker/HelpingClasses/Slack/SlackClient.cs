using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;

namespace TrainingTracker.HelpingClasses.Slack
{
    public class SlackClient
    {
        private readonly Uri _uri;
        private readonly Encoding _encoding = new UTF8Encoding();

        public SlackClient(string urlWithAccessToken)
        {
            _uri = new Uri(urlWithAccessToken);
        }

        //Post a message using simple strings  
        public void PostMessage(string text, string username = null, string channel = null)
        {
            Payload payload = new Payload()
            {
                Channel = channel,
                Username = username,
                Text = text
            };

            PostMessage(payload);
        }

        //Post a message using a Payload object  
        public void PostMessage(Payload payload)
        {
            string payloadJson = JsonConvert.SerializeObject(payload);

            using (WebClient client = new WebClient())
            {
                NameValueCollection data = new NameValueCollection();
                data["payload"] = payloadJson;

                    var response = client.UploadValues(_uri, "POST", data);

                //The response text is usually "ok"  
                string responseText = _encoding.GetString(response);
            }
        }


        public void slackAdapter(List<string> text, List<string> username=null,string channel=null)
        {
            if(channel!=null)
            {
                if(text.Count>0)
                {
                    PostMessage(username: "Zuptu Bot",
                       text: text[0],
                       channel: "#"+channel);
                }
                return;
            }
            else
            {
                if(text.Count>0)
                {
                    if (username != null)
                    {
                        if (text.Count == username.Count)
                        {
                            for (int i = 0; i < text.Count; i++)
                            {
                                PostMessage(username: "Zuptu Bot",
                                text: text[i],
                                channel: "@" + username[i]);
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < text.Count; i++)
                        {
                            PostMessage(username: "Zuptu Bot",
                            text: text[0],
                            channel: "@" + username[i]);
                        }
                    }
                    
                }


            }
        }
    }

    //This class serializes into the Json payload required by Slack Incoming WebHooks  
    public class Payload
    {
        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }
    }
    
}