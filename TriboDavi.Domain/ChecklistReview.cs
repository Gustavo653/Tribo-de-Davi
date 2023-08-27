using TriboDavi.Domain.Enum;
using TriboDavi.Domain.Identity;

namespace TriboDavi.Domain
{
    public class ChecklistReview : BaseEntity
    {
        public ReviewType Type { get; set; }
        public string Observation { get; set; }
        public virtual Ambulance Ambulance { get; set; }
        public virtual Checklist Checklist { get; set; }
        public virtual User User { get; set; }
    }
}
