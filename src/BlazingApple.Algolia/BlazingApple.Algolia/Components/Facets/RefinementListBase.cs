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

	/// <summary>Facet values to render</summary>
	[Parameter]
	public IReadOnlyDictionary<string, long>? Facets { get; set; }

	/// <summary>Facet values currently selected</summary>
	[Parameter]
	public HashSet<string>? Value { get; set; }

	/// <summary>Invoked on change</summary>
	[Parameter]
	public EventCallback<HashSet<string>> ValueChanged { get; set; }

	/// <summary>Add or remove a key from the list</summary>
	/// <param name="facetKey">The facet key.</param>
	/// <returns>Async op.</returns>
	protected async Task OnFacetToggle(string facetKey)
	{
		Value ??= [];

		if (Value.Contains(facetKey))
		{
			Value.Remove(facetKey);
		}
		else
		{
			Value.Add(facetKey);
		}

		if (ValueChanged.HasDelegate)
		{
			await ValueChanged.InvokeAsync(Value);
		}

		if (OnRefine.HasDelegate)
		{
			await OnRefine.InvokeAsync();
		}
	}
}
