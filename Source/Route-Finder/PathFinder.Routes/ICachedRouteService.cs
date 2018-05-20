namespace CalcRoute.Routes
{
    public interface ICachedRouteService : IRouteService
    {
        void SaveCache(string name);

        void LoadCache(string name);

        string GetRouteCache();

        bool HasCache { get; }
        bool UseCache { get; set; }

    }
}