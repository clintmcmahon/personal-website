namespace Website.Models;
using System.Collections.Generic;

public class PhotoViewModel
{
    public PhotoEntry CurrentPhoto { get; set; } = new();
    public PhotoEntry? PreviousPhoto { get; set; }
    public PhotoEntry? NextPhoto { get; set; }
    public IEnumerable<PhotoComment> Comments { get; set; } = new List<PhotoComment>();
}
