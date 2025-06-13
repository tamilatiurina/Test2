namespace Test.API.DTOs;

public class CreateRecordDto
{
    
    public int LanguageId { get; set; }
    public int StudentId { get; set; }
    public int? TaskId { get; set; }

    public CreateTaskDto? Task { get; set; }
    
    
    public long ExecutionTime { get; set; }
    public DateTime Created { get; set; }
}