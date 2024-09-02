using EcommStart.Shared.Models;
using Microsoft.AspNetCore.Mvc;


namespace EcommStart.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        [HttpPost("incluir")]
        public IActionResult Adicionar(Produto produto)
        {
            if (produto == null) return BadRequest("Produto não foi enviado por parametro.");

            Produto? produtoAnterior = Banco.Produtos.OrderByDescending(x => x.Id).FirstOrDefault();

            if (produtoAnterior != null)
                produto.Id = produtoAnterior.Id + 1;
            else
                produto.Id = 1;

            Banco.Produtos.Add(produto);
            return Ok();
        }

        [HttpGet("listar")]
        public IActionResult Listar(string? nome)
        {
            List<Produto>? retorno = Banco.Produtos.ToList();

            if (nome != null)
            {
                retorno = Banco.Produtos.Where(x => x.Nome.ToUpper().Contains(nome)).ToList();
            }

            if (retorno.Count > 0)
            {
                return Ok(retorno);
            }
            else
            {
                return BadRequest("Produtos não encontrados");
            }

        }

        [HttpGet("consultar/{id:int}")]
        public IActionResult Consultar(int id)
        {
            Produto? p = Banco.Produtos.Where(x => x.Id == id).FirstOrDefault();
            if (p == null) return BadRequest($"Erro: Produto nº {id} não encontrado.");
            return Ok(p);

        }

        [HttpPut("alterar")]
        public IActionResult Alterar(Produto produto)
        {
            if (produto == null) return BadRequest("Produto não foi encontrado por parâmetro");

            Produto? p = Banco.Produtos.Where(x => x.Id == produto.Id).FirstOrDefault();
            if (p == null) return BadRequest("Produto não existe mais na base");

            p.Nome = produto.Nome;
            p.Preco = produto.Preco;
            p.Quantidade = produto.Quantidade;
            p.Imagem = produto.Imagem;

            return Ok();
        }

        [HttpDelete("excluir/{id:int}")]
        public IActionResult Delete(int id)
        {
            Produto? p = Banco.Produtos.Where(x => x.Id == id).FirstOrDefault();
            if (p != null) return BadRequest("Produto não existe mais na base");
            Banco.Produtos.Remove(p);
            return Ok();
        }
    }
}


