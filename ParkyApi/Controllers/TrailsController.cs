using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyApi.Models;
using ParkyApi.Models.Dtos;
using ParkyApi.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Controllers
{
    [Route("api/[Trails]")]
    [ApiController]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class TrailsController : ControllerBase
    {
        private readonly ITrailRepository _trailRepository;
        private readonly IMapper _mapper;

        public TrailsController(
            ITrailRepository trailRepository, 
            IMapper mapper)
        {
            _trailRepository = trailRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Get list of trails
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TrailDto>))]
        [ProducesDefaultResponseType]
        public IActionResult GetTrails()
        {
            var objList = _trailRepository.GetTrails();
            var objDto = new List<TrailDto>();

            foreach(var obj in objList)
            {
                objDto.Add(_mapper.Map<TrailDto>(obj));
            }

            return Ok(objDto);
        }

        /// <summary>
        /// Get individual trail
        /// </summary>
        /// <param name="trailId">The Id of the trail</param>
        /// <returns></returns>
        [HttpGet("{trailId:int}", Name = "GetTrail")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesDefaultResponseType]
        public IActionResult GetTrail(int trailId)
        {
            var obj = _trailRepository.GetTrail(trailId);

            if(obj == null)
            {
                return NotFound();
            }

            var objDto = _mapper.Map<TrailDto>(obj);
            return Ok(objDto);
        }

        /// <summary>
        /// Create record for trail
        /// </summary>
        /// <param name="trailDto">The trail object</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TrailDto))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult CreateTrail([FromBody] TrailDto trailDto)
        {
            //check if Trail obj is empty
            if(trailDto == null)
            {
                return BadRequest(ModelState);
            }

            //check if record exists
            if (_trailRepository.TrailExists(trailDto.Name))
            {
                ModelState.AddModelError("", "Trail Exists!");
                return StatusCode(404, ModelState);
            }

            var trailObj = _mapper.Map<Trail>(trailDto);

            if (!_trailRepository.CreateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetTrail", new { trailId = trailObj.Id}, trailObj );
        }

        /// <summary>
        /// Update record for trail
        /// </summary>
        /// <param name="trailDto"></param>
        /// <param name="trailId">The trail Id</param>
        /// <returns></returns>
        [HttpPatch("{trailId:int}", Name = "UpdateTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]        
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult UpdateTrail([FromBody] TrailDto trailDto, int trailId)
        {
            //check if Trail obj is empty
            if (trailDto == null || trailId != trailDto.Id)
            {
                return BadRequest(ModelState);
            }

            var trailObj = _mapper.Map<Trail>(trailDto);

            if (!_trailRepository.UpdateTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        /// <summary>
        /// Delete record for trail
        /// </summary>
        /// <param name="trailId">The trail Id</param>
        /// <returns></returns>
        [HttpDelete("{trailId:int}", Name = "DeleteTrail")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesDefaultResponseType]
        public IActionResult DeleteTrail(int trailId)
        {
            //check if Trail Id exists
            if (!_trailRepository.TrailExists(trailId))
            {
                return NotFound();
            }
            
            var trailObj = _trailRepository.GetTrail(trailId);

            if (!_trailRepository.DeleteTrail(trailObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {trailObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
