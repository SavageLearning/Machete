## Expected configuration  
  
```
{
  "issuer": "https://identity.machetessl.org/id",
  "jwks_uri": "https://identity.machetessl.org/id/.well-known/jwks",
  "authorization_endpoint": "https://identity.machetessl.org/id/connect/authorize",
  "token_endpoint": "https://identity.machetessl.org/id/connect/token",
  "userinfo_endpoint": "https://identity.machetessl.org/id/connect/userinfo",
  "end_session_endpoint": "https://identity.machetessl.org/id/connect/endsession",
  "check_session_iframe": "https://identity.machetessl.org/id/connect/checksession",
  "revocation_endpoint": "https://identity.machetessl.org/id/connect/revocation",
  "introspection_endpoint": "https://identity.machetessl.org/id/connect/introspect",
  "frontchannel_logout_supported": true,
  "frontchannel_logout_session_supported": true,
  "scopes_supported": [
    "openid",
    "profile",
    "email",
    "roles",
    "offline_access",
    "api"
  ],
  "claims_supported": [
    "sub",
    "name",
    "family_name",
    "given_name",
    "middle_name",
    "nickname",
    "preferred_username",
    "profile",
    "picture",
    "website",
    "gender",
    "birthdate",
    "zoneinfo",
    "locale",
    "updated_at",
    "email",
    "email_verified",
    "role"
  ],
  "response_types_supported": [
    "code",
    "token",
    "id_token",
    "id_token token",
    "code id_token",
    "code token",
    "code id_token token"
  ],
  "response_modes_supported": [
    "form_post",
    "query",
    "fragment"
  ],
  "grant_types_supported": [
    "authorization_code",
    "client_credentials",
    "password",
    "refresh_token",
    "implicit"
  ],
  "subject_types_supported": [
    "public"
  ],
  "id_token_signing_alg_values_supported": [
    "RS256"
  ],
  "code_challenge_methods_supported": [
    "plain",
    "S256"
  ],
  "token_endpoint_auth_methods_supported": [
    "client_secret_post",
    "client_secret_basic"
  ]
}
```  
  
##Expected JWKS example
  
```
{
  "keys": [
    {
      "kty": "RSA",
      "use": "sig",
      "kid": "someLongAssUniqueValueof23c",
      "x5t": "someLongAssUniqueValueof23c",
      "e": "the exponent AQAB",
      "n": "a long-ass modulus",
      "x5c": [
        "a url-friendly base-64 encoded representation of the x509 certificate as a DER"
      ]
    }
  ]
}
```  

##Angular app login request  
  
```
https://localhost:63374/id/connect/authorize
?client_id=machete-ui-local-embedded
&redirect_uri=http%3A%2F%2Flocalhost%3A4213%2FV2%2Fauthorize
&response_type=id_token%20token
&scope=openid%20email%20roles%20api%20profile
&state={state}
&nonce={nonce}
```  

##IdentityServer3 Response
```
HTTP/1.1 302 Found
Location: https://identity.machetessl.org/id/login?signin={same-GUID}
Server: Microsoft-IIS/8.5
Set-Cookie:
    SignInMessage.{same-GUID}={computed-value};
    path=/id;
    secure; HttpOnly
X-Powered-By: ASP.NET
Date: Fri, 25 Jan 2019 03:36:02 GMT
Content-Length: 0
```

Our goal: Allow them to obtain authorization and pass through if
they are already logged in, or login with what they've already passed
in and grant a JWT; then, send it to:

HTTP/1.1 302 Found
Location: https://casa.machetessl.org/V2/authorize
`#id_token=the first JWT
 &access_token=the SECOND JWT wtf?
 &token_type=Bearer
 &expires_in=3600
 &scope=openid%20email%20roles%20api%20profile
 &state={state}
 &session_state=tLaH1bLFMi55B0_6R5fyL9jldbzwmwyCTwBVnpSXXS4.ec3183d0f51f277cfed7340853e91054

Server: Microsoft-IIS/8.5
Set-Cookie: idsvr.clients=WyJtYWNoZXRlLWNhc2EtcHJvZCJd; path=/id; secure; HttpOnly
X-Powered-By: ASP.NET
Date: Sat, 26 Jan 2019 06:21:34 GMT
Content-Length: 0

