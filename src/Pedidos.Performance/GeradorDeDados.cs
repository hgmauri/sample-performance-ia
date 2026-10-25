using System.Globalization;

namespace Pedidos.Performance;

public static class GeradorDeDados
{
    public static string[] GerarLinhas(int quantidade, int clientes, int seed)
    {
        var random = new Random(seed);
        var linhas = new string[quantidade];

        for (var i = 0; i < quantidade; i++)
        {
            var cliente = random.Next(1, clientes + 1);
            var sku = random.Next(1, 500);
            var qtd = random.Next(1, 20);
            var preco = random.Next(100, 100_000) / 100m;

            linhas[i] = string.Create(CultureInfo.InvariantCulture,
                $"PED-{i:D6};CLI-{cliente:D4};SKU-{sku:D3};{qtd};{preco:F2}");
        }

        return linhas;
    }

    public static List<string> GerarBloqueados(int quantidade) =>
        Enumerable.Range(1, quantidade).Select(i => $"CLI-{i * 7:D4}").ToList();
}
