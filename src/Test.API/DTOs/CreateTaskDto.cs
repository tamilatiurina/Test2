using System.ComponentModel.DataAnnotations;

namespace Test.API.DTOs;

public class CreateTaskDto
{
    [Length(1,100)]
    public string Name { get; set; }
    
    [Length(1,2000)]
    public string Description { get; set; }
}