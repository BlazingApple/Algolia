using Algolia.Search.Models.Search;
using BlazingApple.Algolia.Models;
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

	private IEnumerable<FacetDetails> GetFacets()
	{
		yield return new FacetDetails("categories", "Categories", FacetDisplayType.Flat);
		yield return new FacetDetails("family", "Family", FacetDisplayType.Flat);
		yield return new FacetDetails("style", "Style", FacetDisplayType.Flat);
		yield return new FacetDetails("is_sponsored", "Sponsored?", FacetDisplayType.Flat);
		yield return new FacetDetails("is_free", "Free?", FacetDisplayType.Flat);
	}
}
