using BenchmarkDotNet.Attributes;

namespace Pedidos.Performance;

[MemoryDiagnoser]
public class RelatorioBenchmark
{
    private string[] _linhas = [];
    private List<string> _bloqueadosLista = [];
    private HashSet<string> _bloqueadosSet = [];

    [Params(10_000, 100_000)]
    public int Linhas { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _linhas = GeradorDeDados.GerarLinhas(Linhas, clientes: 1_000, seed: 42);
        _bloqueadosLista = GeradorDeDados.GerarBloqueados(50);
        _bloqueadosSet = new HashSet<string>(_bloqueadosLista, StringComparer.Ordinal);
    }

    [Benchmark(Baseline = true)]
    public List<ItemRelatorio> Original() => RelatorioOriginal.Gerar(_linhas, _bloqueadosLista);

    [Benchmark]
    public List<ItemRelatorio> Etapa1UmaPassada() => RelatorioEtapas.Etapa1UmaPassada(_linhas, _bloqueadosLista);

    [Benchmark]
    public List<ItemRelatorio> Etapa2HashSet() => RelatorioEtapas.Etapa2HashSet(_linhas, _bloqueadosSet);

    [Benchmark]
    public List<ItemRelatorio> Otimizado() => RelatorioOtimizado.Gerar(_linhas, _bloqueadosSet);
}
