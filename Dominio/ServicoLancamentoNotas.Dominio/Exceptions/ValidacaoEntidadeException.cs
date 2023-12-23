using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ServicoLancamentoNotas.Dominio.Exceptions;

public class ValidacaoEntidadeException : DomainException
{
    public ValidacaoEntidadeException() : base() {}
    public ValidacaoEntidadeException(string msg) : base(msg) {}
    public ValidacaoEntidadeException(string msg, Exception innerException) : base(msg, innerException) {}   
}


        