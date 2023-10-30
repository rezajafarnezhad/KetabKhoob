﻿using System.Net;
using Common.AspNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Infrastructure.AuthorizeAttr;
using Shop.Application.SiteEntities.Banners.Create;
using Shop.Application.SiteEntities.Banners.Edit;
using Shop.Domain.RoleAgg.Enums;
using Shop.Presentation.Facade.SiteEntities.Banner;
using Shop.Query.SiteEntities.DTOs;

namespace Shop.Api.Controllers;

[PermissionChecker(Permission.BannerManagement)]
public class BannerController : BaseApiController
{
    private readonly IBannerFacade _bannerFacade;

    public BannerController(IBannerFacade bannerFacade)
    {
        _bannerFacade = bannerFacade;
    }

    [HttpGet("{id}")]

    public async Task<ApiResult<BannerDto>> GetById(long id)
    {
        return QueryResult(await _bannerFacade.GetBannerById(id));
    }

    [HttpGet("GetList")]
    [AllowAnonymous]
    public async Task<ApiResult<List<BannerDto>>> GetByList()
    {
        return QueryResult(await _bannerFacade.GetBanners());
    }

    [HttpPost]
    public async Task<ApiResult<long>> Create([FromForm] CreateBannerCommand command)
    {
        var result = await _bannerFacade.CreateBanner(command);
        var url = Url.Action("GetBYId", "Banner", new { id = result.Data }, Request.Scheme);
        return CommandResult(result, HttpStatusCode.Created, url);
    }

    [HttpPut]
    public async Task<ApiResult> Edit([FromForm] EditBannerCommand command)
    {
        return CommandResult(await _bannerFacade.EditBanner(command));
    }

}