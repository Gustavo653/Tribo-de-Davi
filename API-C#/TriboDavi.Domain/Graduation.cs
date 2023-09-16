namespace TriboDavi.Domain
{
    public class Graduation : BaseEntity
    {
        public required string Name { get; set; }
        public required string Url { get; set; }
        public required int Position { get; set; }

        public string GetUrl()
        {
            return Url.Replace("tribo-davi", Environment.GetEnvironmentVariable("GCS_BUCKET_NAME"));
        }
    }
}
