namespace PlataformaEducacao.GestaoConteudos.Domain
{
    public class Resultado<T> where T : class
    {
        // TODO: Mover para o projeto Api
        public bool Sucesso { get; set; }
        public List<string> Notificacoes { get; set; }
        public T Objeto { get; set; }
    }
}
