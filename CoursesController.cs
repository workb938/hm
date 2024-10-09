using AutoMapper;
using GTE.Mastery.LearnIT.Business.Dtos;
using GTE.Mastery.LearnIT.Business.Services.Interfaces;
using GTE.Mastery.LearnIT.Business.ViewModelsDtos;
using GTE.Mastery.LearnIT.Domain.Models;
using GTE.Mastery.LearnIT.Domain.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GTE.Mastery.LearnIT.Controllers
{
    [ApiController]
    [Route("courses")]
    public class CoursesController : Controller
    {
        private readonly ICourseService _courseService;
        private readonly IUserService _userService;
        public CoursesController(ICourseService courseService, IUserService userService)
        {
            _courseService = courseService;
            _userService = userService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IReadOnlyList<CoursesViewModelDto>>> AllCourses()
        {
            var courses = await _courseService.GetAllCoursesAsync();
            return View("Courses", courses);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<CoursesViewModelDto>>> Courses(int userId)
        {
            Console.WriteLine($"userId: {userId}");
            var courses = await _courseService.GetAllCoursesByUserIdAsync(userId);
           
            if (courses.Courses == null || !courses.Courses.Any())
            {
                Console.WriteLine("No courses found for user.");
            }
            else
            {
                foreach (var course in courses.Courses)
                {
                    Console.WriteLine($"Course ID: {course.Id}, Title: {course.Title}, Description: {course.Description}");
                }
            }
            return View(courses);
        }

        [HttpGet("create/{userId}")]
        public async Task<IActionResult> Create(int userId)
        {
            TempData["UserId"] = userId; 
            TempData["PreviousUrl"] = HttpContext.Request.Headers["Referer"].ToString();
            return View("CreateCourse");
        }

        [HttpPost("create/{userId}")]
        public async Task<IActionResult> Create([FromForm] CourseDto courseDto)
        {
            if (courseDto == null)
            {
                return BadRequest("Course data is required.");
            }

            if (!ModelState.IsValid)
            {
                return View("CreateCourse", (int)TempData["UserId"]);
            }

            var userId = TempData["UserId"] != null ? (int)TempData["UserId"] : 0;
            if (userId == 0)
            {
                return Unauthorized("User ID not found.");
            }
            var userDto = await _userService.GetByIdAsync(userId);
            if (userDto == null)
            {
                return BadRequest("User does not exist.");
            }

            var course = await _courseService.AddCourseAsync(courseDto);
            courseDto.Id = course.Id;
            await _courseService.AddRelationAsync(courseDto.Id, userId);

            var previousUrl = TempData["PreviousUrl"] as string;
            if (!string.IsNullOrEmpty(previousUrl))
            {
                return Redirect(previousUrl);
            }
            return RedirectToAction("AllCourses");
        }

        [HttpGet("edit/{courseId}")]
        public async Task<IActionResult> Edit(int courseId, int userId)
        {
            var courses = await _courseService.GetAllCoursesByUserIdAsync(courseId);
            if (courses == null)
            {
                return NotFound("Course not found.");
            }
            return View("EditCourse", courses);
        }


        [HttpDelete("{courseId}")]
        public async Task<IActionResult> Delete(int courseId)
        {
            try
            {
                await _courseService.DeleteCourseAsync(courseId);
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
