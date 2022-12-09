using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace API.Controllers
{
    public class ActivitiesController : BaseApiController
    {
        private readonly DataContext _dataContext;
        public ActivitiesController(DataContext dataContext)
        {
            _dataContext = dataContext;     
        }

        [HttpGet]   // api/Activities/
        public async Task<ActionResult<List<Activity>>>GetActivities() {
            return await _dataContext.Activities.ToListAsync();
        }


        [HttpGet("{id}")] // api/Activities/ID
        public async Task<ActionResult<Activity>>GetActivities(Guid id) {
            return await _dataContext.Activities.FindAsync(id);
        }


        
    }
}