using System.Threading.Tasks;

public interface IModelSearchProvider
{
    Task<SearchResultsBundle>
    Search(string objectName);
}
