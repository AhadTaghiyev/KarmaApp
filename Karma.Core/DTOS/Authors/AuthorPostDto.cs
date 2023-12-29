using System;
using Karma.Core.Entities;
using Microsoft.AspNetCore.Http;

namespace Karma.Core.DTOS
{
    public record AuthorPostDto
    {
        public string FullName { get; set; } = null!;
        public string Info { get; set; } = null!;
        public int PositionId { get; set; }
        public List<string> Icons { get; set; } = null!;
        public List<string> Urls { get; set; } = null!;
        public IFormFile? ImageFile{get;set;} 
    }
}

