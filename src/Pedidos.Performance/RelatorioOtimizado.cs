using System.Globalization;
using System.Runtime.InteropServices;

namespace Pedidos.Performance;

/// <summary>
/// Versão otimizada: uma única passada, sem Split e sem alocar strings por linha.
/// </summary>
public static class RelatorioOtimizado
{
    /// <param name="clientesBloqueados">HashSet criado com StringComparer.Ordinal.</param>
    public static List<ItemRelatorio> Gerar(string[] linhas, HashSet<string> clientesBloqueados)
    {
        if (!clientesBloqueados.TryGetAlternateLookup<ReadOnlySpan<char>>(out var bloqueados))
            throw new ArgumentException("Crie o HashSet com StringComparer.Ordinal.", nameof(clientesBloqueados));

        var totais = new Dictionary<string, decimal>(StringComparer.Ordinal);
        var totaisPorCliente = totais.GetAlternateLookup<ReadOnlySpan<char>>();

        foreach (var linha in linhas)
        {
            // Formato: PEDIDO;CLIENTE;SKU;QUANTIDADE;PRECO
            ReadOnlySpan<char> resto = linha;

            ProximoCampo(ref resto); // número do pedido (não usado no relatório)
            var cliente = ProximoCampo(ref resto);
            ProximoCampo(ref resto); // SKU (não usado no relatório)
            var quantidade = int.Parse(ProximoCampo(ref resto), NumberStyles.Integer, CultureInfo.InvariantCulture);
            var preco = decimal.Parse(resto, NumberStyles.Number, CultureInfo.InvariantCulture);

            if (bloqueados.Contains(cliente))
                continue;

            // Busca pelo span: a string do cliente só é criada na primeira vez que ele aparece
            ref var total = ref CollectionsMarshal.GetValueRefOrAddDefault(totaisPorCliente, cliente, out _);
            total += quantidade * preco;
        }

        return totais
            .Select(t => new ItemRelatorio(t.Key, t.Value))
            .OrderByDescending(r => r.Total)
            .ThenBy(r => r.Cliente, StringComparer.Ordinal)
            .Take(10)
            .ToList();
    }

    private static ReadOnlySpan<char> ProximoCampo(ref ReadOnlySpan<char> resto)
    {
        var fim = resto.IndexOf(';');
        var campo = resto[..fim];
        resto = resto[(fim + 1)..];
        return campo;
    }
}
