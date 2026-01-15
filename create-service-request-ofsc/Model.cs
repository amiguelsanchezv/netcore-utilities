namespace CreateServiceRequestOFSC
{
    public class ServiceRequestInput
    {
        public int activityId { get; set; }
        public string? requestType { get; set; }
    }

    public class ServiceRequestRequest
    {
        public int activityId { get; set; }
        public string? requestType { get; set; }
        public string? date { get; set; }
    }

    public class ServiceRequestResponse
    {
        public int? requestId { get; set; }
        public string? resourceId { get; set; }
        public int? resourceInternalId { get; set; }
        public int? activityId { get; set; }
        public int? inventoryId { get; set; }
        public string? date { get; set; }
        public string? created { get; set; }
        public string? requestType { get; set; }
    }

    public class ServiceRequestResult
    {
        public int activityId { get; set; }
        public string? requestType { get; set; }
        public int? requestId { get; set; }
        public string? status { get; set; }
        public string? errorMessage { get; set; }
        public string? created { get; set; }
    }

    public class ErrorResponse
    {
        public string? type { get; set; }
        public string? title { get; set; }
        public string? status { get; set; }
        public string? detail { get; set; }
    }
}
