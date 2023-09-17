using TriboDavi.Domain.Enum;

namespace TriboDavi.Domain
{
    public class Graduation : BaseEntity
    {
        public required string Name { get; set; }
        public required string Url { get; set; }
        public required int Position { get; set; }
        public required GraduationType GraduationType { get; set; }

        public string GetUrl()
        {
            if (Url.Contains("tribo-davi-hom")) return Url;
            else return Url.Replace("tribo-davi", Environment.GetEnvironmentVariable("GCS_BUCKET_NAME"));
        }
    }
}
