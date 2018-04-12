#r "Microsoft.Azure.Documents.Client"
using System;
using System.Collections.Generic;
using Microsoft.Azure.Documents;
using System.Net;
using System.Text;

public static void Run(IReadOnlyList<Document> documents, TraceWriter log)
{
    if (documents != null && documents.Count > 0)
    {
        log.Verbose("Documents modified " + documents.Count);
        log.Verbose("First document Id " + documents[0].Id);
        EventPub ep = new EventPub(log);
        ep.Publish();
    }
}
public class GridEvent<T> where T : class
{
    public string Id { get; set; }
    public string Subject { get; set; }
    public string EventType { get; set; }
    public T Data { get; set; }
    public DateTime EventTime { get; set; }
}
public class EventPub
{
    private TraceWriter log;
    public EventPub(TraceWriter log)
    {
        this.log = log;
    }
    public void Publish()
    {
        this.log.Verbose("Publish invoked!!");   
        string topicEndpoint = "<even-grid-topic-endpoint>";
        string sasKey = "<sas-key>";

        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("aeg-sas-key", sasKey);
        client.DefaultRequestHeaders.UserAgent.ParseAdd("democlient");

        List<GridEvent<object>> eventList = new List<GridEvent<object>>();
    for (int x = 0; x < 5; x++)
    {
        GridEvent<object> testEvent = new GridEvent<object>
        {
            Subject = $"Event {x}",
            EventType = (x % 2 == 0) ? "allEvents" : "filteredEvent",
            EventTime = DateTime.UtcNow,
            Id = Guid.NewGuid().ToString()
        };
        eventList.Add(testEvent);
    }

    string json="{}";
    //string json = JsonConvert.SerializeObject(eventList);
    HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, topicEndpoint)
    {
        Content = new StringContent(json, Encoding.UTF8, "application/json")
    };

    //HttpResponseMessage response = await client.SendAsync(request);

    }

}
