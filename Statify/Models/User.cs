namespace Statify.Models;

using System.Collections.Generic;

public class User
{
	public string Country { get; set; }
	public string DisplayName { get; set; }
	public string Email { get; set; }
	public ExplicitContent ExplicitContent { get; set; }
	public ExternalUrls ExternalUrls { get; set; }
	public Followers Followers { get; set; }
	public string Href { get; set; }
	public string Id { get; set; }
	public List<Image> Images { get; set; }
	public string Product { get; set; }
	public string Type { get; set; }
	public string Uri { get; set; }
}

