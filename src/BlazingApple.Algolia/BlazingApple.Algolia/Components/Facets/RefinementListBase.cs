using Microsoft.AspNetCore.Components;

namespace BlazingApple.Algolia.Components.Facets;

/// <summary>A list of refinements.</summary>
public abstract class RefinementListBase : ComponentBase
{
	/// <summary>The default amount of refinements to show</summary>
	protected const int DefaultItemCount = 10;

	/// <summary>Whether the refinement list is currently showing more.</summary>
	public bool IsShowingMore { get; set; }

	/// <summary>Whether the refinement list can or can't show more.</summary>
	[Parameter]
	public bool CanShowMore { get; set; } = true;

	/// <summary>Invoked after a refinement is selected or unselected.</summary>
	[Parameter]
	public EventCallback OnRefine { get; set; }
}
