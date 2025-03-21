using Library.Models.DTOs;
using Library.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BookController : ControllerBase
    {
        readonly internal IBookServices _bookServices;
        public BookController(IBookServices bookService)
        {
            _bookServices = bookService;
        }

        [HttpGet]
        [Route("GetAll")]
        public IActionResult GetAll()
        {
            var response = _bookServices.GetAll();
            if (response.Success)
                return Ok(response);
            else
                return StatusCode(500, response);
        }

        [HttpGet]
        [Route("GetById/{id}", Name = "GetById")]
        public IActionResult GetById(int id)
        {
            var response = _bookServices.GetById(id);

            if (!response.Success)
            {
                    return NotFound(response);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("SearchByName")]
        public IActionResult SearchByName([FromQuery]string name)
        {
            var response = _bookServices.SearchByName(name);

            if(!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Add(CreateBookDTO book)
        {
            var response = _bookServices.Add(book);

            if (response.Success)
            {
                return CreatedAtRoute(
                    routeName: "GetById",
                    routeValues: new { id = response.Data.Id },
                    value: response
                );              
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPut]
        [Route("{id}/Available")]
        public IActionResult Available([FromQuery]bool available, int id)
        {
            var response = _bookServices.Available(available, id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete]
        [Route("{id}")]
        public IActionResult Delete(int id)
        {
            var response = _bookServices.Delete(id);
            if (!response.Success)
            {
                return NotFound(response);
            }
            return Ok(response);            
        }
    }
}
