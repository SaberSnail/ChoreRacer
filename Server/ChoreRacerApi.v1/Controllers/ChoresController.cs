using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ChoreRacerApi.v1.Models;
using ChoreRacerApi.v1.Data;

namespace ChoreRacerApi.v1.Controllers
{
	[Authorize]
	[Route("[controller]")]
	public sealed class ChoresController : Controller
	{
		public ChoresController(ChoresContext context)
		{
			m_context = context;
		}

		[HttpGet]
		public IEnumerable<ChoreDto> GetAll() => m_context.Chores.ToList();

		[HttpGet("{id}", Name = "GetChore")]
		public IActionResult GetById(long id)
		{
			var chore = m_context.Chores.FirstOrDefault(x => x.Id == id);
			return chore == null ? (IActionResult)NotFound() : new ObjectResult(chore);
		}

		[HttpPost]
		public IActionResult Create([FromBody] ChoreDto chore)
		{
			if (chore == null)
				return BadRequest();

			m_context.Chores.Add(chore);
			m_context.SaveChanges();

			return CreatedAtRoute("GetChore", new { id = chore.Id }, chore);
		}

		[HttpPut("{id}")]
		public IActionResult Update(long id, [FromBody] ChoreDto newChore)
		{
			if (newChore == null || newChore.Id != id)
				return BadRequest();

			var chore = m_context.Chores.FirstOrDefault(x => x.Id == id);
			if (chore == null)
				return NotFound();

			chore.Title = newChore.Title;
			chore.Description = newChore.Description;

			m_context.Chores.Update(chore);
			m_context.SaveChanges();

			return new NoContentResult();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(long id)
		{
			var chore = m_context.Chores.FirstOrDefault(x => x.Id == id);
			if (chore == null)
				return NotFound();

			m_context.Chores.Remove(chore);
			m_context.SaveChanges();

			return new NoContentResult();
		}

		private readonly ChoresContext m_context;
	}
}
