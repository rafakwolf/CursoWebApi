using Aula02Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Linq;
using Aula02Api.Db;
using Aula02Api.Filters;
using Microsoft.EntityFrameworkCore;

namespace Aula02Api.Controllers
{

    [Route("api/[controller]")]
    public class TarefasController : Controller
    {
        private readonly DataContext _db;

        public TarefasController(DataContext db)
        {
            _db = db;
        }

        [HttpGet]
        public IActionResult ObterTodos()
        {
            return Ok(_db.Tarefas.AsNoTracking());
        }

        [HttpGet("{parametro}")]
        public IActionResult Get(int parametro)
        {
            return Ok(_db.Tarefas.FirstAsync(x => x.Id == parametro));
        }

        [HttpPost]
        [ValidateModel]
        public IActionResult Post([FromBody] Tarefa dados)
        {
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest(ModelState);
            // }

            //dados.Id = new Random(1000).Next();

            //var jsonText = JsonConvert.SerializeObject(dados);

            //System.IO.File.AppendAllText("dados.json", jsonText);

            _db.Tarefas.Add(dados);
            _db.SaveChanges();

            return Ok(dados);
        }

        [HttpPut]
        public IActionResult Alterar([FromBody] Tarefa dados)
        {
            _db.Tarefas.Update(dados);
            _db.SaveChanges();

            return Ok(dados);
        }

        [HttpDelete]
        public IActionResult Deletar(int parametro)
        {
            var item = _db.Tarefas.FirstOrDefault(x => x.Id == parametro);

            if (item != null)
                _db.Tarefas.Remove(item);

            _db.SaveChanges();

            return NoContent();
        }
    }

}