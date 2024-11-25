namespace MessageContract
{
    public class MessageModel
    {
        public required string Id { get; set; }
        public required string Description { get; set; }
        public decimal? Price { get; set; }

        public MessageModel()
        {
             
        }

        public override string ToString()
        {
            return $"{Id}\t{Description}\t{Math.Round(Price.GetValueOrDefault(),2).ToString()}";
        }
    }
}
