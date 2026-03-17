namespace Flowdesks.Application.Responses.PPMs
{
    public class FrequencyColorResponse
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
        public int Order { get; set; }
        public string Name { 
            get
            {
                return this.Title;
            }
        }
    }
}
