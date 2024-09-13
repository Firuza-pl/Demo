using Demo.Infrastructure.Modules;
using Demo.Infrastructure.Services;
using Demo.Domain.ViewModels.Courses;
using FluentValidation;
using Microsoft.AspNetCore.Mvc; 
using System.Net;

namespace ADO.API.MinimalAPI
{
    //Controllers
    public static class CourseEndpoints
    {
        public static void MapCourseEndpoints(this IEndpointRouteBuilder app)
        {
            //GET  
            app.MapGet("/api/getCourse", async (ICourseService courseService, ILogger<Program> logger) =>
            {
                logger.Log(LogLevel.Information, "All Courses");

                ApiResponse response = new();
                try
                {
                    var courses = await courseService.GetAllCoursesAsync();

                    if (courses is  null || !courses.Any())
                    {
                        response.isActive = false;
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.ErrorMessages.Add("No courses found.");
                        return Results.NotFound(response);
                    }

                    response.Result = courses;
                    response.isActive = true;
                    response.StatusCode = HttpStatusCode.OK;

                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while getting the courses");

                    response.isActive = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.ErrorMessages.Add("An error occurred while processing your request");

                    return Results.StatusCode((int)HttpStatusCode.InternalServerError);
                }
            }).WithName("GetCourses")
     .Produces<ApiResponse>(201)
     .Produces<ApiResponse>(404)
     .Produces<ApiResponse>(500)
     .WithTags("Course");

            //GET ID

            app.MapGet("/api/getCourseById/{id:int}", async (ICourseService courseService, ILogger<Program> logger, int id) =>
            {
                ApiResponse response = new();

                try
                {
                    logger.Log(LogLevel.Information, "Getting single course");

                    if (id is 0) { Console.WriteLine("Id must be defined"); }

                    var course = await courseService.GetCourseAsync(id);

                    if (course is null)
                    {
                        response.isActive = false;
                        response.StatusCode = HttpStatusCode.BadRequest;
                        response.ErrorMessages.Add("No course found");
                    }

                    response.Result = course;
                    response.isActive = true;
                    response.StatusCode = HttpStatusCode.OK;

                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while getting the course by id");

                    response.isActive = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.ErrorMessages.Add("An error occurred while processing your request");

                    return Results.StatusCode((int)HttpStatusCode.InternalServerError);
                }
            }).WithName("GetSingleCourse")
    .Produces<ApiResponse>(201) 
    .Produces<ApiResponse>(404)
    .Produces<ApiResponse>(500)
    .WithTags("Course");


            //Create
            app.MapPost("/api/createCourse/", async (ICourseService courseService, ILogger<Program> logger, [FromBody] CourseCreatedDTO createdDTO, IValidator<CourseCreatedDTO> validator) =>
            {

                logger.Log(LogLevel.Information, "All courses");

                ApiResponse response = new();

                try
                {
                    var resultValidation = await validator.ValidateAsync(createdDTO);

                    if (!resultValidation.IsValid)
                    {
                        response.ErrorMessages.AddRange(resultValidation.Errors.Select(x => x.ToString()));
                        return Results.BadRequest(response);
                    }

                    // Add course via the service
                    await courseService.AddCourseAsync(createdDTO);

                    // Set the response
                    response.Result = createdDTO;
                    response.isActive = true;
                    response.StatusCode = HttpStatusCode.Created;

                    // Return a Created result
                    return Results.Created($"/api/getcourseById/{createdDTO.CourseName}", response); // Use courseId instead of FirstName
                }
                catch (Exception ex)
                {
                    response.isActive = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.ErrorMessages.Add("An error occurred while processing your request");

                    return Results.StatusCode((int)HttpStatusCode.InternalServerError);
                }

            }).WithName("CreateCourse")
     .Produces<ApiResponse>(201)
     .Produces<ApiResponse>(404)
     .Produces<ApiResponse>(500)
     .WithTags("Course");


            //update
            app.MapPut("/api/updateCourse/", async (ICourseService courseService, ILogger<Program> logger, [FromBody] CourseUpdateDTO updateDTO, IValidator<CourseUpdateDTO> validator) =>
            {
                logger.Log(LogLevel.Information, "Updating course");

                ApiResponse response = new();

                try
                {
                    var resultValidation = await validator.ValidateAsync(updateDTO);
                    if (!resultValidation.IsValid)
                    {
                        response.ErrorMessages.AddRange(resultValidation.Errors.Select(x => x.ToString()));
                        return Results.BadRequest(response);
                    }

                    await courseService.UpdateCourseAsync(updateDTO.CourseId, updateDTO);

                    response.Result = updateDTO;  //"course updated successfully";
                    response.isActive = true;
                    response.StatusCode = HttpStatusCode.OK;

                    return Results.Created($"/api/getCourseById/{updateDTO.CourseId}", response);
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while updating the course");

                    response.isActive = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.ErrorMessages.Add("An error occurred while processing your request");

                    return Results.StatusCode((int)HttpStatusCode.InternalServerError);
                }

            }).WithName("UpdateCourse")
     .Produces<ApiResponse>(201)
     .Produces<ApiResponse>(404)
     .Produces<ApiResponse>(500)
     .WithTags("Course");

            //delete
            app.MapDelete("/api/deleteCourse/{id:int}", async (ICourseService courseService, ILogger<Program> logger, int id) =>
            {
                logger.Log(LogLevel.Information, "Deleting course");

                ApiResponse response = new();

                try
                {
                    bool isDeleted = await courseService.DeleteCourseAsync(id);

                    if (isDeleted)
                    {
                        response.Result = id;
                        response.isActive = true;
                        response.StatusCode = HttpStatusCode.OK;

                        return Results.Ok(response);
                    }
                    else
                    {
                        response.isActive = false;
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.ErrorMessages.Add("No course found to delete");

                        return Results.NotFound(response);
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while deleting the course");

                    response.isActive = false;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.ErrorMessages.Add("An error occurred while processing your request");

                    return Results.StatusCode((int)HttpStatusCode.InternalServerError);
                }
            }).WithName("DeleteCourse")
     .Produces<ApiResponse>(201)
     .Produces<ApiResponse>(404)
     .Produces<ApiResponse>(500)
     .WithTags("Course");

        }
    }
}
