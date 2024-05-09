using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace HuntflowAPI
{
    public class HuntflowWebhooks
    {
        public List<WebhookItem> items { get; set; }

        public HuntflowWebhooks()
        {
            items = new List<WebhookItem>();
        }

        public HuntflowWebhooks(string json)
        {
            JsonConvert.PopulateObject(json, this, NullableConverter.GetSettings());
        }
    }

    public class WebhookItem
    {
        public int id { get; set; }
        public int account { get; set; }
        public Uri url { get; set; }
        public DateTime created { get; set; }
        public bool active { get; set; }
        public List<WebhookEvent> webhookEvents { get; set; }
    }

    public enum WebhookEvent
    {
        APPLICANT,
        VACANCY,
        RESPONSE,
        OFFER,
        VACANCY_REQUEST,
        RECRUITMENT_EVALUATION,
        SURVEY_QUESTIONARY
    }
}