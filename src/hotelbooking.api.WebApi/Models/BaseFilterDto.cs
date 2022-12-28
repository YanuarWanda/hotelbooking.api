﻿using Microsoft.AspNetCore.Mvc;

namespace hotelbooking.api.WebApi.Models;

public class BaseFilterDto
{
	[FromQuery(Name = "p")] public int Page { get; set; } = 1;
	[FromQuery(Name = "s")] public int PageSize { get; set; } = 10;
}