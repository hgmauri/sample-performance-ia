# sample-performance-ia

Exemplo de otimização de performance em .NET 10 com apoio de IA: medir com BenchmarkDotNet, otimizar uma mudança por vez e provar o resultado.

O cenário é um relatório de vendas que soma o total por cliente a partir de linhas de pedidos, ignorando clientes bloqueados.

## O que o exemplo mostra

- `RelatorioOriginal`: versão legível, mas com complexidade O(clientes × pedidos), `List.Contains` e `Split` por linha
- `RelatorioEtapas`: as etapas intermediárias da otimização, uma mudança por vez
- `RelatorioOtimizado`: uma única passada, `HashSet`, parsing com `ReadOnlySpan<char>` e busca no dicionário sem alocar strings (alternate lookup do .NET 9+)
- `RelatorioBenchmark`: comparação com BenchmarkDotNet, incluindo alocação de memória
- `Program.cs`: validação de que todas as versões geram exatamente o mesmo relatório antes de medir
- `CLAUDE.md`: regras que uso para orientar a IA em tarefas de performance

## Resultados de referência

100 mil linhas e 1.000 clientes, .NET 10:

| Versão | Tempo médio | Memória alocada |
|---|---|---|
| Original | 1.553 ms | 33,1 MB |
| Etapa 1: uma passada com Dictionary | 40 ms | 24,9 MB |
| Etapa 2: HashSet para bloqueados | 26 ms | 24,9 MB |
| Etapa 3: Span + alternate lookup | 11,9 ms | 0,23 MB |

Os valores absolutos variam conforme a máquina. Rode o benchmark para obter os seus.

## Como executar

Pré-requisito: [.NET 10 SDK](https://dotnet.microsoft.com/download)

```bash
dotnet run -c Release --project src/Pedidos.Performance -- --filter "*"
```

O benchmark da versão original com 100 mil linhas é propositalmente lento e pode levar alguns minutos.

## Autor

Henrique Mauri, [henriquemauri.net](https://henriquemauri.net)
