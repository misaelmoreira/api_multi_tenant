using ServicoLancamentoNotas.Dominio.Entidades;
using FluentValidation;
using ServicoLancamentoNotas.Dominio.Constantes;

namespace ServicoLancamentoNotas.Dominio.Validacoes.Validador
{
    public class NotaValidador : AbstractValidator<Nota>
    {
        public static readonly NotaValidador Instance = new();
        public NotaValidador()
        {
            RuleFor(x => x.AtividadeId)
                .GreaterThan(default(int))
                .WithMessage(ConstantesDominio.MensagensValidacoes.ERRO_ATIVIDADE_INVALIDO);

            RuleFor(x => x.AlunoId)
                .GreaterThan(default(int))
                .WithMessage(ConstantesDominio.MensagensValidacoes.ERRO_ALUNO_INVALIDO);

            RuleFor(x => x.UsuarioId)
                .GreaterThan(default(int))
                .WithMessage(ConstantesDominio.MensagensValidacoes.ERRO_USUARIO_INVALIDO );                    

            RuleFor(x  => x.ValorNota)
                .InclusiveBetween(0.00, 10.00)
                .WithMessage(ConstantesDominio.MensagensValidacoes.ERRO_VALOR_NOTA_INVALIDO);
            
            When(x => x.MotivoCancelamento is not null, () => 
            {
                RuleFor(x => x.MotivoCancelamento)
                    .MaximumLength(500)
                    .WithMessage(ConstantesDominio.MensagensValidacoes.ERRO_MOTIVO_CANCELAMENTO_EXTENSO);

                RuleFor(x => x.Cancelada)
                    .Equal(true)
                    .WithMessage(ConstantesDominio.MensagensValidacoes.ERRO_FLAG_CANCELADA_INATIVA);
            });
        }        
    }
}