﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParkyApi.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NationalParksController : ControllerBase
    {
        private INationalParkRepository _npRepository;
        private readonly IMapper _mapper;

        public NationalParksController(
            INationalParkRepository npRepository, 
            IMapper mapper)
        {
            _npRepository = npRepository;
            _mapper = mapper;
        }
    }
}