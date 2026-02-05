public sealed class InputformRequest
{
    public string first_name { get; set; } = "";
    public string last_name { get; set; } = "";
    public string email { get; set; } = "";
    public string phone { get; set; } = "";
    public string occupation { get; set; } = "";
    public string sex { get; set; } = "";
    public string birth_day { get; set; } = "";

    public IFormFile profile { get; set; } = default!;
}
