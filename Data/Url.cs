using System.ComponentModel.DataAnnotations;

namespace DotNetMVC_Task.Data;

public class Url {
    public int Id { get; set; }
    [Required]
    public string Token { get; set; }
    [Required]
    public string LongUrl { get; set; }
    public DateTime CreatedAt {  get; set; }
}