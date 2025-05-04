using quill.Interfaces;
using quill.Services;

namespace quill.Resolvers;

public class BoardsServiceResolver
{
    private readonly IServiceProvider _provider;
    private readonly IHttpContextAccessor _httpContext;

    public BoardsServiceResolver(IServiceProvider provider, IHttpContextAccessor httpContext)
    {
        _provider = provider;
        _httpContext = httpContext;
    }

    public IBoardService GetStorage()
    {
        var user = _httpContext.HttpContext.User;
        return user.Identity.IsAuthenticated
            ? _provider.GetRequiredService<BoardServiceDb>()
            : _provider.GetRequiredService<BoardServiceLocal>();
    }
}