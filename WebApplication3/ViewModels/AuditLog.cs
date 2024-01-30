using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication3.ViewModels
{
    public class AuditLog
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string Action { get; set; }
    }
}
