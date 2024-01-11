namespace InvOFSC
{
    public class Invs
    {
        public int totalResults { get; set; }
        public int limit { get; set; }
        public int offset { get; set; }
        public Inv[] items { get; set; }
    }

    public class Inv
    {
        public string status { get; set; }
        public string inventoryType { get; set; }
        public int inventoryId { get; set; }
        public int activityId { get; set; }
        public float quantity { get; set; }
        public string serialNumber { get; set; }
    }
}
