using Demo.Domain.AggregatesModel;
using Demo.Domain.ViewModels.Students;
using Demo.Infrastructure.Modules;
using Demo.Infrastructure.Services;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
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

            var students = studentService.GetAllStudents();

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

        }).WithName("GetStudents")
          .Produces<ApiResponse>(200)
          .Produces(400);

        //GET ID

        app.MapGet("/api/getStudentById/{id:int}", async (IStudentService studentService, ILogger<Program> logger, int id) =>
        {
            logger.Log(LogLevel.Information, "Getting single student");

            ApiResponse response = new();
            var student = studentService.GetStudent(id);

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


        }).WithName("GetSingeStudent")
        .Produces<ApiResponse>(200)
        .Produces(400);


        //Create
        app.MapPost("/api/createStudent/", async (IStudentService studentService, StudentCreatedDTO createdDTO, IValidator<StudentCreatedDTO> validator) =>
        {
            ApiResponse response = new();

            var resultValidation = await validator.ValidateAsync(createdDTO);
            if (!resultValidation.IsValid)
            {
                response.ErrorMessages.AddRange(resultValidation.Errors.Select(x => x.ToString()));
                return Results.BadRequest(response);
            }

            // Add student via the service
            studentService.AddStudent(createdDTO);

            // Set the response
            response.Result = createdDTO;
            response.isActive = true;
            response.StatusCode = HttpStatusCode.Created;

            // Return a Created result
            return Results.Created($"/api/getStudentById/{createdDTO.FirstName}", response); // Use StudentId instead of FirstName

        }).WithName("CreateStudent")
         .Produces<ApiResponse>(201)
         .Produces(400);


        //update
        app.MapPut("/api/updateStudent/", async (IStudentService studentService, ILogger<Program> logger, [FromBody] StudentUpdateDTO updateDTO, IValidator<StudentUpdateDTO> validator) =>
        {
            logger.Log(LogLevel.Information, "Updating student");

            ApiResponse response = new();

            var resultValidation = await validator.ValidateAsync(updateDTO);
            if (!resultValidation.IsValid)
            {
                response.ErrorMessages.AddRange(resultValidation.Errors.Select(x => x.ToString()));
                return Results.BadRequest(response);
            }

            studentService.UpdateStudent(updateDTO.StudentId, updateDTO);

            response.Result = updateDTO;  //"Student updated successfully";
            response.isActive = true;
            response.StatusCode = HttpStatusCode.OK;

            return Results.Created($"/api/getStudentById/{updateDTO.StudentId}", response);

        }).WithName("UpdateStudent")
       .Produces<ApiResponse>(200)
       .Produces(400);

        //delete
        app.MapDelete("/api/deleteStudent/{id:int}", async (IStudentService studentService, ILogger<Program> logger, int id) =>
        {
            logger.Log(LogLevel.Information, "Deleting student");

            ApiResponse response = new();

            try
            {
                bool isDeleted = await studentService.DeleteStudent(id);

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
                logger.LogError(ex, "An error occurred while deleting the student");

                response.isActive = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ErrorMessages.Add("An error occurred while processing your request");

                return Results.StatusCode((int)HttpStatusCode.InternalServerError);
            }
        }).WithName("DeleteStudent")
 .Produces<ApiResponse>(200)
 .Produces<ApiResponse>(404)
 .Produces<ApiResponse>(500);


    }




}
