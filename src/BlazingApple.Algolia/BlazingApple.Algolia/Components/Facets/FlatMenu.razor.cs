using Microsoft.AspNetCore.Components;

namespace BlazingApple.Algolia.Components.Facets;

/// <summary>Flat menu to filter results by.</summary>
public partial class FlatMenu : RefinementListBase
{
	/// <summary>Facet values to render</summary>
	[Parameter]
	public IReadOnlyDictionary<string, long>? Facets { get; set; }
}
