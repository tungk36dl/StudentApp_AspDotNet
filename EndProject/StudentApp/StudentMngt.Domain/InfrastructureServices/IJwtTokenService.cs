﻿using System.Security.Claims;

namespace StudentMngt.Domain.InfrastructureServices;

public interface IJwtTokenService
{
    string GenerateAccessToken(IEnumerable<Claim> claims);

}