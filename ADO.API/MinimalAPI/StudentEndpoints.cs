using Demo.Domain.AggregatesModel;
using Demo.Domain.ViewModels.Students;
using Demo.Infrastructure.Modules;
using Demo.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ADO.API.MinimalAPI;
public static class StudentEndpoints
{
    public static void MapStudentEndpoints(this IEndpointRouteBuilder app)
    {
        //GET  
        app.MapGet("/api/getStudent", async (IStudentService studentService, ILogger<Program> logger) =>
        {
            logger.Log(LogLevel.Information, "All students");

            ApiResponse response = new();
            try
            {
                var students = await studentService.GetAllStudentsAsync();

                if (students == null || !students.Any())
                {
                    response.isActive = false;
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.ErrorMessages.Add("No students found.");
                    return Results.NotFound(response);
                }

                response.Result = students;
                response.isActive = true;
                response.StatusCode = HttpStatusCode.OK;

                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                response.isActive = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ErrorMessages.Add("An error occurred while processing your request");

                return Results.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }).WithName("GetStudents")
  .Produces<ApiResponse>(201)
 .Produces<ApiResponse>(404)
 .Produces<ApiResponse>(500)
 .WithTags("Student");

        //GET ID

        app.MapGet("/api/getStudentById/{id:int}", async (IStudentService studentService, ILogger<Program> logger, int id) =>
        {
            ApiResponse response = new();

            try
            {
                logger.Log(LogLevel.Information, "Getting single student");

                var student = await studentService.GetStudentAsync(id);

                if (student == null)
                {
                    response.isActive = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.ErrorMessages.Add("No student found");
                }

                response.Result = student;
                response.isActive = true;
                response.StatusCode = HttpStatusCode.OK;

                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                response.isActive = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ErrorMessages.Add("An error occurred while processing your request");

                return Results.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }).WithName("GetSingeStudent")
 .Produces<ApiResponse>(201)
 .Produces<ApiResponse>(404)
 .Produces<ApiResponse>(500)
 .WithTags("Student");


        //Create
        app.MapPost("/api/createStudent/", async (IStudentService studentService, ILogger<Program> logger, [FromBody] StudentCreatedDTO createdDTO, IValidator<StudentCreatedDTO> validator) =>
        {

            logger.Log(LogLevel.Information, "All students");

            ApiResponse response = new();

            try
            {
                var resultValidation = await validator.ValidateAsync(createdDTO);
                if (!resultValidation.IsValid)
                {
                    response.ErrorMessages.AddRange(resultValidation.Errors.Select(x => x.ToString()));
                    return Results.BadRequest(response);
                }

                // Add student via the service
                await studentService.AddStudentAsync(createdDTO);

                // Set the response
                response.Result = createdDTO;
                response.isActive = true;
                response.StatusCode = HttpStatusCode.Created;

                // Return a Created result
                return Results.Created($"/api/getStudentById/{createdDTO.FirstName}", response); // Use StudentId instead of FirstName
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                response.isActive = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ErrorMessages.Add("An error occurred while processing your request");

                return Results.StatusCode((int)HttpStatusCode.InternalServerError);
            }

        }).WithName("CreateStudent")
.Produces<ApiResponse>(201)
 .Produces<ApiResponse>(404)
 .Produces<ApiResponse>(500)
 .WithTags("Student");

        //update
        app.MapPut("/api/updateStudent/", async (IStudentService studentService, ILogger<Program> logger, [FromBody] StudentUpdateDTO updateDTO, IValidator<StudentUpdateDTO> validator) =>
        {
            logger.Log(LogLevel.Information, "Updating student");

            ApiResponse response = new();

            try
            {
                var resultValidation = await validator.ValidateAsync(updateDTO);
                if (!resultValidation.IsValid)
                {
                    response.ErrorMessages.AddRange(resultValidation.Errors.Select(x => x.ToString()));
                    return Results.BadRequest(response);
                }

                await studentService.UpdateStudentAsync(updateDTO.StudentId, updateDTO);

                response.Result = updateDTO;  //"Student updated successfully";
                response.isActive = true;
                response.StatusCode = HttpStatusCode.OK;

                return Results.Created($"/api/getStudentById/{updateDTO.StudentId}", response);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                response.isActive = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ErrorMessages.Add("An error occurred while processing your request");

                return Results.StatusCode((int)HttpStatusCode.InternalServerError);
            }

        }).WithName("UpdateStudent")
  .Produces<ApiResponse>(201)
 .Produces<ApiResponse>(404)
 .Produces<ApiResponse>(500)
 .WithTags("Student");

        //delete
        app.MapDelete("/api/deleteStudent/{id:int}", async (IStudentService studentService, ILogger<Program> logger, int id) =>
        {
            logger.Log(LogLevel.Information, "Deleting student");

            ApiResponse response = new();

            try
            {
                bool isDeleted = await studentService.DeleteStudentAsync(id);

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
                    response.ErrorMessages.Add("No student found to delete");

                    return Results.NotFound(response);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                response.isActive = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ErrorMessages.Add("An error occurred while processing your request");

                return Results.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }).WithName("DeleteStudent")
 .Produces<ApiResponse>(201)
 .Produces<ApiResponse>(404)
 .Produces<ApiResponse>(500)
 .WithTags("Student");

    }




}
