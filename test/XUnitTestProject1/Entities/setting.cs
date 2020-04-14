﻿using System;
using System.Collections.Generic;
using System.Text;
using Naruto.BaseRepository.Model;
using System.ComponentModel.DataAnnotations;

namespace Naruto.Domain.Model.Entities
{
    public class setting : IEntity
    {
        public int Id { get; set; }
        public string DuringTime { get; set; }
        public string? Rule { get; set; }
        [ConcurrencyCheck]
        public string Contact { get; set; }
        [Required]
        public string Description { get; set; }
        public int Integral { get; set; }
    }
}
