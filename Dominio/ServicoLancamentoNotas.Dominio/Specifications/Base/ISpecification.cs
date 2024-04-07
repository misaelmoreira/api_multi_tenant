namespace ServicoLancamentoNotas.Dominio.Specifications.Base
{

    public interface ISpecification<in T>
    {
        bool IsSatisfied(T obj);
        
    }
}