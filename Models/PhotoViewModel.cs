namespace Website.Models;
public class PhotoViewModel
{
    public PhotoEntry CurrentPhoto { get; set; } = new();
    public PhotoEntry? PreviousPhoto { get; set; }
    public PhotoEntry? NextPhoto { get; set; }
}
