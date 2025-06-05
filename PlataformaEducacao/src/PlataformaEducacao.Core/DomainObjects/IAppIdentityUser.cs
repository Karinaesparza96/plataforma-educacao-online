namespace PlataformaEducacao.Core.DomainObjects;

public interface IAppIdentityUser
{
    public string GetUserId();
    bool IsAuthenticated();
}