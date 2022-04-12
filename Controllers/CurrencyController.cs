using AutoMapper;
using Domain;
using Domain.Interfaces;
using Mapping.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Trade.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CurrencyController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/<CurrencyController>
        [HttpGet]
        [Authorize]
        public IActionResult Get()
        {
            var currencies = _unitOfWork.Currencies.GetAll();
            var result = _mapper.Map<IList<CurrencyListDTO>>(currencies);
            return Ok(result);
        }

        // GET api/<CurrencyController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var currency = _unitOfWork.Currencies.GetById(id);
            return Ok(currency);
        }

        // POST api/<CurrencyController>
        [HttpPost]
        public IActionResult Post([FromBody] NewCurrencyDTO data)
        {
            var currency = _mapper.Map<Currency>(data);
            _unitOfWork.Currencies.Add(currency);
            _unitOfWork.Complete();

            return CreatedAtAction("get", currency);
        }

        // PUT api/<CurrencyController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CurrencyController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
