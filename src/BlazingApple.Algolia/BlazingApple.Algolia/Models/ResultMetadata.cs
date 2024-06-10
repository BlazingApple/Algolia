using System.Text.Json.Serialization;
using Uglia = Algolia.Search.Models.Search;

namespace BlazingApple.Algolia.Models;

/// <summary>Details about the result.</summary>
public class ResultMetadata
{
	/// <inheritdoc cref="Uglia.HighlightResult"/>
	[JsonPropertyName("_highlightResult")]
	public Uglia.HighlightResult? HighlightResult { get; set; }

	/// <summary>Sequence Id</summary>
	[JsonPropertyName("_distinctSeqID")]
	public int DistinctSequenceId { get; set; }

	/// <summary>Ranking information.</summary>
	[JsonPropertyName("_rankingInfo")]
	public dynamic? RankingInformation { get; set; }

	/// <inheritdoc cref="Uglia.SnippetResult"/>
	[JsonPropertyName("_snippetResult")]
	public Uglia.SnippetResult? SnippetResult { get; set; }
}
