using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamePass.Models.ViewModels
{
    public class GenreViewModel
    {
        public IEnumerable<Genre> Genres { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
