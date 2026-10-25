namespace Pedidos.Performance;

public record Pedido(string Numero, string Cliente, string Sku, int Quantidade, decimal PrecoUnitario);

public record ItemRelatorio(string Cliente, decimal Total);
