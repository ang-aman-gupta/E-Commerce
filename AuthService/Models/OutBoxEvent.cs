namespace AuthService.Models
{
    public class OutBoxEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string EventType { get; set; } // e.g., "UserCreated"
        public string Payload { get; set; } // JSON data (User info)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool Processed { get; set; } = false; // Mark event as processed
    }
}
