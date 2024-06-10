namespace BlazingApple.Algolia.Models;

/// <summary>An Algolia search result</summary>
/// <typeparam name="T">The type of T</typeparam>
public class SearchResult<T>
{
	/// <summary>Result</summary>
	public T? Result { get; set; }

	/// <summary>Metadata about the result</summary>
	public ResultMetadata? Metadata { get; set; }
}
