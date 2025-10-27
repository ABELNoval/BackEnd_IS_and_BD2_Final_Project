using Domain.Entities;
using Infrastructure.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Persistence.Repositories
{
    public class AssessmentRepository : BaseRepository<Assessment>, IAssessmentRepository
    {
        public AssessmentRepository(AppDbContext context) : base(context) { }
    }
}
