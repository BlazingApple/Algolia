# 🔎🍎 BlazingApple's Algolia Library
This client library is intended to ease the use of consuming Algolia's .NET library in Blazor, by offering a Blazor-equivalent to Algolia's React `InstantSearch` component.

## 🔧 Setup

Consumers need to provide configuration (via appSettings or some other means) to the service that will be executing search requests. Note that `Index` is not required, and can be provided as an argument to `IAlgoliaSearchService`.

```
  "Algolia":
  {
    "ApplicationId": "M19DXW5X0Q",
    "ApiKey": "c79b2e61519372a99fa5890db070064c",
    "Index": "fontawesome_com-splayed-5.15.4"
  }
```

## 🧑‍💻 Use
The page below was rendered simply with the following code, and the user using the "search" query in the UI:
![image](https://github.com/BlazingApple/Algolia/assets/3686217/48ec8819-8a4a-4835-ae24-815401eecfeb)

```
<h1>Search</h1>
<InstantSearch T="object" Context="context" ApplyQuerySettings="SetQuery">
	<ResultTemplate>
		@context.ToString()
	</ResultTemplate>
</InstantSearch>

@code {
	private void SetQuery(Query query)
	{
		query.Facets = ["categories", "family", "is_free", "is_new_in_v6", "is_sponsored", "is_staff_favorite", "style"];
		query.HitsPerPage = 100;
		query.FacetFilters = [["type:icon"]];
		query.Distinct = 1;
		query.UserToken = "anonymous-710695cf-35b1-46b7-b277-c397e7dc9ce2";
	}
}
```

In this example we provided no component, nor typed the response we expect from Algolia, and rendered each result by printing out it's serialized JSON data. To render components, simply supply the component to the `ResultTemplate` parameter on the `InstantSearch` component.
