export class api_endpoints {
    static readonly me = "api/user/me"
    static readonly user = "api/user"


    //Team API endpoints
    static readonly team = "api/team"


    //Personal API endpoints
    static readonly personalfolder = "api/personalfolder"

    //Password API endpoints
    static readonly getcertificatepassword = "api/password/certificate"
    static readonly getcredentialpassword = "api/password/credential"
    static readonly getpersonalpassword = "api/password/personal"

    //Login API endpoints
	static readonly userauthorization = "api/authorization"
	static readonly userregister = "api/register"

    //EmailNotification
    static readonly setnewpassword = "user/SetNewPassword"
    static readonly deleteverificateionrequest = "api/deleterequest"

    //Credential API endpoints
    static readonly credential = "api/credential"
    static readonly personalcredential = "api/credential/personal"
    static readonly givecredential = "api/credential/givecredential"

    //Certificate API endpoints
    static readonly certificate = "api/certificate/"
    static readonly certificatedownload = "api/certificate/download/"
    static readonly certificatekeydownload = "api/certificate/key/download"

    //Client API endpoints
    static readonly client = "api/client"
	static readonly clientdetails = "api/client/details"
    static readonly clientcredential = "api/client/credential"
    static readonly clientcertificate = "api/client/certificate"

}
