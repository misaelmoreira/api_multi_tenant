# Adicionar projeto com outra versao de framework 
dotnet new classlib -f net6.0 -n ServicoLancamentoNotas.Infra.CrossCutting

# adicionar referencia de projeto
dotnet add reference CaminhoParaOProjeto.csproj

# Adicionar o projeto na solucao
dotnet sln MinhaSolucao.sln add MeuNovoProjeto/MeuNovoProjeto.csproj
