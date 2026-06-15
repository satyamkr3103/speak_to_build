using System.Threading.Tasks;

public interface IModelSearchProvider
{
    Task<ModelSearchResult> Search(
        string objectName);
}
