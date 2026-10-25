using BenchmarkDotNet.Running;
using Pedidos.Performance;

// 1. Antes de medir, garantir que todas as versões produzem exatamente o mesmo resultado
var linhas = GeradorDeDados.GerarLinhas(20_000, clientes: 1_000, seed: 7);
var bloqueados = GeradorDeDados.GerarBloqueados(50);

var esperado = RelatorioOriginal.Gerar(linhas, bloqueados);
var bloqueadosSet = new HashSet<string>(bloqueados, StringComparer.Ordinal);

var versoes = new[]
{
    RelatorioEtapas.Etapa1UmaPassada(linhas, bloqueados),
    RelatorioEtapas.Etapa2HashSet(linhas, bloqueadosSet),
    RelatorioOtimizado.Gerar(linhas, bloqueadosSet)
};

if (versoes.Any(v => !esperado.SequenceEqual(v)))
    throw new InvalidOperationException("As versões produziram relatórios diferentes. A otimização mudou o comportamento.");

Console.WriteLine("Validação OK: todas as versões produzem o mesmo relatório.");

// 2. Medir com BenchmarkDotNet (execute sempre em Release)
BenchmarkSwitcher.FromAssembly(typeof(RelatorioBenchmark).Assembly).Run(args);
