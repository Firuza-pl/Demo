# Demo

this is just a test demo of how i can use MinimalAPI with ado.net commands instead of a controller

Project Structure Overview

1. Repositories
This folder contains classes responsible for interacting with the data source and performing CRUD operations. These classes follow the repository pattern.
CourseRepository.cs: Implements the repository pattern for managing Course entities.
ICourseRepository.cs: Interface defining the contract for the CourseRepository.
StudentRepository.cs: Implements the repository pattern for managing Student entities.
IStudentRepository.cs: Interface defining the contract for the StudentRepository.

2. Services
This folder contains the service layer classes, which handle the business logic of the application. These services often use the repositories to interact with the data layer.

CourseService.cs: Contains business logic related to courses.
ICourseService.cs: Interface defining the contract for the CourseService.
StudentService.cs: Contains business logic related to students.
IStudentService.cs: Interface defining the contract for the StudentService.

3. Demo.Models
This folder holds the models that represent the data structure used within the application.
AggregatesModel: Contains models that represent aggregates, which are a cluster of related objects that are treated as a single unit.
Attributes: Contains custom attributes used for model validation or other purposes.
ViewModels: Contains models that are specifically designed to be used in the view layer, often for data binding in the UI.

4. ADO.API
This is the main folder for the API project, containing the Minimal API and Validator configurations.

MinimalAPI
This subfolder contains files related to the Minimal API implementation.

Validator
This subfolder contains validators that enforce rules and constraints on the data models.

StudentCreatedDTOValidator.cs: Validator for the student creation DTO.
StudentUpdateDTOValidator.cs: Validator for the student update DTO.
ValidatorConfigurations.cs: Configuration settings for the validators.

5. Demo.Infrastructure
This folder holds infrastructure-related files and configurations.

Database
This subfolder contains classes related to database context and management.
ApiResponse.cs: A class to standardize API responses.
LogicModule.cs: Contains business logic that may be shared across different services or components.
