global using FluentValidation;
global using HireMind.Api.Authorization;
global using HireMind.Application.Behaviors;
global using HireMind.Application.Commands.Authentication.Registration;
global using HireMind.Application.Commands.Authentication.ResetPassword;
global using HireMind.Application.Commands.BusinessCard;
global using HireMind.Application.Commands.JobApplication;
global using HireMind.Application.Commands.ManageJobs;
global using HireMind.Application.Commands.Shared;
global using HireMind.Application.Interfaces;
global using HireMind.Application.Queries.BusinessCard;
global using HireMind.Application.Queries.JobApplication;
global using HireMind.Application.Queries.ManageJobs;
global using HireMind.Application.Queries.Shared;
global using HireMind.Application.Security.Permissions;
global using HireMind.Application.Services;
global using HireMind.Domain;
global using HireMind.Domain.Dtos.JobApplication;
global using HireMind.Domain.Dtos.ManageJobs;
global using HireMind.Domain.Dtos.SharedDtos;
global using HireMind.Domain.IRepositories;
global using HireMind.Domain.Settings;
global using HireMind.Infrastructure;
global using HireMind.Infrastructure.Repositories;
global using HireMind.Infrastructure.Services;
global using MediatR;
global using Microsoft.AspNetCore.Authentication.JwtBearer;
global using Microsoft.AspNetCore.Authorization;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.EntityFrameworkCore;
global using HireMind.Application.Queries.Content;
global using Microsoft.IdentityModel.Tokens;
global using System.Net.Mail;
global using System.Text;
global using HireMind.Application.Commands.Chatbot;
global using HireMind.Domain.Dtos.AI;
global using HireMind.Application.Commands.Authentication.Tokens;
global using HireMind.Domain.Dtos.Authentication;
global using HireMind.Domain.Dtos.BusinessCard;
global using HireMind.Application.Commands.ApplicationStage;
global using HireMind.Application.Queries.ApplicationStage;
global using HireMind.Domain.Dtos.ApplicationStage;
global using HireMind.Domain.Dtos.UpdateApplicationStageStatusRequestDto;
global using HireMind.Application.Queries.HiringStages;
global using HireMind.Application.Queries.Seurity;
global using HireMind.Domain.Dtos.Security;
global using HireMind.Application.Commands.Security;





