﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamePass.Repository.IRepository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenreRepository Genre { get; }
        ISP_Call SP_Call { get; }
    }
}