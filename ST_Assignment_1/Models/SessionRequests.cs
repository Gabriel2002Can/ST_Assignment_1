public class StartSessionRequest
{
    public int UserId { get; set; }
    public int CalendarEntryId { get; set; }
}

public class RecordSetRequest
{
    public int SetNumber { get; set; }
    public string ActualRepsOrDuration { get; set; }
    public int RestAfterSetSeconds { get; set; }
}
