# Pedidos.Performance

Projeto de estudo de performance em .NET 10 com apoio de IA.

## Regras para otimizações de performance
- Nunca otimize sem medir antes. Todo gargalo precisa de um benchmark no BenchmarkDotNet.
- Proponha uma mudança por vez e explique a complexidade (Big O) antes e depois.
- O comportamento não pode mudar: a validação em Program.cs precisa continuar passando.
- Considere alocações de memória, não só tempo ([MemoryDiagnoser]).
- Não use unsafe, stackalloc ou paralelismo sem um benchmark que justifique.
- Mantenha CultureInfo.InvariantCulture em toda conversão numérica.

## Como validar
- dotnet build -c Release sem warnings
- dotnet run -c Release --project src/Pedidos.Performance -- --filter "*"
