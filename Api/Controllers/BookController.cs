using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using BusinessLogic.Dtos;
using BusinessLogic.Exception;
using BusinessLogic.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[Controller]")]
    [ApiController]
    public class BookController : Controller
    {
        private readonly IService<BookDto, int> _service;

        public BookController(IService<BookDto, int> service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var employee = await _service.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound($"Book with Id: {id}");
            }
            return Ok(employee);

        }

        [HttpGet]
        public  IActionResult GetAll() => Ok(_service.GetAll().OrderByDescending(e => e.Author));

        [HttpPost]
        public async Task<IActionResult> Post(BookDto bookDto)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var create = await _service.Add(bookDto);

                    return CreatedAtAction(nameof(Get), new { create.Id }, create);
                }
                catch (ValidateException ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
                }
            }
            return BadRequest(ModelState);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateEmployee(int id, BookDto bookDto)
        {
            if (id != bookDto.Id) return BadRequest();
            try
            {
                await _service.Update(id, bookDto);

                return Ok(await _service.Update(id, bookDto));
            }
            catch (ArgumentNullException ex)
            {
                return NotFound(ex);
            }
            catch (Exception ex)
            {
                return  StatusCode((int) HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> DeleteEmployee(int id)
        {
            try
            {
                var employeeToDelete = await _service.GetByIdAsync(id);

                if (employeeToDelete == null) return NotFound(id);

                await _service.Delete(employeeToDelete.Id);

                return Ok(employeeToDelete);
            }
            catch (ArgumentNullException ex)
            {
                return  StatusCode((int) HttpStatusCode.InternalServerError, ex);
            }

           
        }
    }
}
