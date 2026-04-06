global using HireMind.Domain.Dtos;
global using FluentValidation;
global using MediatR;
global using HireMind.Domain.IRepositories;
global using HireMind.Domain;
global using Mapster;
global using HireMind.Application.Interfaces;
global using Microsoft.AspNetCore.Http;
global using businessCardModel = HireMind.Domain.Entities.BCMS.BusinessCard;
global using jobModel = HireMind.Domain.Entities.HireMind.Job;
global using jobApplication = HireMind.Domain.Entities.HireMind.JobApplication;
global using lookupModel = HireMind.Domain.Entities.Shared.Lookup;
global using applicationStage = HireMind.Domain.Entities.HireMind.ApplicationStage;
global using UserModel = HireMind.Domain.Entities.Security.User;
global using HireMind.Domain.Dtos.ManageJobs;
global using HireMind.Domain.Entities;
global using HireMind.Domain.Dtos.JobApplication;
global using HireMind.Domain.Dtos.BusinessCard;
global using DocumentFormat.OpenXml.Packaging;
global using Microsoft.Extensions.Configuration;
global using System.Net.Http.Headers;
global using System.Text;
global using System.Text.Json;
global using UglyToad.PdfPig;
global using HireMind.Domain.Dtos.AI;
global using HireMind.Domain.Dtos.SharedDtos;
global using HireMind.Domain.Dtos.Authentication;
global using HireMind.Domain.Entities.Security;
global using HireMind.Domain.Settings;
global using Microsoft.Extensions.Options;
global using Microsoft.IdentityModel.JsonWebTokens;
global using Microsoft.IdentityModel.Tokens;
global using System.Security.Claims;
global using System.Security.Cryptography;





