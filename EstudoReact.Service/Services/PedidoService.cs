using Estudo.Model.Enum;
using EstudoReact.Data;
using EstudoReact.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EstudoReact.Service.Services
{
    public class PedidoService
    {
        Context _context;
        public PedidoService()
        {
            _context = new Context();
        }

        public async Task SalvarPedido(Pedido pedido)
        {
            try
            {
                if (ValidaPedido(pedido))
                {
                    if (pedido.Id == 0)
                        _context.Salvar(pedido);
                    else
                        _context.Alterar(pedido);
                    await _context.CommitAsync();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<Pedido>> ListarPedido()
        {
            var pedido = await _context.Pedidos.Include(i => i.Itens).ToListAsync();
            return pedido;
        }

        public async Task<Pedido> SelecionarPedido(int Id)
        {
            var pedido = await _context.Pedidos.FindAsync(Id);
            pedido.Itens = await _context.ItensPedido.Where(a => a.IdPedido == Id).ToListAsync();
            return pedido;
        }

        public async Task ExcluirPedido(int id)
        {
            try
            {
                Pedido pedido = await SelecionarPedido(id);
                _context.Excluir(pedido);
                await _context.CommitAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private bool ValidaPedido(Pedido pedido)
        {
            bool valida = true;

            if (pedido == null)
            {
                valida = false;
                throw new Exception("Pedido inválido");
            }

            if (pedido.DataInclusao > DateTime.Now.Date)
            {
                valida = false;
                throw new Exception("A data do pedido é maior que a data atual");
            }

            if (pedido.Comprador == null || pedido.IdComprador == 0)
            {
                valida = false;
                throw new Exception("Informe um comprador válido para o pedido");
            }
            if (pedido.Itens == null || pedido.Itens.Count() == 0)
            {
                valida = false;
                throw new Exception("Informe itens para o pedido");
            }

            foreach (ItemPedido item in pedido.Itens)
            {
                item.Item = item.Item + 1;
                if (item.Valor <= 0)
                {
                    valida = false;
                    throw new Exception("Informe um valor válido para o item do pedido");
                }

                if (!Enum.IsDefined(typeof(Moeda), item.Moeda))
                {
                    valida = false;
                    throw new Exception("Informe uma moeda válida");
                }
            }
            return valida;

        }

        public async Task<decimal> BuscaValorMoedaEstrangeira(string moeda)
        {
            using HttpClient client = new HttpClient();
            decimal nCotacao = 0;
            try
            {
                // Substitua pela URL da sua API real
                string url = @$"https://economia.awesomeapi.com.br/json/last/{moeda}";

                HttpResponseMessage response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string json = await response.Content.ReadAsStringAsync();

                    using JsonDocument doc = JsonDocument.Parse(json);
                    JsonElement root = doc.RootElement;

                    if (root.TryGetProperty($"{moeda.ToUpper()}BRL", out JsonElement usdbrl) &&
                        usdbrl.TryGetProperty("ask", out JsonElement askElement))
                    {
                        string ask = askElement.GetString().Replace(".", ",");
                        nCotacao = Convert.ToDecimal(ask);
                    }
                }
                else if (response.StatusCode == HttpStatusCode.TooManyRequests)
                {
                    if (response.Headers.TryGetValues("Retry-After", out var valores))
                    {
                        if (int.TryParse(valores.First(), out int segundos))
                        {
                            Console.WriteLine($"Limite atingido. Aguardando {segundos}s...");
                            await Task.Delay(segundos * 1000);
                        }
                        else
                        {
                            await Task.Delay(3000); // fallback de 3s
                        }
                    }
                    else
                    {
                        await Task.Delay(3000); // fallback
                    }
                }
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"Erro HTTP: {httpEx.Message}");
            }
            catch (JsonException jsonEx)
            {
                Console.WriteLine($"Erro ao processar JSON: {jsonEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro inesperado: {ex.Message}");
            }
            return nCotacao;
        }

    }
}
