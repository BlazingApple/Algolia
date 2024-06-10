using Microsoft.AspNetCore.Components;

namespace BlazingApple.Algolia.Components;

/// <summary>The side panel</summary>
public partial class Panel : ComponentBase
{
	/// <summary>The header to display, if any.</summary>
	[Parameter]
	public string? Header { get; set; }

	/// <summary>Child content to render.</summary>
	[Parameter]
	public RenderFragment? ChildContent { get; set; }
}
