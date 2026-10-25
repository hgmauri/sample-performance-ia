using System.Globalization;

namespace Pedidos.Performance;

/// <summary>
/// Etapas intermediárias da otimização: uma mudança por vez, medindo cada uma.
/// </summary>
public static class RelatorioEtapas
{
    /// <summary>Etapa 1: uma única passada com Dictionary (O(pedidos) em vez de O(clientes × pedidos)).</summary>
    public static List<ItemRelatorio> Etapa1UmaPassada(string[] linhas, List<string> clientesBloqueados)
    {
        var totais = new Dictionary<string, decimal>(StringComparer.Ordinal);

        foreach (var linha in linhas)
        {
            var partes = linha.Split(';');
            var cliente = partes[1];

            if (clientesBloqueados.Contains(cliente))
                continue;

            var valor = int.Parse(partes[3], CultureInfo.InvariantCulture) * decimal.Parse(partes[4], CultureInfo.InvariantCulture);
            totais[cliente] = totais.GetValueOrDefault(cliente) + valor;
        }

        return Top10(totais);
    }

    /// <summary>Etapa 2: HashSet para os bloqueados (busca O(1) em vez de O(n)).</summary>
    public static List<ItemRelatorio> Etapa2HashSet(string[] linhas, HashSet<string> clientesBloqueados)
    {
        var totais = new Dictionary<string, decimal>(StringComparer.Ordinal);

        foreach (var linha in linhas)
        {
            var partes = linha.Split(';');
            var cliente = partes[1];

            if (clientesBloqueados.Contains(cliente))
                continue;

            var valor = int.Parse(partes[3], CultureInfo.InvariantCulture) * decimal.Parse(partes[4], CultureInfo.InvariantCulture);
            totais[cliente] = totais.GetValueOrDefault(cliente) + valor;
        }

        return Top10(totais);
    }

    internal static List<ItemRelatorio> Top10(Dictionary<string, decimal> totais) =>
        totais
            .Select(t => new ItemRelatorio(t.Key, t.Value))
            .OrderByDescending(r => r.Total)
            .ThenBy(r => r.Cliente, StringComparer.Ordinal)
            .Take(10)
            .ToList();
}
