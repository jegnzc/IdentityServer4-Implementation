using System.Security.Claims;
using IdentityModel;
using IdentityServer4.Test;
using Microsoft.AspNetCore.Identity;

namespace IdentityServer.Test;

internal class Users
{
    public static List<TestUser> Get()
    {
        return new List<TestUser> {
            new TestUser {
                SubjectId = "5BE86359-073C-434B-AD2D-A3932222DABE",
                Username = "scott",
                Password = "password",
                Claims = new List<Claim> {
                    new Claim(JwtClaimTypes.Email, "scott@scottbrady91.com"),
                    new Claim(JwtClaimTypes.Role, "admin")
                }
            },
            new TestUser {
                SubjectId = "5BE86359-073C-434B-AD2D-A3932222DDBE",
                Username = "1234",
                Password = "1234",
                Claims = new List<Claim> {
                    new Claim(JwtClaimTypes.Email, "scott@scottbrady91.com"),
                    new Claim(JwtClaimTypes.Role, "admin")
                }
            },
            new TestUser {
                SubjectId = "5BE86359-073C-434B-AD2D-A3932222DCBE",
                Username = "abcd",
                Password = "abcd",
                Claims = new List<Claim> {
                    new Claim(JwtClaimTypes.Email, "scott@scottbrady91.com"),
                    new Claim(JwtClaimTypes.Role, "admin")
                }
            }
        };
    }
}