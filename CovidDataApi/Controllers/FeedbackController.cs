using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CovidDataApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CovidDataApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private CovidDbContext db;

        public FeedbackController(CovidDbContext dbContext)
        {
            db = dbContext;       
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<FeedbackData>>> GetFeedbacks()
        {
            return await db.Feedbacks
                .OrderByDescending(s=>s.SubmitedDate)
                .ToListAsync();
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<FeedbackData>> AddFeedback(FeedbackData feedback)
        {
            TryValidateModel(feedback);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            feedback.SubmitedDate = DateTime.Now;
            await db.Feedbacks.AddAsync(feedback);
            await db.SaveChangesAsync();
            return Created("", feedback);
        }
    }
}