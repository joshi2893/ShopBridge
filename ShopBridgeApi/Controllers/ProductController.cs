using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ShopBridgeApi.Models;
using ShopBridgeApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridgeApi.Controllers
{
    [ApiController]
    [Route("product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger; 
        public ProductController(IProductRepository repository, ILogger<ProductController> logger,IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            try
            {
                var product = await _repository.Get(id);
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An Error Occured While Retrieving Product. ID : {id}");
                return StatusCode(500, $"An Error Occured While Processing Your Request");
            }
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var products = await _repository.Get();
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An Error Occured While Retrieving Products");
                return StatusCode(500, $"An Error Occured While Processing Your Request");
            }
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductDto productDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var result = await _repository.Add(_mapper.Map<Product>(productDto));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An Error Occured While Retrieving Products");
                return StatusCode(500, $"An Error Occured While Processing Your Request");
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Product product)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var result = await _repository.Update(product);
                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An Error Occured While Updating Products");
                return StatusCode(500, $"An Error Occured While Processing Your Request");
            }
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var result = await _repository.Delete(id);
                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"An Error Occured While Updating Products");
                return StatusCode(500, $"An Error Occured While Processing Your Request");
            }
        }
    }
}
