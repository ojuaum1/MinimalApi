﻿using Microsoft.AspNetCore.Mvc;
using MinimalAPI.Domains;
using MinimalApi.ViewModels;
using MinimalAPI.Services;
using MongoDB.Driver;
using MinimalApi.Domains;

namespace MinimalApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IMongoCollection<Order> _order;
        private readonly IMongoCollection<Client> _client;
        private readonly IMongoCollection<Product> _product;

        public OrderController(MongoDbService mongoDbService)
        {
            _order = mongoDbService.GetDatabase.GetCollection<Order>("order");
            _client = mongoDbService.GetDatabase.GetCollection<Client>("client");
            _product = mongoDbService.GetDatabase.GetCollection<Product>("product");
        }

        // Método para criar um novo pedido
        [HttpPost]
        public async Task<ActionResult<Order>> Create(OrderViewModel orderViewModel)
        {
            try
            {
                // Cria uma nova instância de Order e atribui os valores da ViewModel
                Order order = new Order
                {
                    Id = orderViewModel.Id,
                    Date = orderViewModel.Date,
                    Status = orderViewModel.Status,
                    ProductId = orderViewModel.ProductId,
                    ClientId = orderViewModel.ClientId
                };

                // Verifica se o cliente existe no banco de dados
                var client = await _client.Find(x => x.Id == order.ClientId).FirstOrDefaultAsync();
                if (client == null)
                {
                    return NotFound();
                }

                // Atribui o cliente encontrado ao pedido
                order.Client = client;

                // Insere o pedido na coleção de pedidos
                await _order.InsertOneAsync(order);
                return StatusCode(201, order);
            }
            catch (Exception ex)
            {
                // Retorna um erro caso ocorra uma exceção
                return BadRequest(ex.Message);
            }
        }

        // Método para obter um pedido pelo ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetById(string id)
        {
            try
            {
                // Procura o pedido pelo ID
                var order = await _order.Find(o => o.Id == id).FirstOrDefaultAsync();
                if (order == null)
                {
                    return NotFound();
                }

                // Obtém o cliente e os produtos associados ao pedido
                var client = await _client.Find(c => c.Id == order.ClientId).FirstOrDefaultAsync();
                order.Client = client;

                var products = await _product.Find(p => order.ProductId.Contains(p.Id)).ToListAsync();
                order.Products = products;

                return Ok(order);
            }
            catch (Exception ex)
            {
                // Retorna um erro caso ocorra uma exceção
                return BadRequest(ex.Message);
            }
        }

        // Método para obter todos os pedidos
        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetAll()
        {
            try
            {
                // Obtém todos os pedidos
                var orders = await _order.Find(_ => true).ToListAsync();
                foreach (var order in orders)
                {
                    // Para cada pedido, obtém o cliente e os produtos associados
                    var client = await _client.Find(c => c.Id == order.ClientId).FirstOrDefaultAsync();
                    order.Client = client;

                    var products = await _product.Find(p => order.ProductId.Contains(p.Id)).ToListAsync();
                    order.Products = products;
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                // Retorna um erro caso ocorra uma exceção
                return BadRequest(ex.Message);
            }
        }

        // Método para atualizar um pedido pelo ID
        [HttpPut("{id}")]
        public async Task<ActionResult<Order>> Update(string id, OrderViewModel orderViewModel)
        {
            try
            {
                // Procura o pedido pelo ID
                var order = await _order.Find(o => o.Id == id).FirstOrDefaultAsync();
                if (order == null)
                {
                    return NotFound();
                }

                // Atualiza os valores do pedido com os da ViewModel
                order.Date = orderViewModel.Date;
                order.Status = orderViewModel.Status;
                order.ProductId = orderViewModel.ProductId;
                order.ClientId = orderViewModel.ClientId;

                // Verifica se o cliente existe no banco de dados
                var client = await _client.Find(x => x.Id == order.ClientId).FirstOrDefaultAsync();
                if (client == null)
                {
                    return NotFound();
                }

                // Atribui o cliente encontrado ao pedido
                order.Client = client;

                // Substitui o pedido na coleção de pedidos
                await _order.ReplaceOneAsync(o => o.Id == id, order);
                return Ok(order);
            }
            catch (Exception ex)
            {
                // Retorna um erro caso ocorra uma exceção
                return BadRequest(ex.Message);
            }
        }

        // Método para deletar um pedido pelo ID
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            try
            {
                // Procura o pedido pelo ID
                var order = await _order.Find(o => o.Id == id).FirstOrDefaultAsync();
                if (order == null)
                {
                    return NotFound();
                }

                // Deleta o pedido da coleção de pedidos
                await _order.DeleteOneAsync(o => o.Id == id);
                return NoContent();
            }
            catch (Exception ex)
            {
                // Retorna um erro caso ocorra uma exceção
                return BadRequest(ex.Message);
            }
        }
    }
}
