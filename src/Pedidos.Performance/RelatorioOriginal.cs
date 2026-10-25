using System.Globalization;

namespace Pedidos.Performance;

/// <summary>
/// Versão original: funciona, é legível e parece inofensiva.
/// </summary>
public static class RelatorioOriginal
{
    public static List<ItemRelatorio> Gerar(string[] linhas, List<string> clientesBloqueados)
    {
        var pedidos = new List<Pedido>();

        foreach (var linha in linhas)
        {
            var partes = linha.Split(';');

            pedidos.Add(new Pedido(
                Numero: partes[0],
                Cliente: partes[1],
                Sku: partes[2],
                Quantidade: int.Parse(partes[3], CultureInfo.InvariantCulture),
                PrecoUnitario: decimal.Parse(partes[4], CultureInfo.InvariantCulture)));
        }

        var resultado = new List<ItemRelatorio>();

        foreach (var cliente in pedidos.Select(p => p.Cliente).Distinct())
        {
            if (clientesBloqueados.Contains(cliente))
                continue;

            var total = pedidos
                .Where(p => p.Cliente == cliente)
                .Sum(p => p.Quantidade * p.PrecoUnitario);

            resultado.Add(new ItemRelatorio(cliente, total));
        }

        return resultado
            .OrderByDescending(r => r.Total)
            .ThenBy(r => r.Cliente, StringComparer.Ordinal)
            .Take(10)
            .ToList();
    }
}
