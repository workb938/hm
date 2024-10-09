using AutoMapper;
using GTE.Mastery.LearnIT.Business.Dtos;
using GTE.Mastery.LearnIT.Business.Services.Interfaces;
using GTE.Mastery.LearnIT.Business.ViewModelsDtos;
using GTE.Mastery.LearnIT.Data.Repositories.Interfaces;
using GTE.Mastery.LearnIT.Domain.Models;

namespace GTE.Mastery.LearnIT.Business.Services
{
    public class CourseService : ICourseService
    {
        private readonly ICourseRepository _courseRepository;
        private readonly IMapper _mapper;

        public CourseService(ICourseRepository courseRepository, IMapper mapper)
        {
            _courseRepository = courseRepository;
            _mapper = mapper;
        }

        public async Task<CourseDto?> GetByIdAsync(int courseId)
        {
            var course = await _courseRepository.GetByIdAsync(courseId);
            return _mapper.Map<CourseDto>(course);
        }

        public async Task<CoursesViewModelDto> GetAllCoursesByUserIdAsync(int userId)
        {
            var courses = await _courseRepository.GetAllCoursesByUserIdAsync(userId);
            return _mapper.Map<CoursesViewModelDto>(courses);
        }

        public async Task<IReadOnlyList<CourseDto>> GetAllCoursesAsync()
        {
            var courses = await _courseRepository.GetAllAsync();
            return _mapper.Map<IReadOnlyList<CourseDto>>(courses);
        }

        public async Task<CourseDto> AddCourseAsync(CourseDto courseDto)
        {
            if (courseDto == null)
            {
                throw new ArgumentNullException(nameof(courseDto));
            }

            courseDto.CreationDate = DateTime.Now;
            var course = _mapper.Map<Course>(courseDto);
            await _courseRepository.AddAsync(course);
            courseDto.Id = course.Id;
            return courseDto;
        }

        public async Task AddRelationAsync(int? courseId, int? userId)
        {
            await _courseRepository.AddRelationAsync(courseId, userId);
        }

        public async Task UpdateCourseAsync(int courseId, CourseDto courseDto)
        {
            if (courseDto == null)
            {
                throw new ArgumentNullException(nameof(courseDto));
            }

            var existingCourse = await _courseRepository.GetByIdAsync(courseId);
            if (existingCourse == null)
            {
                throw new InvalidOperationException($"Course with Id {courseId} does not exist.");
            }

            _mapper.Map(courseDto, existingCourse);
            await _courseRepository.UpdateAsync(existingCourse);
        }

        public async Task DeleteCourseAsync(int courseId)
        {
            var existingCourse = await _courseRepository.GetByIdAsync(courseId);
            if (existingCourse == null)
            {
                throw new InvalidOperationException($"Course with Id {courseId} does not exist.");
            }

            await _courseRepository.DeleteAsync(courseId);
        }
    }
}
