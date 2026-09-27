import { AuthConfig } from "angular-oauth2-oidc"

export const OidcFlowConfig: AuthConfig = {
    issuer: "https://adfs.test.local/adfs",
    redirectUri: "https://cm.test.local:4200/login/oauth/callback",
    clientId: "b9443624-540c-4fef-8107-1dcedcd487d1",
    scope: "openid email profile allatclaims",
    resource: "https://cm.test.local:4200/api/",
    responseType: "code",
	clearHashAfterLogin: true,
	logoutUrl: "https://adfs.test.local/adfs/oauth2/logout",
    postLogoutRedirectUri: "https://cm.test.local",
	tokenEndpoint: "https://adfs.test.local/adfs/oauth2/token/",
    disablePKCE: true

}

