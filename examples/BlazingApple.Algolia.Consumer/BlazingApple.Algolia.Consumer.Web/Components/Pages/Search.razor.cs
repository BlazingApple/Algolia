using Algolia.Search.Models.Search;
using Microsoft.AspNetCore.Components;

namespace BlazingApple.Algolia.Consumer.Web.Components.Pages;

public partial class Search : ComponentBase
{
	private void SetQuery(Query query)
	{
		query.Facets = ["categories", "family", "is_free", "is_new_in_v6", "is_sponsored", "is_staff_favorite", "style"];
		query.HitsPerPage = 100;
		query.FacetFilters = [["type:icon"]];
		query.Distinct = 1;
		query.UserToken = "anonymous-710695cf-35b1-46b7-b277-c397e7dc9ce2";
	}
}
