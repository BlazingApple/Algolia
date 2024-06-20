namespace BlazingApple.Algolia.Models;

/// <summary>Facet details</summary>
public record FacetDetails
	(
	string Key,
	string Title,
	FacetDisplayType Display
	)
{
	/// <summary>Subtext to display, if any.</summary>
	public string? Description { get; set; }
}

/// <summary>How to render the facet, if at all.</summary>
public enum FacetDisplayType
{
	/// <summary>Flat menu</summary>
	Flat,

	/// <summary>Hierarchy</summary>
	Hierarchy,

	/// <summary>Not sure.</summary>
	RefinementList,
}
