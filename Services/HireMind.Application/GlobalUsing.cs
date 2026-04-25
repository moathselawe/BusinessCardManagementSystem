global using DocumentFormat.OpenXml.Packaging;
global using FluentValidation;
global using HireMind.Application.Interfaces;
global using HireMind.Domain;
global using HireMind.Domain.Dtos.AI;
global using HireMind.Domain.Dtos.Authentication;
global using HireMind.Domain.Dtos.BusinessCard;
global using HireMind.Domain.Dtos.JobApplication;
global using HireMind.Domain.Dtos.ManageJobs;
global using HireMind.Domain.Dtos.Security;
global using HireMind.Domain.Dtos.SharedDtos;
global using HireMind.Domain.Entities.Security;
global using HireMind.Domain.IRepositories;
global using HireMind.Domain.Settings;
global using Mapster;
global using MediatR;
global using Microsoft.AspNetCore.Http;
global using Microsoft.Extensions.Configuration;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.Tokens;
global using System.Net.Http.Headers;
global using System.Security.Claims;
global using System.Security.Cryptography;
global using System.Text;
global using System.Text.Json;
global using UglyToad.PdfPig;
global using HireMind.Domain.Dtos.Public;
global using applicationStage = HireMind.Domain.Entities.HireMind.ApplicationStage;
global using businessCardModel = HireMind.Domain.Entities.BCMS.BusinessCard;
global using jobApplication = HireMind.Domain.Entities.HireMind.JobApplication;
global using jobModel = HireMind.Domain.Entities.HireMind.Job;
global using lookupModel = HireMind.Domain.Entities.Shared.Lookup;
global using UserModel = HireMind.Domain.Entities.Security.User;





