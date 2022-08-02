# bs.Auth

A simple implementation of JWT authentication for NET Core API projects.

You can import this project as external library in your API project and follow the above steps to start using it.

## Implements Models

First you have to implement all the required model.

### UserModel

This model persists usually in your application (maybe in your database) and you need it for main authentication porcess. 

```c#
 public class UserModel<T> : IUserModel<T>
    {
        public T Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public IRoleModel<T>[] Roles { get; set; }
		// [... everithing else you want ...]
    }
```

### RoleModel

```c#
 public class RoleModel<T> : IRoleModel<T>
    {
        public T Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        // [... everithing else you want ...]
    }
```

### UserViewModel

This is the retuned view model and you will create it in the authentication process.

```c#
public class UserViewModel<T> : IUserViewModel<T>
{
    public T Id { get; set; }
    public string Username { get; set; }
    public string Token { get; set; }
    public DateTime TokenExpireAt { get; set; }
    public string[] Roles { get; set; }
    // [... everithing else you want ...]
}
```

### UserAuthDto 

This is the DTO passed in authentication request. Ussually it becomes from the frontend.

```c#
public class UserAuthDto : IUserAuthDto
{
    public string Username  { get; set;}
    public string Password  { get; set;}
    // [... everithing else you want ...]
}
```

## Implements Service

Now we have to implement the auth service deriving from abstracted class BaseAuthService.

The only method we have to implement is the 'AuthenticateAsync' method. The above is an implementing example.

 

```c#
public class AuthService : BaseAuthService
{
     public AuthService(IAppSecuritySettingsModel securitySettings) : base(securitySettings)
     {
     }

​    public override async Task<IUserViewModel<T>> AuthenticateAsync<T>(IUserAuthDto userAuth)
​    {
​        // dummy autentication
​        if (userAuth.Password != "123")
​        {
​            throw new ApplicationException("Invalid credential");
​        }

​        // instead of retriving user model from db we create a mock
​        var user = new UserModel<T>
​        {
​            Id = default(T),
​            Username = "Pinco",
​            Password = "123",
​            Roles = new List<IRoleModel<T>>
​            {
​                new RoleModel<T>
​                {
​                    Id = default(T),
​                    Code = "admin",
​                    IsActive = true,
​                    Name = "Administrator"
​                }
​            }.ToArray()
​        };

​        var token = GenerateClaimsAndToken(user);

​        var userViewModel = new UserViewModel<T>
​        {
​            Id = user.Id,
​            Username = user.Username,
​            Token = token.Token,
​            TokenExpireAt = token.ExpireAt,
​            Roles = user.Roles.Select(r=>r.Code).ToArray()
​        };

​        return userViewModel;
​    }
}
```

## How to use it

To use JWT autentication in .Net Core app you ave to register the JWT Bearer authentication scheme at the bootstrap of your API.

So simply go on ur Startup.cs file and in the ConfigureServices methos add the above code.

Pay attention that we used the same **IAppSecuritySettingsModel** model needed by the 'AuthService' service to confugure the JWT Bearer option, so I fairly suggest you to register an instance of the AppSecuritySettingsModel class in ur DI container (better if you expose it as configuration file in your application).

```c#
// Setting authentication using JWT Token in request header
services.AddAuthentication(options =>
{
     options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
     options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
     options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
 })
     .AddJwtBearer(options =>
     {
         options.SaveToken = true;
         options.RequireHttpsMetadata = false;
         options.TokenValidationParameters = new TokenValidationParameters()
         {
             ValidateIssuer = securitySettings.ValidateIssuer,
             ValidateAudience = securitySettings.ValidateAudience,
             ValidAudience = securitySettings.ValidAudience,
             ValidIssuer = securitySettings.ValidIssuer,
             ClockSkew = TimeSpan.Zero,// It forces tokens to expire exactly at token expiration time instead of 5 minutes later
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securitySettings.Secret))
          };
      });
```
